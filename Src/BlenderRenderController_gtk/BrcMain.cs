using BRClib;
using BRClib.ViewModels;
using BRClib.Commands;
using BRClib.Extentions;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gtk;
using NLog;
using System.Threading;
using System.Collections.Generic;
using PathIO = System.IO.Path;
using BRCRes = BRClib.Properties.Resources;


namespace BlenderRenderController
{
    using static BRClib.Global;

    public partial class BrcMain : Window
    {
        BrcMainViewModel _vm;


        public BrcMain() : this(Glade.LoadUI("BrcGtk.glade", "brc_style.css"), "BrcMain")
        {
            _vm = new BrcMainViewModel();

            Initialize();

            _vm.PropertyChanged += ViewModel_PropertyChanged;
            CheckConfigs();

            // 'Invoke' makes sure the event handlers will run on the UI thread
            _vm.OnRenderFinished = e => Invoke(OnRenderMngrFinished, e);
        }

        static bool ClearOutputFolder(string path)
        {
            bool result;
            string errMsg = null;

            try
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                DirectoryInfo[] subDirs = dir.GetDirectories();

                // clear files in the output
                foreach (FileInfo fi in dir.GetFiles())
                {
                    fi.Delete();
                }

                // clear files in the 'chunks' subdir
                var chunkSDir = subDirs.FirstOrDefault(di => di.Name == "chunks");
                if (chunkSDir != null)
                {
                    Directory.Delete(chunkSDir.FullName, true);
                }

                result = true;
            }
            catch (IOException)
            {
                errMsg = "Can't clear output folder, files are in use";
                result = false;
            }
            catch (Exception ex)
            {
                errMsg = $"An unexpected error ocurred, sorry.\n\n{ex.Message} ({ex.HResult:X})";
                result = false;
            }

            Trace.WriteLineIf(!result, errMsg);

            if (!result)
            {
                var msgDialog = new MessageDialog(null, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, 
                    errMsg);

                msgDialog.Run();
            }

            return result;
        }


        async void OpenBlendFile(string blendFile)
        {
            logger.Info("Loading .blend");

            var (retcode, cmd) = await _vm.OpenBlend(blendFile);
            // success = retcode >= 0;
            // showDlg = retcode != 0;

            Dialog dlg = null;

            switch (retcode)
            {
                case -1: // ERROR File not found
                    dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "File not found");
                    break;
                case -2: // ERROR no info receved
                    dlg = new DetailDialog(this, BRCRes.AppErr_NoInfoReceived, cmd.GenerateReport(), MessageType.Error);
                    dlg.AddButton("Retry", ResponseType.Yes);
                    dlg.AddButton("_Cancel", ResponseType.Cancel);
                    var response = (ResponseType)dlg.Run(); 
                    if (response == ResponseType.Yes)
                    {
                        OpenBlendFile(blendFile);
                    }
                    dlg.Destroy();
                    return;
                case 1: // WARN RenderFormat is image
                    var eMsg = string.Format(BRCRes.AppErr_RenderFormatIsImage, _vm.Data.FileFormat);
                    dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Warning, ButtonsType.Ok, eMsg);
                    break;
                case 2: // WARN Invalid output path
                        // use .blend folder path if outputPath is unset, display a warning about it
                    dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Warning, ButtonsType.Ok, BRCRes.AppErr_BlendOutputInvalid);
                    break;
            }

            if (dlg != null)
            {
                dlg.Run();
                dlg.Destroy();
            }
        }

        void Status(string text, Label item = null)
        {
            if (item == null) item = lblStatus;

            Application.Invoke(delegate {
                item.Text = text;
            });
        }

        void Status(string text) => Status(text, null);


        // Events handlers

        private void BrcMain_DeleteEvent(object o, DeleteEventArgs args)
        {
            if (_vm.IsBusy)
            {
                var dlg = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Question, ButtonsType.YesNo,
                    "Closing now will cancel the rendering process. Close anyway?");

                var confirmation = (ResponseType)dlg.Run();

                if (confirmation == ResponseType.No)
                {
                    args.RetVal = false;
                    return;
                }
                else
                {
                    _vm.StopRender();
                    _vm.CancelExtraTasks();
                }
            }

            args.RetVal = true;
            SaveSettings();

            if (aboutWin != null) aboutWin.Destroy();
            if (prefWin != null) prefWin.Destroy();

            Application.Quit();
            logger.Info("Program closing");
        }

        private void On_OpenFile(object sender, EventArgs e)
        {
            var result = (ResponseType)openBlendDialog.Run();
            openBlendDialog.Hide();

            if (result == ResponseType.Accept)
            {
                var blendFile = openBlendDialog.Filename;
                OpenBlendFile(blendFile);
            }

        }

        private void On_OpenRecent(object o, RecentInfo info)
        {
            var blendFile = info.UriDisplay;
            OpenBlendFile(blendFile);
        }

        void On_ClearRecents_Click(object o, CancelArgs e)
        {
            var dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo,
                "This will clear all files in the recent blends list, are you sure?");

            var result = (ResponseType)dialog.Run(); dialog.Destroy();

            e.RetVal = result == ResponseType.No;
        }

        private void On_ReloadFile(object sender, EventArgs e)
        {
            var bpath = _vm.BlendFile;
            _vm.UnloadProject();
            OpenBlendFile(bpath);
        }

        private void On_UnloadFile(object sender, EventArgs e)
        {
            _vm.UnloadProject();
        }


        void On_miGithub_Clicked(object s, EventArgs e)
        {
            _vm.OpenProjectPage();
        }

        void On_miDonate_Clicked(object s, EventArgs e)
        {
            _vm.OpenDonationPage();
        }

        void On_miReportABug_Clicked(object s, EventArgs e)
        {
            _vm.OpenReportIssuePage();
        }

        private void On_Quit(object s, EventArgs e)
        {
            Application.Quit();
        }

        void On_cbJoiningAction_Changed(object s, EventArgs e)
        {
            var cb = (ComboBox)s;
            Settings.AfterRender = (AfterRenderAction)cb.Active;
        }

        void On_cbRenderer_Changed(object s, EventArgs e)
        {
            var cb = (ComboBox)s;
            Settings.Renderer = (Renderer)cb.Active;
        }



        void On_numFrameRange_ValueChanged(object s, EventArgs e)
        {
            _vm.StartFrame = (int)numStartFrameAdjust.Value;
            _vm.EndFrame = (int)numEndFrameAdjust.Value;
        }

        void On_numChunkSize_ValueChanged(object s, EventArgs e)
        {
            var adj = (Adjustment)s;
            _vm.ChunkSize = (int)adj.Value;
        }

        void On_numProcessMax_ValueChanged(object s, EventArgs e)
        {
            var adj = (Adjustment)s;
            _vm.MaxCores = (int)adj.Value;
        }

        void StartRender_Clicked(object s, EventArgs e)
        {
            // start render... 
            Debug.Assert(_vm.IsNotBusy, "Bad state!", "Start render called while busy");    

            var outdir = _vm.OutputPath;

            bool destNotEmpty = (Directory.Exists(outdir) && Directory.GetFiles(outdir).Length > 0)
                || Directory.Exists(_vm.DefaultChunksDirPath);

            if (destNotEmpty)
            {
                var dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Question,
                       ButtonsType.YesNo,
                       "All existing files and folders in the output folder will be deleted!\n" +
                       "Do you want to continue?");

                var result = (ResponseType)dlg.Run(); dlg.Destroy();

                if (result == ResponseType.No)
                    return;

                if (!ClearOutputFolder(outdir))
                    return;
            }

            _vm.StartRender();

            btnStopRender.Show();
            startStopStack.VisibleChild = btnStopRender;
            btnStopRender.GrabFocus();
        }

        void StopRender_Clicked(object s, EventArgs e)
        {
            // stop / cancel render...
            Debug.Assert(_vm.IsBusy, "Bad state!");

            var dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Warning, 
                ButtonsType.YesNo, "Are you sure you want to stop?");

            var result = (ResponseType)dlg.Run(); dlg.Destroy();
            if (result == ResponseType.No) return;

            _vm.StopRender();

            btnStartRender.Show();
            startStopStack.VisibleChild = btnStartRender;
            btnStartRender.GrabFocus();
        }

        private void UpdateProgress(float p = 0f)
        {
            if (p < 0f)
            {
                StartMarquee();
            }
            else
            {
                StopMarquee();
                workProgress.Fraction = p;
            }
        }

        private void OnRenderMngrFinished(BrcRenderResult e)
        {
            //StopWork(true);

            Dialog dlg = null;

            if (e == BrcRenderResult.AllOk)
            {
                if (Settings.AfterRender == AfterRenderAction.NOTHING
                    && Settings.DeleteChunksFolder)
                {
                    try
                    {
                        Directory.Delete(_vm.DefaultChunksDirPath, true);
                    }
                    catch (Exception ex)
                    {
                         dlg = new MessageDialog(this, DialogFlags.Modal, 
                            MessageType.Error, ButtonsType.Close,
                            $"Failed to clear 'chunks' folder:\n\n{ex.Message} ({ex.HResult:X})");

                        dlg.Run(); dlg.Destroy();
                    }
                }

                dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo,
                        "Open destination folder?");

                var result = (ResponseType)dlg.Run();
                dlg.Destroy(); dlg = null;

                if (result == ResponseType.Yes)
                    _vm.OpenOutputFolder();

            }
            else if (e == BrcRenderResult.Aborted)
            {
                Status("Operation aborted");
            }
            else if (e == BrcRenderResult.AfterRenderFailed)
            {
                dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok,
                    BRCRes.RM_AfterRenderFailed);
                Status("Errors detected");
            }
            else
            {
                dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.Close,
                        BRCRes.RM_unexpected_error);
                Status("Unexpected error");
            }

            if (dlg != null)
            {
                dlg.Run();
                dlg.Destroy();
            }

        }

        async void On_RenderMixdown_Clicked(object s, EventArgs e)
        {
            var mix = await _vm.RunMixdown();
            if (mix.ExitCode != 0)
            {
                var report = mix.GenerateReport();
                var dlg = new DetailDialog(this, "Mixdown failed", report, MessageType.Error);
                dlg.AddButton("_OK", ResponseType.Ok);
                dlg.Run(); dlg.Destroy();
            }
        }

        async void On_JoinChunks_Clicked(object s, EventArgs e)
        {
            var dlg = new ConcatDialog();
            dlg.TransientFor = this;
            var response = (ResponseType)dlg.Run(); dlg.Destroy();

            if (response != ResponseType.Ok) return;

            var cmd = dlg.Concat;
            var cct = await _vm.RunConcat(cmd.ConcatTextFile, cmd.OutputFile, cmd.MixdownFile);

            if (cmd.ExitCode != 0)
            {
                var report = cmd.GenerateReport();
                var ddlg = new DetailDialog(this, "Concatenation failed", report, MessageType.Error);
                ddlg.AddButton("_OK", ResponseType.Ok);
                ddlg.Run(); ddlg.Destroy();
            }
        }

        void BtnChangeFolder_Clicked(object s, EventArgs e)
        {
            var result = (ResponseType)chooseOutputFolderDialog.Run();
            if (result == ResponseType.Accept)
            {
                _vm.OutputPath =
                entryOutputPath.Text = chooseOutputFolderDialog.Filename;
            }

            chooseOutputFolderDialog.Hide();
        }

        void BtnOpenFolder_Clicked(object s, EventArgs e)
        {
            _vm.OpenOutputFolder();
        }

        private void RecentMngr_Changed(object sender, EventArgs e)
        {
            var items = RecentManager.Default.RecentItems;
            var orderedItems = items.OrderBy(ri => ri.Added).Select(ri => ri.Uri);

            Settings.RecentProjects = new List<string>(orderedItems);

            Trace.WriteLine("Recent items" + string.Join(", ", orderedItems));
        }


        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var vm = (BrcMainViewModel)sender;

            switch (e.PropertyName)
            {
                case nameof(vm.IsBusy):
                case nameof(vm.IsNotBusy):
                case nameof(vm.ConfigOk):
                    miJoinChunks.Sensitive = vm.ConfigOk;
                    miOpenFile.Sensitive =
                    miOpenRecent.Sensitive =
                    tsOpenFile.Sensitive =
                    tsOpenRecent.Sensitive = vm.ConfigOk && vm.IsNotBusy;
                    cbRenderer.Sensitive =
                    cbJoiningAction.Sensitive = vm.IsNotBusy;
                    break;
                case nameof(vm.ProjectLoaded):
                    btnStartRender.Sensitive = vm.ProjectLoaded;
                    lblProjectName.Visible = vm.ProjectLoaded;
                    break;
                case nameof(vm.Progress):
                    UpdateProgress(vm.Progress);
                    Status(vm.StatusTime, lblETR);
                    break;
                case nameof(vm.Footer):
                    Status(vm.Footer);
                    break;
                case nameof(vm.Header):
                    lblProjectName.Text = vm.Data.ProjectName;
                    break;
                case nameof(vm.Title):
                    Title = vm.Title;
                    break;
                case nameof(vm.AutoFrameRange):
                    fiStartFrame.Sensitive = !vm.AutoFrameRange;
                    fiEndFrame.Sensitive = !vm.AutoFrameRange;
                    swAutoFrameRange.Active = vm.AutoFrameRange;
                    break;
                case nameof(vm.AutoChunkSize):
                    fiChunkSize.Sensitive = !vm.AutoChunkSize;
                    swAutoChunkSize.Active = vm.AutoChunkSize;
                    break;
                case nameof(vm.AutoMaxCores):
                    fiMaxCores.Sensitive = !vm.AutoMaxCores;
                    swAutoMaxCores.Active = vm.AutoMaxCores;
                    break;
                case nameof(vm.StartFrame):
                    numStartFrameAdjust.Value = vm.StartFrame;
                    break;
                case nameof(vm.EndFrame):
                    numEndFrameAdjust.Value = vm.EndFrame;
                    break;
                case nameof(vm.OutputPath):
                    entryOutputPath.Text = vm.OutputPath;
                    break;
                case nameof(vm.MaxCores):
                    numMaxCoresAdjust.Value = vm.MaxCores;
                    break;
                case nameof(vm.ChunkSize):
                    numChunkSizeAdjust.Value = vm.ChunkSize;
                    break;

                // Infobox items
                case nameof(vm.ActiveScene):
                    activeSceneInfoValue.Text = !string.IsNullOrEmpty(vm.ActiveScene) ? vm.ActiveScene : "...";
                    break;
                case nameof(vm.Duration):
                    durationInfoValue.Text = vm.Duration != TimeSpan.Zero 
                                            ? string.Format("{0:%h}h {0:%m}m {0:%s}s {0:%f}ms", vm.Duration) 
                                            : "...";
                    break;
                case nameof(vm.Fps):
                    fpsInfoValue.Text = vm.Fps > 0 ? vm.Fps.ToString("F2") : "...";
                    break;
                case nameof(vm.Resolution):
                    resolutionInfoValue.Text = !string.IsNullOrEmpty(vm.Resolution) ? vm.Resolution : "...";
                    break;
            }



            miUnload.Sensitive =
            tsReloadFile.Sensitive =
            miReloadFile.Sensitive =
            miRenderMixdown.Sensitive =
            swAutoFrameRange.Sensitive =
            swAutoChunkSize.Sensitive =
            swAutoMaxCores.Sensitive =
            frOutputFolder.Sensitive = vm.ProjectLoaded && vm.IsNotBusy;
        }

        void On_ShowAbout(object s, EventArgs e)
        {
            aboutWin.Run();
            aboutWin.Hide();
        }

        void On_Preferences(object sender, EventArgs e)
        {
            prefWin.Run();
            prefWin.Hide();
        }

    }
}
