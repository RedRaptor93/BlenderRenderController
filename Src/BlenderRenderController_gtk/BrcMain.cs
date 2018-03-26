using BlenderRenderController.Services;
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

using PathIO = System.IO.Path;
using System.Threading;
using System.Collections.Generic;

namespace BlenderRenderController
{
    public partial class BrcMain : WindowBase
    {
        BrcSettings _settings;
        BrcViewModel _vm;
        RenderManager _renderMngr;
        ETACalculator _etaCalc;
        CancellationTokenSource _afterRenderCancelSrc;

        int _autoStartF, _autoEndF;

        public BrcMain() : base("BrcGtk.glade", "brc_style.css", "BrcMain")
        {
            Initialize();

            _settings = Services.Settings.Current;
            _vm = new BrcViewModel();
            _vm.PropertyChanged += ViewModel_PropertyChanged;
            //_vm.ConfigOk = true; // TODO: setup settings and scripts infra.

            _renderMngr = new RenderManager(_settings);
            _renderMngr.Finished += RenderMngr_Finished;
            _renderMngr.AfterRenderStarted += RenderMngr_AfterRenderStarted;
            _renderMngr.ProgressChanged += RenderMngr_ProgressChanged;

            _etaCalc = new ETACalculator(10, 5);
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
                //logger.Error(msg);
                //MessageBox.Show(msg);
                result = false;
            }
            catch (Exception ex)
            {
                //logger.Error(ex.Message);
                //logger.Trace(ex.StackTrace);
                //MessageBox.Show("An unexpected error ocurred, sorry.\n\n" + ex.Message);
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
            numChunkSizeAdjust.Value = project.ChunkList[0].Length;
            numProcMaxAdjust.Value = project.MaxConcurrency;

            entryOutputPath.Text = project.OutputPath;
        }
        
        async void OpenBlendFile(string blendFile)
        {
            Status("Loading " + PathIO.GetFileName(blendFile) + " ...");
            workSpinner.Active = true;

            await _vm.GetBlendInfo(blendFile);

            if (_vm.ProjectLoaded)
            {
                _autoStartF = _vm.Project.Start;
                _autoEndF = _vm.Project.End;
            }

            workSpinner.Active = false;
        }

        void Status(string text, Label item = null)
        {
            if (item == null) item = lblStatus;

            Application.Invoke(delegate
            {
                item.Text = text;
            });
        }

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
                    args.RetVal = true;
                    StopWork(false);
                }
            }

            Services.Settings.Save();
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
            Process.Start("https://github.com/RedRaptor93/BlenderRenderController");
        }

        void On_miDonate_Clicked(object s, EventArgs e)
        {
            _vm.OpenDonationPage();
        }

        void On_miReportABug_Clicked(object s, EventArgs e)
        {
            Process.Start("https://github.com/RedRaptor93/BlenderRenderController/wiki/Reporting-an-issue");
        }

        private void On_Quit(object s, EventArgs e)
        {
            Application.Quit();
        }

        void On_cbJoiningAction_Changed(object s, EventArgs e)
        {
            var cb = (ComboBox)s;
            var active = cb.Active;

            _settings.AfterRender = (AfterRenderAction)active;
        }

        void On_cbRenderer_Changed(object s, EventArgs e)
        {
            var cb = (ComboBox)s;
            var active = cb.Active;
            _settings.Renderer = (Renderer)active;
        }

        void On_AutoStartStop_Toggled(object s, EventArgs e)
        {
            var radioBtn = (RadioButton)s;
            AutoFrameRange = radioBtn.Active;

            frameRangeBox.Sensitive = !AutoFrameRange;

            if (AutoFrameRange)
            {
                _vm.Project.Start = _autoStartF;
                _vm.Project.End = _autoEndF;
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

            _renderMngr.Setup(_vm.Project, _settings.AfterRender, _settings.Renderer);

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
            Status($"Completed {e.PartsCompleted} / {_vm.Project.ChunkList.Count} chunks, " +
                $"{e.FramesRendered} frames rendered");

            float porcentageDone = e.PartsCompleted / (float)_vm.Project.ChunkList.Count;

            workProgress.Fraction = porcentageDone;

            _etaCalc.Update(porcentageDone);

            if (_etaCalc.ETAIsAvailable)
            {
                Status("ETR: " + _etaCalc.ETR.ToString(@"hh\:mm\:ss"), lblETR);
            }

            // TODO? TimeElapsed

        }

        private void RenderMngr_AfterRenderStarted(object sender, AfterRenderAction e)
        {
            workProgress.Fraction = 0;
            workSpinner.Active = true;

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

            if (e == BrcRenderResult.AllOk)
            {
                MessageDialog dlg;

                if (_renderMngr.Action == AfterRenderAction.NOTHING
                    && _settings.DeleteChunksFolder)
                {
                    try
                    {
                        Directory.Delete(_vm.Project.ChunksDirPath, true);
                    }
                    catch (Exception ex)
                    {
                         dlg = new MessageDialog(this, DialogFlags.Modal, 
                            MessageType.Error, ButtonsType.Close,
                            $"Failed to clear 'chunks' folder:\n\n{ex.Message} ({ex.HResult})");

                        dlg.Run(); dlg.Destroy();
                    }
                }

                dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.YesNo,
                        "Open destination folder?");

                var result = (ResponseType)dlg.Run();

                if (result == ResponseType.Yes)
                    _vm.OpenOutputFolder();

                dlg.Destroy();
            }
            else if (e == BrcRenderResult.Aborted)
            {
                Status("Operation aborted");
            }
            else
            {
                // ToDo: Share string resources
                var dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.Close,
                        "An unexpected error ocurred");
                dlg.Run(); dlg.Destroy();
                Status("Unexpected error");
            }
        }

        async void On_RenderMixdown_Clicked(object s, EventArgs e)
        {
            _vm.IsBusy = 
            workSpinner.Active = true;
            ResetCTS();

            Status("Rendering mixdown...");

            var mixcmd = new MixdownCmd(_settings.BlenderProgram)
            {
                BlendFile = _vm.Project.BlendFilePath,
                Range = _vm.Project.ChunkList.GetFullRange(),
                MixdownScript = Scripts.MixdownAudio,
                OutputFolder = _vm.Project.OutputPath
            };

            var result = await mixcmd.RunAsync(_afterRenderCancelSrc.Token);

            if (result == 0)
            {
                Status("Mixdown complete");
            }
            else
            {
                //MessageBox.Show("Something went wrong, check logs at the output folder...",
                //        Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);

                var msgBox = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok,
                    "Something went wrong, check logs at the output folder...");

                mixcmd.SaveReport(_vm.Project.OutputPath);

                Status("Something went wrong...");

                msgBox.Run(); msgBox.Destroy();
            }

            _vm.IsBusy =
            workSpinner.Active = false;
        }

        void On_JoinChunks_Clicked(object s, EventArgs e)
        {
            _vm.IsBusy =
            workSpinner.Active = true;
            ResetCTS();

            // TODO: Manual concat dialog

            _vm.IsBusy =
            workSpinner.Active = false;

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

            _settings.RecentProjects = new Infra.RecentBlendsCollection(orderedItems);

            Console.WriteLine("Recent items" + string.Join(", ", orderedItems));
        }


        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var vm = (BrcViewModel)sender;

            Console.WriteLine("Propchanged invoked, Name = {0}", e.PropertyName);
            Console.WriteLine(vm);

            startStopStack.Sensitive = vm.CanRender;
            miUnload.Sensitive = vm.CanEditCurrentProject;

            miRenderMixdown.Sensitive = vm.CanRender && !vm.IsBusy;
            miJoinChunks.Sensitive = !vm.IsBusy;

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

            UpdateInfoBoxItems(vm.Project);
            UpdateOptions(vm.Project);
        }


        // Dialog handlers

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

        void On_BlenderFileSet(object s, EventArgs e)
        {
            var chooser = (FileChooserButton)s;
            _settings.BlenderProgram = chooser.Filename;
        }

        void On_FFmpegFileSet(object s, EventArgs e)
        {
            var chooser = (FileChooserButton)s;
            _settings.FFmpegProgram = chooser.Filename;
        }

        void On_Showtooltips_Toggle(object s, EventArgs e)
        {
            var tgl = (CheckButton)s;
            _settings.DisplayToolTips = tgl.Active;
        }

        void On_DeleteChunks_Toggle(object s, EventArgs e)
        {
            var tgl = (CheckButton)s;
            _settings.DeleteChunksFolder = tgl.Active;
        }

        void On_LoggingLvl_Changed(object s, EventArgs e)
        {
            var cb = (ComboBox)s;
            _settings.LoggingLevel = cb.Active;
        }

        void On_PrefOk_Clicked(object s, EventArgs e)
        {
            prefWin.Hide();
        }

    }
}
