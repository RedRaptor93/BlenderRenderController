using BlenderRenderController.Services;
using BRClib;
using BRClib.Commands;
using BRClib.Extentions;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Gtk;

using PathIO = System.IO.Path;

namespace BlenderRenderController
{
    public partial class BrcMain : GtkWindow
    {
        BrcSettings _settings;
        BrcViewModel _vm;
        int _autoStartF, _autoEndF;

        public BrcMain() : base("BrcGtk.glade", "brc_style.css", "BrcMain")
        {
            Initialize();

            _settings = Services.Settings.Current;
            _vm = new BrcViewModel();
            _vm.ConfigOk = true;
            _vm.PropertyChanged += ViewModel_PropertyChanged;
            ViewModel_PropertyChanged(_vm, new System.ComponentModel.PropertyChangedEventArgs("Created"));


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
                    "%s", errMsg);

                msgDialog.Run();
            }

            return result;
        }

        void UpdateInfoBoxItems(Project project)
        {
            activeSceneInfoValue.Text = project.ActiveScene;
            durationInfoValue.Text = project.Duration.HasValue 
                ? string.Format("{0:%h}h {0:%m}m {0:%s}s {0:%f}ms", project.Duration.Value)
                : null;
            fpsInfoValue.Text = project.Fps.ToString("D2");
            resolutionInfoValue.Text = project.Resolution;
        }

        // Events handlers
        private async void On_OpenFile(object sender, EventArgs e)
        {
            string blendFile = null;
            var result = openBlendDialog.Run();

            if (result == (int)ResponseType.Accept)
            {
                blendFile = openBlendDialog.Filename;
                lblStatus.Text = "Loading " + PathIO.GetFileName(blendFile) + " ...";
                workSpinner.Active = true;

                await _vm.GetBlendInfo(blendFile);

                workSpinner.Active = false;

                UpdateInfoBoxItems(_vm.Project);
            }


            openBlendDialog.Hide();
        }

        private void On_ReloadFile(object sender, EventArgs e)
        {
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
            //var cb = (ComboBox)s;

        }

        void On_cbRenderer_Changed(object s, EventArgs e)
        {

        }

        void On_AutoStartStop_Toggled(object s, EventArgs e)
        {

        }

        void On_AutoChunkSize_Toggled(object s, EventArgs e)
        {

        }

        private void NumAdjust_ValueChanged(object sender, EventArgs e)
        {
            var adjust = (Adjustment)sender;
        }

        void On_numFrameRange_ValueChanged(object s, EventArgs e)
        {
            var spinBtn = (SpinButton)s;

            Console.WriteLine("Name = " + spinBtn.Name);

            var startFrame = numStartFrameAdjust.Value;
            var endFrame = numEndFrameAdjust.Value;

            Console.WriteLine($"Values = {startFrame}-{endFrame}");
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
            var result = chooseOutputFolderDialog.Run();
            if (result == (int)ResponseType.Accept)
            {
                entryOutputPath.Text = chooseOutputFolderDialog.Filename;
            }

            chooseOutputFolderDialog.Hide();
        }

        void BtnOpenFolder_Clicked(object s, EventArgs e)
        {
            
        }

        void On_ShowAbout(object s, EventArgs e)
        {

        }

        private void RecentMngr_Changed(object sender, EventArgs e)
        {
            var items = RecentManager.Default.RecentItems;
            var orderedItems = items.OrderBy(ri => ri.Added).Select(ri => ri.Uri);

            _settings.RecentProjects = new Infra.RecentBlendsCollection(orderedItems);


        }


        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var vm = (BrcViewModel)sender;

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
        }

    }
}
