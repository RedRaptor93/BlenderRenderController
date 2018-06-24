using BRClib;
using BRClib.Render;
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
        BrcViewModel _vm;
        RenderManager _renderMngr;
        ETACalculator _etaCalc;
        CancellationTokenSource _afterRenderCancelSrc;


        public BrcMain() : this(Glade.LoadUI("BrcGtk.glade", "brc_style.css"), "BrcMain")
        {
            Initialize();

            _ProjBase = lblProjectName.Text;

            _vm = new BrcViewModel(ShowVMDialog, Status);
            _vm.PropertyChanged += ViewModel_PropertyChanged;
            CheckConfigs();

            _renderMngr = new RenderManager();

            // 'Invoke' makes sure the event handlers will run on the UI thread
            _renderMngr.Finished += (s,e) => Invoke(RenderMngr_Finished, s, e);
            _renderMngr.AfterRenderStarted += (s, e) => Invoke(RenderMngr_AfterRenderStarted, s, e);
            _renderMngr.ProgressChanged += (s, e) => Invoke(RenderMngr_ProgressChanged, s, e);

            _etaCalc = new ETACalculator(10, 5);

            ShowAll();
        }

        public VMDialogResult ShowVMDialog(string title, string message, string details, VMDialogButtons buttons)
        {
            ButtonsType btns = ButtonsType.Ok;
            var type = title.ToLower().StartsWith("w") ? MessageType.Warning : MessageType.Error;
            bool retryCancel = false;
            switch (buttons)
            {
                case VMDialogButtons.OK:
                    btns = ButtonsType.Ok;
                    break;
                case VMDialogButtons.OKCancel:
                    btns = ButtonsType.OkCancel;
                    break;
                case VMDialogButtons.YesNo:
                    btns = ButtonsType.YesNo;
                    break;
                case VMDialogButtons.RetryCancel:
                    retryCancel = true;
                    break;
            }

            Dialog dlg = null;

            if (details != null)
            {
                if (retryCancel)
                {
                    dlg = new DetailDialog(message, title, details, this, type);
                    // the lack of a ResponseType.Retry and Cancel makes me sad D:
                    dlg.AddButton("Retry", 1);
                    dlg.AddButton(Stock.Lookup(Stock.Cancel).Label, 2);
                }
                else
                {
                    dlg = new DetailDialog(message, title, details, this, type, btns);
                }
            }
            else
            {
                dlg = new MessageDialog(null, DialogFlags.Modal, type, btns, message);
            }

            var r = dlg.Run(); dlg.Destroy();
            return Helpers.VMDR(r);
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

        void UpdateInfoBoxItems(Project project)
        {
            if (project == null)
            {
                activeSceneInfoValue.Text = 
                durationInfoValue.Text = 
                fpsInfoValue.Text =
                resolutionInfoValue.Text = "...";
                lblProjectName.Text = _ProjBase;
            }
            else
            {
                activeSceneInfoValue.Text = project.ActiveScene;
                durationInfoValue.Text = project.Duration.HasValue 
                    ? string.Format("{0:%h}h {0:%m}m {0:%s}s {0:%f}ms", project.Duration.Value)
                    : null;
                fpsInfoValue.Text = project.Fps.ToString("F2");
                resolutionInfoValue.Text = project.Resolution;

                lblProjectName.Text = _ProjBase + " - " + project.ProjectName;
            }
        }

        void UpdateOptions(Project project)
        {
            if (project == null) return;

            numStartFrameAdjust.Value = project.Start;
            numEndFrameAdjust.Value = project.End;
            numChunkSizeAdjust.Value = project.ChunkLenght;
            numProcMaxAdjust.Value = project.MaxConcurrency;

            entryOutputPath.Text = project.OutputPath;
        }

        async void OpenBlendFile(string blendFile)
        {
            _vm.IsBusy = true;

            var(loaded, result) = await _vm.OpenBlendFile(blendFile);
            if (!loaded)
            {
                if (result == VMDialogResult.Retry)
                {
                    OpenBlendFile(blendFile);
                    return;
                }
                
            }

            _vm.IsBusy = false;
        }

        void Status(string text, Label item)
        {
            if (item == null) item = lblStatus;

            //Application.Invoke(delegate
            //{
                item.Text = text;
            //});

        }

        void Status(string text) => Status(text, null);

        void ResetCTS()
        {
            if (_afterRenderCancelSrc != null)
            {
                _afterRenderCancelSrc.Dispose();
                _afterRenderCancelSrc = null;
            }
            _afterRenderCancelSrc = new CancellationTokenSource();
        }


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
                    StopWork(false);
                }
            }

            args.RetVal = true;
            SaveSettings();

            if (aboutWin != null) aboutWin.Destroy();
            if (prefWin != null) prefWin.Destroy();

            Application.Quit();
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
            var bpath = _vm.Project.BlendFilePath;
            _vm.Project = null;
            OpenBlendFile(bpath);
        }

        private void On_UnloadFile(object sender, EventArgs e)
        {
            _vm.Project = null;
        }


        void On_miGithub_Clicked(object s, EventArgs e)
        {
            ShellOpen("https://github.com/RedRaptor93/BlenderRenderController");
        }

        void On_miDonate_Clicked(object s, EventArgs e)
        {
            _vm.OpenDonationPage();
        }

        void On_miReportABug_Clicked(object s, EventArgs e)
        {
            ShellOpen("https://github.com/RedRaptor93/BlenderRenderController/wiki/Reporting-an-issue");
        }

        private void On_Quit(object s, EventArgs e)
        {
            Application.Quit();
        }

        void On_cbJoiningAction_Changed(object s, EventArgs e)
        {
            var cb = (ComboBox)s;
            var active = cb.Active;

            Settings.AfterRender = (AfterRenderAction)active;
        }

        void On_cbRenderer_Changed(object s, EventArgs e)
        {
            var cb = (ComboBox)s;
            var active = cb.Active;
            Settings.Renderer = (Renderer)active;
        }

        void On_AutoStartStop_Toggled(object s, EventArgs e)
        {
            var radioBtn = (RadioButton)s;
            AutoFrameRange = radioBtn.Active;

            frameRangeBox.Sensitive = !AutoFrameRange;

            if (AutoFrameRange)
            {
                _vm.Project.ResetRange();
            }
        }

        void On_AutoChunkSize_Toggled(object s, EventArgs e)
        {
            var radioBtn = (RadioButton)s;
            AutoChunkDiv = radioBtn.Active;

            chunkDivBox.Sensitive = !AutoChunkDiv;

            if (!AutoChunkDiv)
                return;

            var currentStart = numStartFrameAdjust.Value;
            var currentEnd = numEndFrameAdjust.Value;
            var maxParallel = Environment.ProcessorCount;
            var expectedCLen = Math.Ceiling((currentEnd - currentStart + 1) / maxParallel);

            _vm.Project.ChunkLenght = (int)expectedCLen;
        }

        void On_numFrameRange_ValueChanged(object s, EventArgs e)
        {
            var spinBtn = (SpinButton)s;

            var startFrame = numStartFrameAdjust.Value;
            var endFrame = numEndFrameAdjust.Value;

            if (AutoChunkDiv)
            {
                _vm.Project.ChunkLenght = (int)Math.Ceiling((endFrame - startFrame + 1) / _vm.Project.MaxConcurrency);
            }

            _vm.Project.Start = (int)startFrame;
            _vm.Project.End = (int)endFrame;
        }

        void On_numChunkSize_ValueChanged(object s, EventArgs e)
        {
            var adj = (Adjustment)s;
            _vm.Project.ChunkLenght = (int)adj.Value;
        }

        void On_numProcessMax_ValueChanged(object s, EventArgs e)
        {
            var adj = (Adjustment)s;
            _vm.Project.MaxConcurrency = (int)adj.Value;
        }

        void StartRender_Clicked(object s, EventArgs e)
        {
            // start render... 

            var outdir = _vm.Project.OutputPath;

            bool destNotEmpty = (Directory.Exists(outdir) && Directory.GetFiles(outdir).Length > 0)
                || Directory.Exists(_vm.Project.ChunksDirPath);

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

            StartRender();

            btnStopRender.Show();
            startStopStack.VisibleChild = btnStopRender;
            btnStopRender.GrabFocus();
        }

        void StopRender_Clicked(object s, EventArgs e)
        {
            // stop / cancel render...

            if (_vm.IsBusy)
            {
                var dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Warning, 
                                            ButtonsType.YesNo,
                                            "Are you sure you want to stop?");

                var result = (ResponseType)dlg.Run(); dlg.Destroy();

                if (result == ResponseType.No)
                    return;

                StopWork(false);
            }

            btnStartRender.Show();
            startStopStack.VisibleChild = btnStartRender;
            btnStartRender.GrabFocus();
        }

        void StartRender()
        {
            IEnumerable<Chunk> chunks;
            if (AutoChunkDiv)
            {
                chunks = Chunk.CalcChunks(_vm.Project.Start, _vm.Project.End,
                    _vm.Project.MaxConcurrency);
            }
            else
            {
                chunks = Chunk.CalcChunksByLength(_vm.Project.Start, _vm.Project.End,
                    _vm.Project.ChunkLenght);
            }

            _vm.UpdateCurrentChunks(chunks);

            _vm.IsBusy = true;

            _renderMngr.Setup(_vm.Project, Settings.AfterRender, Settings.Renderer);

            Status("Starting render...");

            _renderMngr.StartAsync();
        }

        void StopWork(bool completed)
        {
            if (!completed)
            {
                if (_renderMngr.InProgress)
                {
                    _renderMngr.Abort();
                }

                if (_afterRenderCancelSrc != null)
                    _afterRenderCancelSrc.Cancel();
            }

            _etaCalc.Reset();
            _vm.IsBusy = false;

            workProgress.Fraction = 0;

            Status("ETR: " + TimeSpan.Zero.ToString(@"hh\:mm\:ss"), lblETR);
        }

        private void RenderMngr_ProgressChanged(object sender, RenderProgressInfo e)
        {
            // BUG: Progress reporting does not update main UI

            lblStatus.Text = $"Completed {e.PartsCompleted} / {_vm.Project.ChunkList.Count} chunks, " +
                $"{e.FramesRendered} frames rendered";

            float porcentageDone = e.PartsCompleted / (float)_vm.Project.ChunkList.Count;

            workProgress.Fraction = porcentageDone;

            _etaCalc.Update(porcentageDone);

            if (_etaCalc.ETAIsAvailable)
            {
                lblETR.Text = "ETR: " + _etaCalc.ETR.ToString(@"hh\:mm\:ss");
            }

            // TODO? TimeElapsed

        }

        private void RenderMngr_AfterRenderStarted(object sender, AfterRenderAction e)
        {
            workProgress.Fraction = 0;

            switch (e)
            {
                case AfterRenderAction.MIX_JOIN:
                    Status("Joining chunks w/ custom mixdown");
                    break;
                case AfterRenderAction.JOIN:
                    Status("Joining chunks");
                    break;
                case AfterRenderAction.MIXDOWN:
                    Status("Rendering mixdown");
                    break;
            }
        }

        private void RenderMngr_Finished(object sender, BrcRenderResult e)
        {
            StopWork(true);

            Dialog dlg = null;

            if (e == BrcRenderResult.AllOk)
            {
                if (_renderMngr.Action == AfterRenderAction.NOTHING
                    && Settings.DeleteChunksFolder)
                {
                    try
                    {
                        Directory.Delete(_vm.Project.ChunksDirPath, true);
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

                dlg.Run();

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
            _vm.IsBusy = true;
            ResetCTS();

            Status("Rendering mixdown...");

            var mixcmd = new MixdownCmd()
            {
                BlendFile = _vm.Project.BlendFilePath,
                Range = _vm.Project.ChunkList.GetFullRange(),
                OutputFolder = _vm.Project.OutputPath
            };

            var result = await mixcmd.RunAsync(_afterRenderCancelSrc.Token);

            if (result == 0)
            {
                Status("Mixdown complete");
            }
            else
            {
                var report = mixcmd.GenerateReport();
                var dlg = new DetailDialog("Mixdown failed", "Error", report, this, MessageType.Error);

                Status("Something went wrong...");

                dlg.Run(); dlg.Destroy();
            }

            _vm.IsBusy = false;
        }

        void On_JoinChunks_Clicked(object s, EventArgs e)
        {
            return;

            _vm.IsBusy = true;
            ResetCTS();

            // TODO: Manual concat dialog

            _vm.IsBusy = false;

        }

        void BtnChangeFolder_Clicked(object s, EventArgs e)
        {
            var result = (ResponseType)chooseOutputFolderDialog.Run();
            if (result == ResponseType.Accept)
            {
                _vm.Project.OutputPath =
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

        void VMProject_PropChanged(object s, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var proj = (Project)s;
            UpdateOptions(proj);
            UpdateInfoBoxItems(proj);
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var vm = (BrcViewModel)sender;

            workSpinner.Active = vm.IsBusy;

            startStopStack.Sensitive = vm.CanRender;
            miUnload.Sensitive = vm.CanEditCurrentProject;

            miRenderMixdown.Sensitive = vm.CanRender && !vm.IsBusy;
            //miJoinChunks.Sensitive = !vm.IsBusy;
            miJoinChunks.Sensitive = false; // TODO: manual concat UI

            miPref.Sensitive = !vm.IsBusy;

            miReloadFile.Sensitive = tsReloadFile.Sensitive = vm.CanReloadCurrentProject;

            frRenderOptions.Sensitive =
            frOutputFolder.Sensitive = vm.CanEditCurrentProject;

            miOpenFile.Sensitive =
            tsOpenFile.Sensitive =
            miOpenRecent.Sensitive =
            tsOpenRecent.Sensitive = vm.CanLoadNewProject;

            lblETR.Visible =
            lblTimeElapsed.Visible = vm.ProjectLoaded;

            lblStatus.Text = vm.DefaultStatusMessage;

            if (e.PropertyName == nameof(vm.Project))
            {
                UpdateOptions(vm.Project);
                UpdateInfoBoxItems(vm.Project);

                if (vm.ProjectLoaded)
                    vm.Project.PropertyChanged += VMProject_PropChanged;
            }

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
