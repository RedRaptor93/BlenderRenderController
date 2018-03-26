using BRClib;
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
            string blend = "*.blend";

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

            InitDialogs();
            CheckConfigs();
        }


        void InitDialogs()
        {
            aboutWin = new AboutDialog(Builder.GetObject("AboutWin").Handle);
            aboutWin.Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            aboutWin.Close += delegate { aboutWin.Hide(); };

            prefWin = new Dialog(Builder.GetObject("PrefWin").Handle);

        }

        void CheckConfigs()
        {
            _vm.ConfigOk = Services.Settings.CheckCorrectConfig();

            if (!_vm.ConfigOk)
            {
                var msgBox = new MessageDialog(this, DialogFlags.Modal, MessageType.Warning, ButtonsType.OkCancel,
                    "One or more required programs were not found, click 'Ok' to go to preferences...");

                var resp = (ResponseType)msgBox.Run();

                if (resp == ResponseType.Ok)
                {
                    prefWin.Run();
                    prefWin.Hide();
                }

                msgBox.Destroy();
            }
        }
        
        
        // private properties
        bool AutoFrameRange { get; set; }

        bool AutoChunkDiv { get; set; }
    }
}
