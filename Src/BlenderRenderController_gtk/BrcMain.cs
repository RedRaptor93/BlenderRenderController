using BlenderRenderController.Services;
using BRClib;
using BRClib.Commands;
using BRClib.Extentions;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gtk;

using PathIO = System.IO.Path;

namespace BlenderRenderController
{
    public partial class BrcMain : WindowBase
    {
        BrcSettings _settings;
        BrcViewModel _vm;
        int _autoStartF, _autoEndF;

        public BrcMain() : base("BrcGtk.glade", "brc_style.css", "BrcMain")
        {
            Initialize();

            _settings = Services.Settings.Current;
            _vm = new BrcViewModel();
            _vm.PropertyChanged += ViewModel_PropertyChanged;
            _vm.ConfigOk = true;
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
            lblStatus.Text = "Loading " + PathIO.GetFileName(blendFile) + " ...";
            workSpinner.Active = true;

            await _vm.GetBlendInfo(blendFile);

            _autoStartF = _vm.Project.Start;
            _autoEndF = _vm.Project.End;

            workSpinner.Active = false;
        }

        // Events handlers

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

        private void On_Preferences(object sender, EventArgs e)
        {
            
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

        }

        void On_cbRenderer_Changed(object s, EventArgs e)
        {

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

            btnStopRender.Show();
            startStopStack.VisibleChild = btnStopRender;
            btnStopRender.GrabFocus();
        }

        void StopRender_Clicked(object s, EventArgs e)
        {
            // stop / cancel render...

            btnStartRender.Show();
            startStopStack.VisibleChild = btnStartRender;
            btnStartRender.GrabFocus();
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

        void On_ShowAbout(object s, EventArgs e)
        {
            //var about = new AboutWin(Builder);
            aboutWin.Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            aboutWin.Run();
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

    }
}
