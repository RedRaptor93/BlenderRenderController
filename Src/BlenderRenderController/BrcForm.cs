// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using BRCRes = BRClib.Properties.Resources;
using BlenderRenderController.Properties;
using BlenderRenderController.Ui;
using BRClib;
using BRClib.Commands;
using BRClib.ViewModels;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.Taskbar;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlenderRenderController
{
    using static Global;

    /// <summary>
    /// Main Window
    /// </summary>
    public partial class BrcForm : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        const string TimePassedPrefix = "Time: ",
                     ETR_Prefix = "ETR: ",
                     TimeFmt = @"hh\:mm\:ss";

        BrcMainViewModel _vm;
        AboutBox _aboutBox = new AboutBox();


        public BrcForm()
        {
            InitializeComponent();

            _vm = new BrcMainViewModel();
            _vm.PropertyChanged += ViewModel_PropertyChanged;
            _vm.OnRenderFinished = OnRenderFinishedHandler;
            projectBindingSrc.DataSource = _vm;

            TaskbarManager.Instance.ApplicationId = nameof(BlenderRenderController);
        }


        public BrcForm(string blendFile) : this()
        {
            if (CheckProgramPaths())
            {
                // window must be visible
                Show();
                GetBlendInfo(blendFile);
            }
        }


        private void BrcForm_Load(object sender, EventArgs e)
        {
            _vm.ConfigOk = CheckProgramPaths();

            SetStartingState();

            // setup sources for ComboBoxes
            SetComboBoxData(cbRenderer, CustomRes.RendererResources, Settings.Renderer);
            cbRenderer.SelectedIndexChanged += Renderer_Changed;

            SetComboBoxData(cbAfterRenderAction, CustomRes.AfterRenderResources, Settings.AfterRender);
            cbAfterRenderAction.SelectedIndexChanged += AfterRenderAction_Changed;

            // load recent blends from file
            UpdateRecentBlendsMenu();

            processCountNumericUpDown.Maximum = Environment.ProcessorCount;

            // Time duration format
            infoDuration.DataBindings["Value"].Format += (fs, fe) =>
            {
                if (!Convert.IsDBNull(fe.Value) && fe.Value != null)
                {
                    fe.Value = string.Format("{0:%h}h {0:%m}m {0:%s}s {0:%f}ms", (TimeSpan)fe.Value);
                }
            };

            exitToolStripMenuItem.Click += delegate { Close(); };

            // set tooltip text to infoItems childs
            foreach (Control ctrl in infoBox.Controls)
            {
                var caption = toolTipInfo.GetToolTip(ctrl);
                if (string.IsNullOrEmpty(caption)) continue;
                foreach (Control subItem in ctrl.Controls)
                {
                    toolTipInfo.SetToolTip(subItem, caption);
                }
            }

        }


        private void BrcForm_Shown(object sender, EventArgs e)
        {
            logger.Info("Program Started");

            if (!_vm.ConfigOk)
            {
                var settingsForm = new SettingsForm();
                settingsForm.FormClosed += SettingsForm_FormClosed;

                var td = new TaskDialog
                {
                    Caption = "Setup required",
                    InstructionText = "Paths missing",
                    Text = BRCRes.AppErr_RequiredProgramsNotFound,
                    Icon = TaskDialogStandardIcon.Warning,
                    StandardButtons = TaskDialogStandardButtons.Close,
                    OwnerWindowHandle = this.Handle
                };

                var tdCmdLink = new TaskDialogCommandLink("BtnOpenSettings", "Goto Settings");
                tdCmdLink.Click += delegate
                {
                    settingsForm.Show(this);
                    td.Close();
                };

                td.Controls.Add(tdCmdLink);
                td.Show();

            }
        }

        private void BrcForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_vm.IsBusy)
            {
                var result = MessageBox.Show(
                             "Closing now will cancel the rendering process. Close anyway?",
                             "Render in progress",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
					_vm.StopRender();
					_vm.CancelExtraTasks();
                }
            }

            logger.Info("Program closing");
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // when closing the Settings window, check if valid
            // and update UI if needed
            _vm.ConfigOk = CheckProgramPaths();
        }

        private void SetComboBoxData<T>(ComboBox cb, IDictionary<T, string> data, T selected)
        {
            cb.DisplayMember = "Value";
            cb.ValueMember = "Key";
            cb.DataSource = data.ToList();
            cb.SelectedValue = selected;
        }


        #region BlendFileInfo

        private async void GetBlendInfo(string blendFile)
        {
            logger.Info("Loading .blend");

            var (retcode, cmd) = await _vm.OpenBlend(blendFile);
            // success = retcode >= 0;
            // showDlg = retcode != 0;

            switch (retcode)
            {
                case -1: // ERROR File not found
                    MessageBox.Show("File not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                case -2: // ERROR no info receved
                    var d = new DetailedMessageBox(BRCRes.AppErr_NoInfoReceived, "Error", cmd.GenerateReport()).ShowDialog();

                    if (d == DialogResult.Retry)
                        GetBlendInfo(blendFile);
                    else
                        return;

                    break;
                case 1: // WARN RenderFormat is image
                    var eMsg = string.Format(BRCRes.AppErr_RenderFormatIsImage, _vm.Data.FileFormat);
                    MessageBox.Show(BRCRes.AppErr_RenderFormatIsImage, "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 2: // WARN Invalid output path
                        // use .blend folder path if outputPath is unset, display a warning about it
                    MessageBox.Show(BRCRes.AppErr_BlendOutputInvalid, "Warning",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }

            AddRecentItem(blendFile);
            // reset binding source?
        }

        void AddRecentItem(string item)
        {
            int eIdx = Settings.RecentProjects.IndexOf(item);
            if (eIdx == 0) return;
            if (eIdx > 0)
            {
                Settings.RecentProjects.RemoveAt(eIdx);
                Settings.RecentProjects.Insert(0, item);
                return;
            }

            // remove items that excede capacity
            int count = Settings.RecentProjects.Count;
            while (--count >= 10)
            {
                Settings.RecentProjects.RemoveAt(count);
            }

            Settings.RecentProjects.Insert(0, item);
            UpdateRecentBlendsMenu();
        }

        private void OpenBlend_Click(object sender, EventArgs e)
        {
            var result = openBlendDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                var blend = openBlendDialog.FileName;
                GetBlendInfo(blend);
            }
        }

        private void ReloadBlend_Click(object sender, EventArgs e)
        {
            var blend = _vm.BlendFile;
            if (!string.IsNullOrEmpty(blend))
            {
                GetBlendInfo(blend);
            }

            _vm.Footer = _vm.ProjectLoaded ? "Ready" : "Select a file";
        }

        private void RecentBlendsItem_Click(object sender, EventArgs e)
        {
            var recentItem = (ToolStripMenuItem)sender;
            var blendPath = recentItem.ToolTipText;

            if (!File.Exists(blendPath))
            {
                var res = MessageBox.Show("Blend file not found, remove it from the recents list?", "Not found",
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (res == DialogResult.Yes)
                {
                    Settings.RecentProjects.Remove(blendPath);
                }

                return;
            }

            GetBlendInfo(blendPath);
        }
        #endregion

        #region RenderMethods

        private void OnRenderFinishedHandler(BrcRenderResult e)
        {
            // all slow work is done
            //StopWork(true);

            if (e == BrcRenderResult.AllOk)
            {
                if (Settings.AfterRender != AfterRenderAction.NOTHING &&
                    Settings.DeleteChunksFolder)
                {
                    try
                    {
                        Directory.Delete(_vm.DefaultChunksDirPath, true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to clear 'chunks' folder:\n\n" + ex.Message,
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                }

                var dialog = MessageBox.Show("Open destination folder?", "Work complete!",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Information);

                if (dialog == DialogResult.Yes)
                    OpenOutputFolder();
            }
            else if (e == BrcRenderResult.AfterRenderFailed)
            {
                MessageBox.Show(BRCRes.RM_AfterRenderFailed, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                _vm.Footer = "Erros detected";
            }
            else if (e == BrcRenderResult.Aborted)
            {
                _vm.Footer = "Operation Aborted";
            }
            else
            {
                MessageBox.Show(BRCRes.RM_unexpected_error, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _vm.Footer = "Unexpected error";
            }

        }

        void BtnStartWork_Click(object s, EventArgs e)
		{
			Debug.Assert(_vm.IsNotBusy, "Bad state!");

			var outputDir = _vm.OutputPath;

            if ((Directory.Exists(outputDir) && Directory.GetFiles(outputDir).Length > 0)
                || Directory.Exists(_vm.DefaultChunksDirPath))
            {
                var dialogResult = MessageBox.Show("All existing files will be deleted!\n" +
                                                    "Do you want to continue?",
                                                    "Output folder not empty",
                                                    MessageBoxButtons.YesNo,
                                                    MessageBoxIcon.Exclamation);

                if (dialogResult == DialogResult.No)
                    return;

                var tryToClear = Helper.ClearOutputFolder(outputDir);

                if (!tryToClear) return;
            }

			_vm.StartRender();
		}

        void BtnStopWork_Click(object s, EventArgs e)
		{
			Debug.Assert(_vm.IsBusy, "Bad state!");

			var result = MessageBox.Show("Are you sure you want to stop?",
                                                "Cancel",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Exclamation);

            if (result == DialogResult.No)
                return;

            // cancel
			_vm.StopRender();
		}

        #endregion


        void UpdateProgressBars(float prog = 0f)
        {
            string titleProg = "Blender Render Controller";

            if (prog < 0f)
            {
                renderProgressBar.Style = ProgressBarStyle.Marquee;
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Indeterminate);
            }
            else
            {
                var progI = (int)(prog * 100);
                renderProgressBar.Style = ProgressBarStyle.Blocks;
                renderProgressBar.Value = (int)(prog * 100);

                if (prog != 0)
                {
                    titleProg = $"{progI}% complete - Blender Render Controller";
                    TaskbarManager.Instance.SetProgressValue(progI, 100);
                }
                else
                {
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
                }
            }

            SafeSetText(titleProg, this);
        }

        private void UpdateRecentBlendsMenu()
        {
            // a common name, so the menu items can be found later
            const string tsName = "recent";

            // clear local tsMenu items
            var localItems = recentBlendsMenu.Items.Find(tsName, false);
            foreach (var item in localItems)
            {
                recentBlendsMenu.Items.Remove(item);
            }


            // show placeholder if recents list is empty
            if (Settings.RecentProjects.Count == 0)
            {
                miEmptyPH.Visible = true;
                return;
            }
            else
            {
                miEmptyPH.Visible = false;
            }

            // make items from recent list
            foreach (string item in Settings.RecentProjects)
            {
                var menuItem = new ToolStripMenuItem
                {
                    ToolTipText = item,
                    Text = Path.GetFileNameWithoutExtension(item),
                    Image = Resources.blender_icon,
                    Name = tsName
                };
                menuItem.Click += RecentBlendsItem_Click;
                recentBlendsMenu.Items.Add(menuItem);
            }

        }
        
        private void SafeSetText(string msg, Control ctrl)
        {
            if (ctrl.InvokeRequired)
                ctrl.Invoke(new Action<string, Control>(SafeSetText), msg, ctrl);
            else
                ctrl.Text = msg;
        }

        void SafeSetText(string msg, ToolStripItem item)
        {
            if (InvokeRequired)
                Invoke(new Action<string, ToolStripItem>(SafeSetText), msg, item);
            else
                item.Text = msg;
        }


        private async void mixDownButton_Click(object sender, EventArgs e)
        {
            var mix = await _vm.RunMixdown();

            if (mix.ExitCode != 0)
            {
                var report = mix.GenerateReport();
                var dlg = new DetailedMessageBox("Mixdown failed", "Error", report);

                dlg.ShowDialog();
            }
        }

        private async void concatenatePartsButton_Click(object sender, EventArgs e)
        {
            var mc = new ConcatForm();
            var dResult = mc.ShowDialog();

            if (dResult == DialogResult.OK)
            {
				var cct = await _vm.RunConcat(mc.ChunksTextFile, mc.OutputFile, mc.MixdownAudioFile);

                if (cct.ExitCode != 0)
                {
                    var report = cct.GenerateReport();
                    var dlg = new Ui.DetailedMessageBox("Failed to concatenate chunks", "Error", report);

                    dlg.ShowDialog();
                }
            }
        }


        private void OpenOutputFolder()
        {
            if (!_vm.OpenOutputFolder())
            {
                MessageBox.Show("Output folder does not exist.", "",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
            }
        }


        private void Enter_GotoNext(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || (e.KeyCode == Keys.Return))
            {
                SelectNextControl((Control)sender, true, true, true, true);
                e.SuppressKeyPress = true; //disables sound
            }
        }

        private void openOutputFolderButton_Click(object sender, EventArgs e)
        {
            OpenOutputFolder();
        }
        
        private void AfterRenderAction_Changed(object sender, EventArgs e)
        {
            Settings.AfterRender = (AfterRenderAction)cbAfterRenderAction.SelectedValue;
        }

        private void Renderer_Changed(object sender, EventArgs e)
        {
            Settings.Renderer = (Renderer)cbRenderer.SelectedValue;
        }


        private void outputFolderBrowseButton_Click(object sender, EventArgs e)
        {
            string dialogTxt = "Select output location";

            var openFolder = new CommonOpenFileDialog
            {
                IsFolderPicker = true,
                InitialDirectory = _vm.OutputPath,
                Title = dialogTxt,
            };

            var res = openFolder.ShowDialog();

            if (res == CommonFileDialogResult.Ok)
            {
                _vm.OutputPath = openFolder.FileName;
            }
        }

        private void StartEnd_Validating(object sender, CancelEventArgs e)
        {
            if (totalStartNumericUpDown.Value >= totalEndNumericUpDown.Value)
            {
                errorProvider.
                    SetError(totalStartNumericUpDown, "Start frame cannot be equal or greater then End frame");
                e.Cancel = true;
            }
            else if (totalEndNumericUpDown.Value <= totalStartNumericUpDown.Value)
            {
                errorProvider
                    .SetError(totalEndNumericUpDown, "End frame cannot be equal or less then Start frame");
                e.Cancel = true;
            }
            else if ((totalEndNumericUpDown.Value - totalStartNumericUpDown.Value + 1) < 50)
            {
                var msg = "Project must be at least 50 frames long";

                errorProvider.SetError(totalEndNumericUpDown, msg);
                errorProvider.SetError(totalStartNumericUpDown, msg);
                e.Cancel = true;
            }
            else
                errorProvider.Clear();
        }


        private void donateButton_Click(object sender, EventArgs e)
        {
            _vm.OpenDonationPage();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settingsForm = new SettingsForm();
            settingsForm.FormClosed += SettingsForm_FormClosed;
            settingsForm.ShowDialog();
        }

        private void toolStripMenuItemBug_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/RedRaptor93/BlenderRenderController/wiki/Reporting-an-issue");
        }

        private void clearRecentProjectsListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Settings.RecentProjects.Count == 0)
                return;

            var response = MessageBox.Show(
                 "This will clear all files in the recent blends list, are you sure?",
                 "Clear recent blends?",
                 MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (response == DialogResult.Yes)
                Settings.RecentProjects.Clear();
        }

        private void UnloadCurrent_Click(object sender, EventArgs e)
        {
            projectBindingSrc.Clear();
            _vm.UnloadProject();
        }

        private void AboutBRC_Click(object sender, EventArgs e)
        {
            _aboutBox.ShowDialog();
        }

        private void miGithub_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/RedRaptor93/BlenderRenderController");
        }

        void SetStartingState()
        {
            miRenderMixdown.Enabled = _vm.ConfigOk && _vm.ProjectLoaded && _vm.IsNotBusy;
            frOutputFolder.Enabled = _vm.ProjectLoaded && _vm.IsNotBusy;

            miOpenRecent.Enabled =
            miOpenFile.Enabled =
            openFileTSButton.Enabled = _vm.ConfigOk && _vm.IsNotBusy;

            statusETR.Visible = _vm.IsBusy;
            miJoinChunks.Enabled = _vm.IsNotBusy;
            miSettings.Enabled = _vm.IsNotBusy;

            miReloadCurrent.Enabled =
            reloadTSButton.Enabled =
            unloadToolStripMenuItem.Enabled = _vm.ProjectLoaded && _vm.CanLoadMore;

            totalEndNumericUpDown.Enabled =
            totalStartNumericUpDown.Enabled = !_vm.AutoFrameRange;
            chunkLengthNumericUpDown.Enabled = !_vm.AutoChunkSize;
            processCountNumericUpDown.Enabled = !_vm.AutoMaxProcessors;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var vm = (BrcMainViewModel)sender;

            // all of the more complex Binding logic goes here

            miRenderMixdown.Enabled = vm.ConfigOk && vm.ProjectLoaded && vm.IsNotBusy;
            frOutputFolder.Enabled = vm.ProjectLoaded && vm.IsNotBusy;

            if (e.PropertyName == nameof(vm.IsBusy) || e.PropertyName == nameof(vm.ConfigOk))
            {
                if (vm.IsBusy)
                {
                    if (!projectBindingSrc.IsBindingSuspended)
                        projectBindingSrc.SuspendBinding();
                }
                else
                {
                    if (projectBindingSrc.IsBindingSuspended)
                        projectBindingSrc.ResumeBinding();
                }

                // Toolstrip and menu items can't have data binding
                miOpenRecent.Enabled =
                miOpenFile.Enabled =
                openFileTSButton.Enabled = vm.ConfigOk && vm.IsNotBusy;

                statusETR.Visible = vm.IsBusy;
                miJoinChunks.Enabled = vm.IsNotBusy;
                miSettings.Enabled = vm.IsNotBusy;
            }
            else if (e.PropertyName == nameof(vm.ProjectLoaded) || e.PropertyName == nameof(vm.CanLoadMore))
            {
                //panelChunkSize.Enabled =
                //panelFrameRange.Enabled =
                miReloadCurrent.Enabled =
                reloadTSButton.Enabled =
                unloadToolStripMenuItem.Enabled = vm.ProjectLoaded && vm.CanLoadMore;
            }
            else if (e.PropertyName == nameof(vm.Progress))
            {
                UpdateProgressBars(vm.Progress);
                SafeSetText(vm.StatusTime, statusETR);
            }
            else if (e.PropertyName == nameof(vm.Footer))
            {
                SafeSetText(vm.Footer, statusMessage);
            }
            else if (e.PropertyName == nameof(vm.AutoFrameRange))
            {
                totalEndNumericUpDown.Enabled =
                totalStartNumericUpDown.Enabled = !vm.AutoFrameRange;
            }
            else if (e.PropertyName == nameof(vm.AutoChunkSize))
            {
                chunkLengthNumericUpDown.Enabled = !vm.AutoChunkSize;
            }
            else if (e.PropertyName == nameof(vm.AutoMaxProcessors))
            {
                processCountNumericUpDown.Enabled = !vm.AutoMaxProcessors;
            }
        }
    }

}
