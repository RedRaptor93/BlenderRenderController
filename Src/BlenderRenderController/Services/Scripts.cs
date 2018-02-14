﻿// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System.IO;
using System.Reflection;
using System.Text;

namespace BlenderRenderController.Services
{
    static class Scripts
    {
        const string PyGetProjInfo = "get_project_info.py";
        const string PyMixdownAudio = "mixdown_audio.py";

        static string _gpinfo, _mix, _scriptF;

        public static string GetProjectInfo => _gpinfo;

        public static string MixdownAudio => _mix;

        public static string ScriptsFolder => _scriptF;


        public static void Init(string baseDir)
        {
            _scriptF = Path.Combine(baseDir, "scripts");

            if (!Directory.Exists(_scriptF))
            {
                Directory.CreateDirectory(_scriptF);
            }

            var overwrite = Settings.CheckVerFile(true);
            _gpinfo = Path.Combine(_scriptF, PyGetProjInfo);
            _mix = Path.Combine(_scriptF, PyMixdownAudio);

            if (!File.Exists(_gpinfo) || overwrite)
            {
                WriteScriptToDisk(PyGetProjInfo, _scriptF);
            }
            if (!File.Exists(_mix) || overwrite)
            {
                WriteScriptToDisk(PyMixdownAudio, _scriptF);
            }
        }

        public static void Init() => Init(Settings.BaseDir);


        private static void WriteScriptToDisk(string resourceName, string scriptsDir)
        {
            var filePath = Path.Combine(scriptsDir, resourceName);

            // scripts are embedded in the BRCLib assembly
            var assembly = Assembly.GetAssembly(typeof(BRClib.Chunk));
            var resourcePath = assembly.GetName().Name + ".Scripts." + resourceName;

            using (var resStream = assembly.GetManifestResourceStream(resourcePath))
            using (var fileStream = File.Create(filePath))
            {
                string genHeader = "# Generated by BRC, do not modify!\n";
                byte[] header = Encoding.UTF8.GetBytes(genHeader);

                fileStream.Write(header, 0, header.Length);

                // copy contents to file
                resStream.Seek(0, SeekOrigin.Begin);
                resStream.CopyTo(fileStream);
            }
        }

    }
}
