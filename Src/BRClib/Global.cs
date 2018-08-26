using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using NLog;

namespace BRClib
{
    using Env = Environment;

    public static class Global
    {
        public static void Init(bool portableMode)
        {
            GetProjInfoScript = FindScript(PyGetProjInfo);
            MixdownScript = FindScript(PyMixdownAudio);
            _configFilePath = FindFile(SETTINGS_FILE);

            //  set config file path if it was not found
            if (_configFilePath == null)
            {
                if (portableMode) {
                    _configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SETTINGS_FILE);
                } else {
                    _configFilePath = Path.Combine(Env.GetFolderPath(Env.SpecialFolder.ApplicationData), APPDATA_DIR, SETTINGS_FILE);
                }
            }

            Settings = Load(_configFilePath);

            NlogSetup(portableMode);
        }


        public static ConfigModel Settings { get; private set; }

        public static string GetProjInfoScript { get; private set; }
        public static string MixdownScript { get; private set; }

        public static void SaveSettings()
        {
            SaveInternal(Settings, _configFilePath);
        }

        public static bool CheckProgramPaths()
        {
            bool blenderFound = true, ffmpegFound = true;
            string ePath;

            if (!File.Exists(Settings.BlenderProgram))
            {
                ePath = FindProgram("blender");
                blenderFound = ePath != null;

                if (blenderFound) Settings.BlenderProgram = ePath;
            }

            if (!File.Exists(Settings.FFmpegProgram))
            {
                ePath = FindProgram("ffmpeg");
                ffmpegFound = ePath != null;

                if (ffmpegFound) Settings.FFmpegProgram = ePath;
            }

            return blenderFound && ffmpegFound;
        }

        // workaround Process.Start not working on .NET core
        public static void ShellOpen(string file_uri)
        {
            var stInfo = new ProcessStartInfo(file_uri) {
                UseShellExecute = true
            };
            Process.Start(stInfo);
        }


        static string _configFilePath;

        const string SETTINGS_FILE = "brc_settings.json";
        const string PyGetProjInfo = "get_project_info.py";
        const string PyMixdownAudio = "mixdown_audio.py";
        const string APPDATA_DIR = "BlenderRenderController";


        public static string FindProgram(string name, params string[] morePaths)
        {
            if (Env.OSVersion.Platform == PlatformID.Win32NT)
            {
                name += ".exe";
            }

            var EnvPATH = Env.GetEnvironmentVariable("PATH").Split(Path.PathSeparator);
            var searchPath = new List<string>(EnvPATH.Length + 1 + morePaths.Length);
            searchPath.Add(AppDomain.CurrentDomain.BaseDirectory);
            searchPath.AddRange(EnvPATH);
            searchPath.AddRange(morePaths);

            var found = searchPath.Select(p => Path.Combine(p, name)).FirstOrDefault(File.Exists);

            return found;
        }

        static string FindScript(string name)
        {
            string[] dirs = {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", name),
                Path.Combine(Env.GetFolderPath(Env.SpecialFolder.ApplicationData), APPDATA_DIR, "Scripts", name)
            };

            // Script files must exist
            var found = dirs.First(File.Exists);

            return found;
        }

        static string FindFile(string name)
        {
            string[] dirs = {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name),
                Path.Combine(Env.GetFolderPath(Env.SpecialFolder.ApplicationData), APPDATA_DIR, name)
            };

            var found = dirs.FirstOrDefault(File.Exists);

            return found;
        }

        static ConfigModel Load(string configFile)
        {
            if (File.Exists(configFile))
            {
                return JsonConvert.DeserializeObject<ConfigModel>(File.ReadAllText(configFile));
            }

            // create file w/ default settings
            var def = new ConfigModel();
            SaveInternal(def, configFile);

            return def;
        }

        static void SaveInternal(ConfigModel def, string filepath)
        {
            var json = JsonConvert.SerializeObject(def, Formatting.Indented);
            File.WriteAllText(filepath, json);
        }

        static void NlogSetup(bool portableMode)
        {
            LogLevel lLvl;

            switch (Settings.LoggingLevel)
            {
                case 1: lLvl = LogLevel.Info; break;
                case 2: lLvl = LogLevel.Trace; break;
                default: return;
            }

            string fileTgt = "brclogfile";
            if (portableMode) fileTgt += "_p";

            var target = LogManager.Configuration.FindTargetByName(fileTgt);
            LogManager.Configuration.AddRule(lLvl, LogLevel.Fatal, target, "*");

            LogManager.ReconfigExistingLoggers();
        }

    }
}
