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

        const string progId = nameof(BlenderRenderController);

        CancellationTokenSource _afterRenderCancelSrc;

        BrcMainViewModel _vm;


        public BrcForm()
        {
            InitializeComponent();

            _vm = new BrcMainViewModel();
            _vm.PropertyChanged += ViewModel_PropertyChanged;
            _vm.OnRenderFinished = OnRenderFinishedHandler;

            TaskbarManager.Instance.ApplicationId = progId;
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

            // invoke manually to set starting state
            ViewModel_PropertyChanged(_vm, new PropertyChangedEventArgs("Created"));

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

            // extra bindings
            totalStartNumericUpDown.DataBindings.Add("Enabled", startEndCustomRadio, "Checked");
            totalEndNumericUpDown.DataBindings.Add("Enabled", startEndCustomRadio, "Checked");

            chunkLengthNumericUpDown.DataBindings.Add("Enabled", chunkOptionsCustomRadio, "Checked");
            processCountNumericUpDown.DataBindings.Add("Enabled", chunkOptionsCustomRadio, "Checked");

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
                    StopWork(false);
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

            //Status(_vm.DefaultStatusMessage);
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

        private void renderAllButton_Click(object sender, EventArgs e)
        {
            if (_vm.IsBusy)
            {
                var result = MessageBox.Show("Are you sure you want to stop?",
                                                "Cancel",
                                                MessageBoxButtons.YesNo,
                                                MessageBoxIcon.Exclamation);

                if (result == DialogResult.No)
                    return;

                // cancel
                StopWork(false);
            }
            else
            {
                var outputDir = _vm.Project.OutputPath;

                if ((Directory.Exists(outputDir) && Directory.GetFiles(outputDir).Length > 0)
                    || Directory.Exists(_vm.Project.DefaultChunksDirPath))
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

                RenderAll();
            }
        }

        #endregion

        #region UpdateElements

        private void UpdateProgress(RenderProgressInfo info)
        {
            float progPct = info.FramesRendered / (float)_vm.Project.TotalFrames;

            Status($"Completed {info.PartsCompleted} / {_vm.Project.ChunkList.Count} chunks, " +
                $"{info.FramesRendered} frames rendered");

            UpdateProgressBars((int)(progPct * 100));

            _etaCalc.Update(progPct);

            if (_etaCalc.ETAIsAvailable)
            {
                var etr = ETR_Prefix + _etaCalc.ETR.ToString(TimeFmt);
                Status(etr, statusETR);
            }

            //time elapsed display
            TimeSpan runTime = _chrono.Elapsed;
            var tElapsed = TimePassedPrefix + runTime.ToString(TimeFmt);
            Status(tElapsed, statusTime);
        }

        void UpdateProgressBars(int progressPercent = 0)
        {
            string titleProg = "Blender Render Controller";

            if (progressPercent < 0)
            {
                renderProgressBar.Style = ProgressBarStyle.Marquee;
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Indeterminate);
            }
            else
            {
                renderProgressBar.Style = ProgressBarStyle.Blocks;
                renderProgressBar.Value = progressPercent;

                if (progressPercent != 0)
                {
                    titleProg = $"{progressPercent}% complete - Blender Render Controller";
                    TaskbarManager.Instance.SetProgressValue(progressPercent, 100);
                }
                else
                    TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress);
            }

            SafeChangeText(titleProg, this);
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
        
        private void SafeChangeText(string msg, Control ctrl)
        {
            if (ctrl.InvokeRequired)
            {
                ctrl.Invoke(new Action<string, Control>(SafeChangeText), msg, ctrl);
            }
            else
            {
                ctrl.Text = msg;
            }
        }

        private void Status(string msg, ToolStripItem tsItem)
        {
            if (tsItem == null) tsItem = statusMessage;

            if (InvokeRequired)
                Invoke(new Action<string, ToolStripItem>(Status), msg, tsItem);
            else
                tsItem.Text = msg;
        }

        void Status(string msg) => Status(msg, null);

        #endregion


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
                var cct = await _vm.RunConcat(mc.ConcatTextFile, mc.OutputFile, mc.MixdownFile);

                if (cct.ExitCode != 0)
                {
                    var report = concat.GenerateReport();
                    var dlg = new Ui.DetailedMessageBox("Failed to concatenate chunks", "Error", report);

                    dlg.ShowDialog();
                }
            }
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

        private void StartEndOptionsRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (startEndBlendRadio.Checked)
            {
                // set to blend values
                _vm.Project.ResetRange();
            }
        }

        private void ChunkOptionsRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (chunkOptionsAutoRadio.Checked)
            {
                _vm.Project.MaxConcurrency = Environment.ProcessorCount;
                // recalc auto chunks:
                var currentStart = totalStartNumericUpDown.Value;
                var currentEnd = totalEndNumericUpDown.Value;
                var currentProcessors = processCountNumericUpDown.Value;

                var expectedChunkLen = Math.Ceiling((currentEnd - currentStart + 1) / currentProcessors);

                _vm.Project.ChunkLenght = (int)expectedChunkLen;
            }
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
                InitialDirectory = _vm.Project.OutputPath,
                Title = dialogTxt,
            };

            var res = openFolder.ShowDialog();

            if (res == CommonFileDialogResult.Ok)
            {
                _vm.Project.OutputPath = openFolder.FileName;
            }
        }

        private void StartEndNumeric_Validated(object sender, EventArgs e)
        {
            var currentStart = totalStartNumericUpDown.Value;
            var currentEnd = totalEndNumericUpDown.Value;
            var currentProcessors = processCountNumericUpDown.Value;

            if (chunkOptionsAutoRadio.Checked)
            {
                var expectedChunkLen = Math.Ceiling((currentEnd - currentStart + 1) / currentProcessors);
                _vm.Project.ChunkLenght = (int)expectedChunkLen;
            }

            // set max chunk size to total frames
            chunkLengthNumericUpDown.Maximum = currentEnd - currentStart + 1;
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
            _vm.Project = null;
        }

        private void AboutBRC_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void miGithub_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/RedRaptor93/BlenderRenderController");
        }


        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var vm = (BrcViewModel)sender;

            if (vm.IsBusy)
            {
                renderAllButton.Text = "Stop Render";
                renderAllButton.Image = Resources.stop_icon;

                if (!projectBindingSrc.IsBindingSuspended)
                    projectBindingSrc.SuspendBinding();
            }
            else
            {
                renderAllButton.Text = "Start Render";
                renderAllButton.Image = Resources.render_icon;

                if (projectBindingSrc.IsBindingSuspended)
                    projectBindingSrc.ResumeBinding();
            }

            renderAllButton.Enabled = vm.CanRender;


            unloadToolStripMenuItem.Enabled = vm.CanEditCurrentProject;

            miRenderMixdown.Enabled = vm.CanRender && !vm.IsBusy;
            miJoinChunks.Enabled = !vm.IsBusy;

            miSettings.Enabled = !vm.IsBusy;

            miReloadCurrent.Enabled =
            reloadTSButton.Enabled = vm.CanReloadCurrentProject;

            frOutputFolder.Enabled = vm.CanEditCurrentProject;

            panelChunkSize.Enabled =
            panelFrameRange.Enabled = vm.CanEditCurrentProject;

            cbRenderer.Enabled =
            cbAfterRenderAction.Enabled = !vm.IsBusy;

            miOpenFile.Enabled =
            openFileTSButton.Enabled = vm.CanLoadNewProject;

            miOpenRecent.Enabled =
            openRecentsTSButton.Enabled = vm.CanLoadNewProject;

            blendNameLabel.Visible = vm.ProjectLoaded;

            statusETR.Visible = 
            statusTime.Visible = vm.IsBusy;

            Status(vm.DefaultStatusMessage);
        }
    }

}
