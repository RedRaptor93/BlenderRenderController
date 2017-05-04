﻿using BlenderRenderController.newLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlenderRenderController
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Register LogServices Here
            LogService.Log.RegisterLogSevice(new FileLogger());
            LogService.Log.RegisterLogSevice(new ConsoleLogger());

            Application.Run(new MainForm());
        }

        //string[] Args = Environment.GetCommandLineArgs();
        //public static class ArgHolder
        //{
        //    public static Args { get; set; }
        //}
}
}
