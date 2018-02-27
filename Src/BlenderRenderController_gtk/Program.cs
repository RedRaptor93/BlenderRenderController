using System;
using System.Reflection;
using Gtk;


namespace BlenderRenderController
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Report();

            Application.Init();

            var app = new Application("org.BlenderRenderController", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var win = new BrcMain();
            app.AddWindow(win);

            win.Show();

            Application.Run();
        }

        private static void Report()
        {
            System.Console.WriteLine("Report start");
            
            var assembly = Assembly.GetExecutingAssembly();
            var resNames = assembly.GetManifestResourceNames();

            System.Console.WriteLine("Resources:");
            
            foreach (var item in resNames)
            {
                System.Console.WriteLine("\t" + item);
            }

            System.Console.WriteLine("Report end");
        }
    }
}
