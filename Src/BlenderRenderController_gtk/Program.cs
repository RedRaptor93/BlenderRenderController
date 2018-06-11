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
            Global.Init(true);
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
