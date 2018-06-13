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
            string pstr = System.Configuration.ConfigurationManager.AppSettings["portable"];
            bool portable = bool.TryParse(pstr, out bool r) ? r : false;

            Global.Init(portable);
            Application.Init();

            var app = new Application("org.BlenderRenderController", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var win = new BrcMain();
            app.AddWindow(win);

            win.Show();

            Application.Run();
        }

    }
}
