﻿// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using BlenderRenderController.Infra;
using BRClib;
using BRClib.Scripts;
using Newtonsoft.Json;

namespace BlenderRenderController.Services
{
    using Env = Environment;

    static class Settings
    {
        static string _baseDir;
        const string SETTINGS_FILE = "brc_settings.json";
        const string BRC_VER = "BRC_VER";

        static BrcSettings _setts;
        static bool _portable;
        //static IScriptPath _scriptPaths;

        internal static BrcSettings Current
        {
            get
            {
                return _setts;
            }
        }

        public static bool Portable
        {
            get
            {
                return _portable;
            }
        }

        internal static string BaseDir => _baseDir;



        internal static void Init(bool portableMode = false)
        {
            _portable = portableMode;

            _baseDir = Portable ? Env.CurrentDirectory : Dirs.AppData;

            Env.SetEnvironmentVariable(BRC_VER, Assembly.GetExecutingAssembly().GetName().Version.ToString());

            _setts = LoadInternal(Path.Combine(_baseDir, SETTINGS_FILE));
        }

        public static bool CheckCorrectConfig()
        {
            bool blenderFound = true, ffmpegFound = true;

            if (!File.Exists(_setts.BlenderProgram))
            {
                var ePath = SearchPathForProgram("blender");
                blenderFound = ePath != null;

                if (blenderFound) _setts.BlenderProgram = ePath;
            }

            if (!File.Exists(_setts.FFmpegProgram))
            {
                var ePath = SearchPathForProgram("ffmpeg");
                ffmpegFound = ePath != null;

                if (ffmpegFound) _setts.FFmpegProgram = ePath;
            }

            return blenderFound && ffmpegFound;
        }

        public static bool CheckVerFile(bool createFile)
        {
            var brc_ver = Env.GetEnvironmentVariable(BRC_VER);

            var verFile = Path.Combine(_baseDir, "ver");

            if (!File.Exists(verFile) && createFile)
            {
                File.WriteAllText(verFile, brc_ver);
                return false;
            }

            var savedVer = File.ReadAllText(verFile);

            if (savedVer == brc_ver)
            {
                return true;
            }

            if (createFile)
                File.WriteAllText(verFile, brc_ver);

            return false;
        }

        static string SearchPathForProgram(string program)
        {
            if (Env.OSVersion.Platform == PlatformID.Win32NT)
            {
                program += ".exe";
            }

            var envPATH = Env.GetEnvironmentVariable("PATH").Split(Path.PathSeparator);

            var exePath = envPATH.Select(x => Path.Combine(x, program))
                                    .FirstOrDefault(File.Exists);

            return exePath;
        }

        private static BrcSettings GetDefaults()
        {
            string defBlenderDir, defFFmpegDir;
            string blenderExe = "blender";
            string ffmpegExe = "ffmpeg";

            switch (Env.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    // GetFolderPath doesn't work and only returns the 32bit ProgramFiles
                    string envVar = Env.Is64BitOperatingSystem ? "ProgramW6432" : "ProgramFiles";
                    var programFiles = Env.GetEnvironmentVariable(envVar);

                    defBlenderDir = Path.Combine(programFiles, "Blender Foundation", "Blender");
                    defFFmpegDir = "";

                    var ext = ".exe";
                    blenderExe += ext;
                    ffmpegExe += ext;
                    break;
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    // TODO: Mac specific guess?
                    // remember: Platform == Unix on both Linux and MacOSX
                default:
                    defBlenderDir = defFFmpegDir = "/usr/bin";
                    break;
            }

            return new BrcSettings
            {
                BlenderProgram = Path.Combine(defBlenderDir, blenderExe),
                FFmpegProgram = Path.Combine(defFFmpegDir, ffmpegExe),
                LoggingLevel = 0,
                DisplayToolTips = true,
                AfterRender = AfterRenderAction.MIX_JOIN,
                Renderer = Renderer.BLENDER_RENDER,
                RecentProjects = new RecentBlendsCollection(),
                DeleteChunksFolder = false
            };
        }

        public static void Save()
        {
            SaveInternal(_setts, Path.Combine(_baseDir, SETTINGS_FILE));
        }

        public static void Load()
        {
            _setts = LoadInternal(Path.Combine(_baseDir, SETTINGS_FILE));
        }

        public static void ResetDefaults()
        {
            _setts = GetDefaults();
            Save();
        }

        private static BrcSettings LoadInternal(string settingsPath)
        {
            if (File.Exists(settingsPath))
            {
                return JsonConvert.DeserializeObject<BrcSettings>(File.ReadAllText(settingsPath));
            }
            else
            {
                // create settings file
                var def = GetDefaults();
                SaveInternal(def, settingsPath);

                return def;
            }
        }

        private static void SaveInternal(BrcSettings def, string settingsPath)
        {
            var json = JsonConvert.SerializeObject(def, Formatting.Indented);

            var settingsDir = Directory.GetParent(settingsPath).FullName;

            if (!Directory.Exists(settingsDir))
            {
                Directory.CreateDirectory(settingsDir);
            }

            File.WriteAllText(settingsPath, json);
        }
    }


}
