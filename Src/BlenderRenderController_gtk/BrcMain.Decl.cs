﻿using BRClib;
using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace BlenderRenderController
{
    partial class BrcMain
    {
        #region Ui declarations
        #pragma warning disable 169, 649

        // main frames
        [UI] Frame frBlendFile, frRenderOptions, frOutputFolder;
        [UI] Box frameRangeBox, chunkDivBox;

        [UI] Label lblProjectName;

        [UI] Stack startStopStack;
        [UI] Button btnStartRender, btnStopRender;

        [UI] Adjustment numStartFrameAdjust, numEndFrameAdjust, 
            numChunkSizeAdjust,  numProcMaxAdjust;

        // status bar
        [UI] ProgressBar workProgress;
        [UI] Spinner workSpinner;
        [UI] Label lblStatus, lblTimeElapsed, lblETR;

        // info box values
        [UI] Label activeSceneInfoValue, durationInfoValue, fpsInfoValue, resolutionInfoValue;

        // menubar
        [UI] MenuBar menuBar;

        // -File
        [UI] ImageMenuItem miOpenFile, miReloadFile, miPref;
        [UI] MenuItem miUnload, miOpenRecent;

        // -Tools
        [UI] ImageMenuItem miRenderMixdown, miJoinChunks;

        // toolbar
        [UI] Toolbar toolbar;
        [UI] ToolButton tsOpenFile, tsReloadFile, tsAbout;
        [UI] MenuToolButton tsOpenRecent;

        [UI] ComboBox cbJoiningAction, cbRenderer;

        [UI] Entry entryOutputPath;

        #pragma warning restore 169, 649
        #endregion

        FileChooserDialog openBlendDialog, chooseOutputFolderDialog;
        AboutDialog aboutWin;
        Dialog prefWin;

        RecentItemsMenu recentBlendsMenu;

        string _ProjBase;

        const string RECENT_ITEM_NAME = "recent";

        void Initialize()
        {
            // setup filters
            const string blend = "*.blend";

            var blendFilter = new FileFilter();
            blendFilter.Name = ".blend files";
            blendFilter.AddPattern(blend);

            var recentBlendsFilter = new RecentFilter();
            recentBlendsFilter.Name = blendFilter.Name;
            recentBlendsFilter.AddPattern(blend);

            _ProjBase = lblProjectName.Text;

            openBlendDialog = new FileChooserDialog("Open blend file", this, FileChooserAction.Open,
                "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);

            openBlendDialog.Filter = blendFilter;

            chooseOutputFolderDialog = new FileChooserDialog("Choose output folder", this, FileChooserAction.SelectFolder,
                "Cancel", ResponseType.Cancel, "Select", ResponseType.Accept);

            recentBlendsMenu = new RecentItemsMenu(recentBlendsFilter);
            miOpenRecent.Submenu = recentBlendsMenu;
            tsOpenRecent.Menu = recentBlendsMenu;

            recentBlendsMenu.RecentItem_Clicked += On_OpenRecent;
            recentBlendsMenu.Clear_Clicked += On_ClearRecents_Click;

            recentBlendsMenu.ShowAll();

            numProcMaxAdjust.Value =
            numProcMaxAdjust.Upper = Environment.ProcessorCount;

            frameRangeBox.Sensitive = chunkDivBox.Sensitive = false;
            AutoFrameRange = AutoChunkDiv = true;

            this.DeleteEvent += BrcMain_DeleteEvent;
            this.numChunkSizeAdjust.Changed += NumChunkSizeAdjust_Changed;
            this.tsOpenRecent.Clicked += TsOpenRecent_Clicked;

            InitDialogs();

        }

        private void TsOpenRecent_Clicked(object sender, EventArgs e)
        {
            recentBlendsMenu.PopupAtWidget((MenuToolButton)sender, Gdk.Gravity.SouthWest, Gdk.Gravity.NorthWest, null);
        }

        private void NumChunkSizeAdjust_Changed(object sender, EventArgs e)
        {
            var adjust = (Adjustment)sender;
            _vm.Project.ChunkLenght = (int)adjust.Value;
        }

        void InitDialogs()
        {
            aboutWin = new AboutDialog(Builder.GetObject("AboutWin").Handle);
            aboutWin.Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            aboutWin.Close += delegate { aboutWin.Hide(); };

            prefWin = new PreferencesWin();
            prefWin.TransientFor = this;
            prefWin.Hidden += delegate
            {
                _vm.ConfigOk = BRClib.Global.CheckProgramPaths();
            };
        }

        void CheckConfigs()
        {
            _vm.ConfigOk = BRClib.Global.CheckProgramPaths();

            if (!_vm.ConfigOk)
            {
                var msgBox = new MessageDialog(this, DialogFlags.Modal, MessageType.Warning, ButtonsType.OkCancel,
                    "One or more required programs were not found, click 'Ok' to go to preferences...");

                var resp = (ResponseType)msgBox.Run(); msgBox.Destroy();

                if (resp == ResponseType.Ok)
                {
                    prefWin.Run();
                    prefWin.Hide();
                }
            }
        }
        
        void Invoke<T>(EventHandler<T> handler, object s, T args)
        {
            Application.Invoke(delegate {
                handler(s, args);
            });
        }

        void Invoke(EventHandler handler, object s, EventArgs args)
        {
            Application.Invoke(s, args, handler);
        }
        
        // private properties
        bool AutoFrameRange { get; set; }

        bool AutoChunkDiv { get; set; }
    }
}
