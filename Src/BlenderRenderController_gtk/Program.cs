using System;
using Gtk;
using Global = BRClib.Global;

namespace BlenderRenderController
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var pstr = Environment.GetEnvironmentVariable("BRC_PORTABLE_MODE");
            bool portable_mode = false;
            bool.TryParse(pstr, out portable_mode);

            Global.Init(portable_mode);

            Application.Init();

            // TODO: figure out a way to make GTK look for the images in the Resources dir.
            // as a workaround, set the CWD to Resources dir
            var resDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            Environment.CurrentDirectory = resDir;

            var app = new Application("org.BlenderRenderController", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var win = new BrcMain();
            app.AddWindow(win);

            win.Show();

            Application.Run();
        }

    }
}
