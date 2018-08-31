using BRCRes = BRClib.Properties.Resources;
using UI = Gtk.Builder.ObjectAttribute;
using System;
using Gtk;
using NLog;
using Action = System.Action;

namespace BlenderRenderController
{
    partial class BrcMain
    {
        #region Ui declarations
        #pragma warning disable 169, 649

        // main frames
        [UI] Frame frBlendFile, frRenderOptions, frOutputFolder;
        //[UI] Box boxFrameRange, boxChunkCores;

        // optionFields
        [UI] Box fiStartFrame, fiEndFrame, fiChunkSize, fiMaxCores;
        [UI] Switch swAutoFrameRange, swAutoChunkSize, swAutoMaxCores;

        [UI] Label lblProjectName;

        // Start / stop btns
        [UI] Stack startStopStack;
        [UI] Button btnStartRender, btnStopRender;

        [UI] Adjustment numStartFrameAdjust, numEndFrameAdjust, 
            numChunkSizeAdjust,  numMaxCoresAdjust;

        // status bar
        [UI] ProgressBar workProgress;
        [UI] Label lblStatus, lblETR;

        // info box values
        [UI] Label activeSceneInfoValue, durationInfoValue, fpsInfoValue, resolutionInfoValue;

        // menubar
        //[UI] MenuBar menuBar;

        // -File
        [UI] ImageMenuItem miOpenFile, miReloadFile, miPref;
        [UI] MenuItem miUnload, miOpenRecent;

        // -Tools
        [UI] ImageMenuItem miRenderMixdown, miJoinChunks;

        // toolbar
        //[UI] Toolbar toolbar;
        [UI] ToolButton tsOpenFile, tsReloadFile, tsAbout;
        [UI] ToolButton tsOpenRecent;

        [UI] ComboBox cbJoiningAction, cbRenderer;

        [UI] Entry entryOutputPath;

        #pragma warning restore 169, 649
        #endregion

        FileChooserDialog openBlendDialog, chooseOutputFolderDialog;
        AboutDialog aboutWin;
        Dialog prefWin;
        RecentItemsMenu recentBlendsMenu;
        readonly Builder m_builder;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private BrcMain(Tuple<Builder, CssProvider> elements, string root)
            : base(elements.Item1.GetObject(root).Handle)
        {
            m_builder = elements.Item1;
            m_builder.Autoconnect(this);
            StyleContext.AddProviderForScreen(Screen, elements.Item2, 800);
        }

        void Initialize()
        {
            logger.Info("Initializing main window...");
            // setup filters
            const string blend = "*.blend";

            var blendFilter = new FileFilter();
            blendFilter.Name = ".blend files";
            blendFilter.AddPattern(blend);

            var recentBlendsFilter = new RecentFilter();
            recentBlendsFilter.Name = blendFilter.Name;
            recentBlendsFilter.AddPattern(blend);


            openBlendDialog = new FileChooserDialog("Open blend file", this, FileChooserAction.Open,
                "_Cancel", ResponseType.Cancel, "_Open", ResponseType.Accept);

            openBlendDialog.Filter = blendFilter;

            chooseOutputFolderDialog = new FileChooserDialog("Choose output folder", this, FileChooserAction.SelectFolder,
                "_Cancel", ResponseType.Cancel, "Select", ResponseType.Accept);

            recentBlendsMenu = new RecentItemsMenu(recentBlendsFilter);
            miOpenRecent.Submenu = recentBlendsMenu;

            recentBlendsMenu.RecentItem_Clicked += On_OpenRecent;
            recentBlendsMenu.Clear_Clicked += On_ClearRecents_Click;

            numMaxCoresAdjust.Value =
            numMaxCoresAdjust.Upper = Environment.ProcessorCount;

            // events
            DeleteEvent += BrcMain_DeleteEvent;
            tsOpenRecent.Clicked += TsOpenRecent_Clicked;
            numMaxCoresAdjust.ValueChanged += On_numProcessMax_ValueChanged;
            numChunkSizeAdjust.ValueChanged += On_numChunkSize_ValueChanged;
            swAutoMaxCores.AddNotification("active", On_AutoMaxCores_Toggled);
            swAutoFrameRange.AddNotification("active", On_AutoFramerange_Toggled);
            swAutoChunkSize.AddNotification("active", On_AutoChunkSize_Toggled);

            // Init dialogs
            aboutWin = new AboutDialog(m_builder.GetObject("AboutWin").Handle);
            aboutWin.Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            aboutWin.Close += delegate { aboutWin.Hide(); };

            prefWin = new PreferencesWin();
            prefWin.TransientFor = this;
            prefWin.Hidden += delegate
            {
                _vm.ConfigOk = BRClib.Global.CheckProgramPaths();
            };

            // Set starting state
            miRenderMixdown.Sensitive =
            swAutoFrameRange.Sensitive =
            swAutoChunkSize.Sensitive =
            swAutoMaxCores.Sensitive =
            frOutputFolder.Sensitive = _vm.ProjectLoaded && _vm.IsNotBusy;

            miOpenFile.Sensitive =
            miOpenRecent.Sensitive =
            tsOpenRecent.Sensitive =
            tsOpenFile.Sensitive = _vm.ConfigOk && _vm.IsNotBusy;

            miReloadFile.Sensitive =
            tsReloadFile.Sensitive =
            miUnload.Sensitive = _vm.ProjectLoaded && _vm.CanLoadMore;

            Status("...");

            lblProjectName.Visible = false;

            fiStartFrame.Sensitive = !_vm.AutoFrameRange;
            fiEndFrame.Sensitive = !_vm.AutoFrameRange;
            fiChunkSize.Sensitive = !_vm.AutoChunkSize;
            fiMaxCores.Sensitive = !_vm.AutoMaxCores;

            swAutoChunkSize.Active = _vm.AutoChunkSize;
            swAutoFrameRange.Active = _vm.AutoFrameRange;
            swAutoMaxCores.Active = _vm.AutoMaxCores;

            btnStartRender.Sensitive = _vm.ProjectLoaded;

            // ShowAlls
            recentBlendsMenu.ShowAll();

        }

        void On_AutoFramerange_Toggled(object s, GLib.NotifyArgs e)
        {
            _vm.AutoFrameRange = swAutoFrameRange.Active;
        }

        void On_AutoChunkSize_Toggled(object s, GLib.NotifyArgs e)
        {
            _vm.AutoChunkSize = swAutoChunkSize.Active;
        }

        void On_AutoMaxCores_Toggled(object s, GLib.NotifyArgs e)
        {
            _vm.AutoMaxCores = swAutoMaxCores.Active;
        }

        private void TsOpenRecent_Clicked(object sender, EventArgs e)
        {
            recentBlendsMenu.PopupAtWidget((Widget)sender, Gdk.Gravity.SouthWest, Gdk.Gravity.NorthWest, null);
        }

        void CheckConfigs()
        {
            _vm.ConfigOk = BRClib.Global.CheckProgramPaths();

            if (!_vm.ConfigOk)
            {
                var msgBox = new MessageDialog(this, DialogFlags.Modal, MessageType.Warning, ButtonsType.OkCancel,
                    BRCRes.AppErr_RequiredProgramsNotFound);

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

        void Invoke<T>(Action<T> action, T args)
        {
            Application.Invoke(delegate { action(args); });
        }


        uint timeoutID;

        void StartMarquee()
        {
            timeoutID = GLib.Timeout.Add(200, Marquee_Tick);
        }

        private bool Marquee_Tick()
        {
            workProgress.Pulse();
            return true;
        }

        void StopMarquee()
        {
            if (timeoutID > 0)
            {
                GLib.Timeout.Remove(timeoutID);
                timeoutID = 0;
            }
        }
    }
}
