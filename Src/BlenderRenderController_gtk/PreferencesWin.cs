using System;
using System.IO;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using Global = BRClib.Global;

namespace BlenderRenderController
{
    class PreferencesWin : Dialog
    {
#pragma warning disable 649
        [UI] FileChooserButton blenderChooser, ffmpegChooser;
        [UI] CheckButton showTooltipsCk, delChunksCk;
        [UI] ComboBox logginLvlCb;
        [UI] Button btnOk;
#pragma warning restore 649

        private PreferencesWin(Builder builder, string root)
            : base(builder.GetObject(root).Handle)
        {
            builder.Autoconnect(this);
        }

        public PreferencesWin()
            : this(Glade.LoadUI("Dialogs.glade"), "PrefWin")
        {
            LoadSettings();
            btnOk.Clicked += On_PrefOk_Clicked;
        }

        void LoadSettings()
        {
            string blender = Global.Settings.BlenderProgram,
                   ffmpeg = Global.Settings.FFmpegProgram;

            if (File.Exists(blender))
            {
                blenderChooser.SetFilename(blender);
            }

            if (File.Exists(ffmpeg))
            {
                ffmpegChooser.SetFilename(ffmpeg);
            }

            showTooltipsCk.Active = Global.Settings.DisplayToolTips;
            delChunksCk.Active = Global.Settings.DeleteChunksFolder;
            logginLvlCb.Active = Global.Settings.LoggingLevel;
        }


        void On_BlenderFileSet(object s, EventArgs e)
        {
            var chooser = (FileChooserButton)s;
            Global.Settings.BlenderProgram = chooser.Filename;
        }

        void On_FFmpegFileSet(object s, EventArgs e)
        {
            var chooser = (FileChooserButton)s;
            Global.Settings.FFmpegProgram = chooser.Filename;
        }

        void On_Showtooltips_Toggle(object s, EventArgs e)
        {
            var tgl = (CheckButton)s;
            Global.Settings.DisplayToolTips = tgl.Active;
        }

        void On_DeleteChunks_Toggle(object s, EventArgs e)
        {
            var tgl = (CheckButton)s;
            Global.Settings.DeleteChunksFolder = tgl.Active;
        }

        void On_LoggingLvl_Changed(object s, EventArgs e)
        {
            var cb = (ComboBox)s;
            Global.Settings.LoggingLevel = cb.Active;
        }

        void On_PrefOk_Clicked(object s, EventArgs e)
        {
            Hide();
        }

    }
}
