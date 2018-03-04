using System;
using Gtk;


namespace BlenderRenderController
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Services.Settings.Init(true);
            Services.Scripts.Init();


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
