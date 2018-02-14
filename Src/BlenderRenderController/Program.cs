// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using BRClib.Scripts;
using System.Windows.Forms;
using System.IO;
using CommandLine;
using BlenderRenderController.Infra.Cmd;
using BlenderRenderController.Services;

namespace BlenderRenderController
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // parse cmd args
            var cmdResult = Parser.Default.ParseArguments<Arguments>(args);
            var arguments = (cmdResult as Parsed<Arguments>)?.Value;

            string cmdFile = arguments.BlendFile ?? null;
            string portableStr = arguments.Portable ?? System.Configuration.ConfigurationManager.AppSettings["portable"];
            bool portable = bool.TryParse(portableStr, out bool ptb) ? ptb : false;

            Settings.Init(portable);
            var scriptsDir = Path.Combine(Settings.BaseDir, "scripts");

            switch (arguments?.ClearAction)
            {
                case "scripts":
                    if (Directory.Exists(scriptsDir)) Directory.Delete(scriptsDir, true);
                    break;
                case "settings":
                    Settings.ResetDefaults();
                    break;
                case "all":
                    var versionFile = Path.Combine(Settings.BaseDir, "ver");
                    if (File.Exists(versionFile)) File.Delete(versionFile);
                    if (Directory.Exists(scriptsDir)) Directory.Delete(scriptsDir, true);
                    Settings.ResetDefaults();
                    break;
                case null:
                default:
                    break;
            }

            Scripts.Init();

            NlogSetup();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            BrcForm form;

            if (cmdFile != null)
            {
                form = new BrcForm(cmdFile);
            }
            else
            {
                form = new BrcForm();
            }

            Application.Run(form);
        }


        static void NlogSetup()
        {
            var _sett = Services.Settings.Current;
            LogLevel lLvl;

            switch (_sett.LoggingLevel)
            {
                case 1: lLvl = LogLevel.Info; break;
                case 2: lLvl = LogLevel.Trace; break;
                default: return;
            }

            string fileTgt = "brclogfile";
            if (Services.Settings.Portable)
            {
                fileTgt += "_p";
            }

            var target = LogManager.Configuration.FindTargetByName(fileTgt);
            LogManager.Configuration.AddRule(lLvl, LogLevel.Fatal, fileTgt, "*");

            LogManager.ReconfigExistingLoggers();
        }

    }
}
