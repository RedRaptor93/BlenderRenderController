using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using BRClib.Scripts;
using System.Windows.Forms;
using System.IO;

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
            
            Services.Settings.Init();
            Services.Scripts.Init(Services.Settings.BaseDir);

            NlogSetup();

            var cmdFile = args.Where(a => Path.GetExtension(a) == ".blend")
                              .FirstOrDefault();


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
