using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using Settings = BlenderRenderController.Services.Settings;

namespace BlenderRenderController
{
    class PreferencesWin : Dialog
    {
#pragma warning disable 649
        [UI] FileChooserButton blenderChooser, ffmpegChooser;
        [UI] CheckButton showTooltipsCk, delChunksCk;
        [UI] ComboBox logginLvlCb;
#pragma warning restore 649

        private PreferencesWin(Builder builder, string root)
            : base(builder.GetObject(root).Handle)
        {
            builder.Autoconnect(this);
            LoadSettings();
        }

        public PreferencesWin()
            : this(Glade.Load("PrefWin.glade"), "PrefWin")
        {}

        void LoadSettings()
        {
            string blender = Settings.Current.BlenderProgram,
                   ffmpeg = Settings.Current.FFmpegProgram;

            if (File.Exists(blender))
            {
                blenderChooser.SetFilename(blender);
            }

            if (File.Exists(ffmpeg))
            {
                ffmpegChooser.SetFilename(ffmpeg);
            }

            showTooltipsCk.Active = Settings.Current.DisplayToolTips;
            delChunksCk.Active = Settings.Current.DeleteChunksFolder;
            logginLvlCb.Active = Settings.Current.LoggingLevel;
        }


        void On_BlenderFileSet(object s, EventArgs e)
        {
            var chooser = (FileChooserButton)s;
            Settings.Current.BlenderProgram = chooser.Filename;
        }

        void On_FFmpegFileSet(object s, EventArgs e)
        {
            var chooser = (FileChooserButton)s;
            Settings.Current.FFmpegProgram = chooser.Filename;
        }

        void On_Showtooltips_Toggle(object s, EventArgs e)
        {
            var tgl = (CheckButton)s;
            Settings.Current.DisplayToolTips = tgl.Active;
        }

        void On_DeleteChunks_Toggle(object s, EventArgs e)
        {
            var tgl = (CheckButton)s;
            Settings.Current.DeleteChunksFolder = tgl.Active;
        }

        void On_LoggingLvl_Changed(object s, EventArgs e)
        {
            var cb = (ComboBox)s;
            Settings.Current.LoggingLevel = cb.Active;
        }

        void On_PrefOk_Clicked(object s, EventArgs e)
        {
            Hide();
        }

    }
}
