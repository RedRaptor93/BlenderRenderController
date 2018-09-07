﻿using BlenderRenderController.Ui;

namespace BlenderRenderController
{
    partial class BrcForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrcForm));
            this.renderProgressBar = new System.Windows.Forms.ProgressBar();
            this.totalStartNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.projectBindingSrc = new System.Windows.Forms.BindingSource(this.components);
            this.startFrameLabel = new System.Windows.Forms.Label();
            this.outputFolderTextBox = new System.Windows.Forms.TextBox();
            this.totalFrameCountLabel = new System.Windows.Forms.Label();
            this.totalEndNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.processCountNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.processCountLabel = new System.Windows.Forms.Label();
            this.toolTipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.chunkLengthLabel = new System.Windows.Forms.Label();
            this.chunkLengthNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.renderInfoLabel = new System.Windows.Forms.Label();
            this.btnStartWork = new System.Windows.Forms.Button();
            this.btnStopWork = new System.Windows.Forms.Button();
            this.cbRenderer = new System.Windows.Forms.ComboBox();
            this.cbAfterRenderAction = new System.Windows.Forms.ComboBox();
            this.checkAutoFrame = new System.Windows.Forms.CheckBox();
            this.checkChunkSize = new System.Windows.Forms.CheckBox();
            this.checkMaxProcs = new System.Windows.Forms.CheckBox();
            this.infoActiveScene = new BlenderRenderController.Ui.InfoBoxItem();
            this.infoDuration = new BlenderRenderController.Ui.InfoBoxItem();
            this.infoFPS = new BlenderRenderController.Ui.InfoBoxItem();
            this.infoResolution = new BlenderRenderController.Ui.InfoBoxItem();
            this.recentBlendsMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.miEmptyPH = new System.Windows.Forms.ToolStripMenuItem();
            this.openRecentsTSButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.miOpenRecent = new System.Windows.Forms.ToolStripMenuItem();
            this.blendFileLabel = new System.Windows.Forms.Label();
            this.outputFolderLabel = new System.Windows.Forms.Label();
            this.optionsLabel = new System.Windows.Forms.Label();
            this.openOutputFolderButton = new System.Windows.Forms.Button();
            this.outputFolderBrowseButton = new System.Windows.Forms.Button();
            this.openBlendDialog = new System.Windows.Forms.OpenFileDialog();
            this.flpStartEnd = new System.Windows.Forms.FlowLayoutPanel();
            this.blendNameLabel = new System.Windows.Forms.Label();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.openFileTSButton = new System.Windows.Forms.ToolStripButton();
            this.reloadTSButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.panelFrameRange = new System.Windows.Forms.Panel();
            this.flpChunkMode = new System.Windows.Forms.FlowLayoutPanel();
            this.panelChunkSize = new System.Windows.Forms.Panel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miReloadCurrent = new System.Windows.Forms.ToolStripMenuItem();
            this.unloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miRenderMixdown = new System.Windows.Forms.ToolStripMenuItem();
            this.miJoinChunks = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miGithub = new System.Windows.Forms.ToolStripMenuItem();
            this.miReportBug = new System.Windows.Forms.ToolStripMenuItem();
            this.miDonate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.miAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusETR = new System.Windows.Forms.ToolStripStatusLabel();
            this.frOutputFolder = new System.Windows.Forms.Panel();
            this.infoBox = new System.Windows.Forms.FlowLayoutPanel();
            this.frOptions = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.totalStartNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectBindingSrc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.totalEndNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.processCountNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chunkLengthNumericUpDown)).BeginInit();
            this.recentBlendsMenu.SuspendLayout();
            this.flpStartEnd.SuspendLayout();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.panelFrameRange.SuspendLayout();
            this.flpChunkMode.SuspendLayout();
            this.panelChunkSize.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.frOutputFolder.SuspendLayout();
            this.infoBox.SuspendLayout();
            this.frOptions.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // renderProgressBar
            // 
            this.renderProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.renderProgressBar.Location = new System.Drawing.Point(12, 486);
            this.renderProgressBar.MarqueeAnimationSpeed = 75;
            this.renderProgressBar.Name = "renderProgressBar";
            this.renderProgressBar.Size = new System.Drawing.Size(449, 20);
            this.renderProgressBar.Step = 1;
            this.renderProgressBar.TabIndex = 2;
            this.toolTipInfo.SetToolTip(this.renderProgressBar, "Progress bar");
            // 
            // totalStartNumericUpDown
            // 
            this.totalStartNumericUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.projectBindingSrc, "StartFrame", true));
            this.totalStartNumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorProvider.SetIconAlignment(this.totalStartNumericUpDown, System.Windows.Forms.ErrorIconAlignment.BottomRight);
            this.totalStartNumericUpDown.Location = new System.Drawing.Point(6, 21);
            this.totalStartNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.totalStartNumericUpDown.Name = "totalStartNumericUpDown";
            this.totalStartNumericUpDown.Size = new System.Drawing.Size(90, 22);
            this.totalStartNumericUpDown.TabIndex = 5;
            this.totalStartNumericUpDown.Tag = "";
            this.totalStartNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.totalStartNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.totalStartNumericUpDown.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Enter_GotoNext);
            this.totalStartNumericUpDown.Validating += new System.ComponentModel.CancelEventHandler(this.StartEnd_Validating);
            // 
            // projectBindingSrc
            // 
            this.projectBindingSrc.DataSource = typeof(BRClib.ViewModels.BrcMainViewModel);
            // 
            // startFrameLabel
            // 
            this.startFrameLabel.AutoSize = true;
            this.startFrameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startFrameLabel.Location = new System.Drawing.Point(3, 4);
            this.startFrameLabel.Name = "startFrameLabel";
            this.startFrameLabel.Size = new System.Drawing.Size(71, 15);
            this.startFrameLabel.TabIndex = 6;
            this.startFrameLabel.Text = "Start Frame";
            // 
            // outputFolderTextBox
            // 
            this.outputFolderTextBox.BackColor = System.Drawing.Color.White;
            this.outputFolderTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.projectBindingSrc, "OutputPath", true));
            this.outputFolderTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.outputFolderTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputFolderTextBox.Location = new System.Drawing.Point(0, 0);
            this.outputFolderTextBox.Name = "outputFolderTextBox";
            this.outputFolderTextBox.Size = new System.Drawing.Size(449, 22);
            this.outputFolderTextBox.TabIndex = 11;
            this.outputFolderTextBox.Tag = "";
            this.toolTipInfo.SetToolTip(this.outputFolderTextBox, "Path to folder where your video will be rendered to.\r\n\r\nIt defaults to the output" +
        " folder set in your project.");
            this.outputFolderTextBox.WordWrap = false;
            this.outputFolderTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Enter_GotoNext);
            // 
            // totalFrameCountLabel
            // 
            this.totalFrameCountLabel.AutoSize = true;
            this.totalFrameCountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalFrameCountLabel.Location = new System.Drawing.Point(103, 4);
            this.totalFrameCountLabel.Name = "totalFrameCountLabel";
            this.totalFrameCountLabel.Size = new System.Drawing.Size(68, 15);
            this.totalFrameCountLabel.TabIndex = 13;
            this.totalFrameCountLabel.Text = "End Frame";
            // 
            // totalEndNumericUpDown
            // 
            this.totalEndNumericUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.projectBindingSrc, "EndFrame", true));
            this.totalEndNumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorProvider.SetIconAlignment(this.totalEndNumericUpDown, System.Windows.Forms.ErrorIconAlignment.BottomRight);
            this.totalEndNumericUpDown.Location = new System.Drawing.Point(106, 21);
            this.totalEndNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.totalEndNumericUpDown.Name = "totalEndNumericUpDown";
            this.totalEndNumericUpDown.Size = new System.Drawing.Size(90, 22);
            this.totalEndNumericUpDown.TabIndex = 6;
            this.totalEndNumericUpDown.Tag = "";
            this.totalEndNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.totalEndNumericUpDown.Value = new decimal(new int[] {
            123456,
            0,
            0,
            0});
            this.totalEndNumericUpDown.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Enter_GotoNext);
            this.totalEndNumericUpDown.Validating += new System.ComponentModel.CancelEventHandler(this.StartEnd_Validating);
            // 
            // processCountNumericUpDown
            // 
            this.processCountNumericUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.projectBindingSrc, "MaxCores", true));
            this.processCountNumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processCountNumericUpDown.Location = new System.Drawing.Point(102, 21);
            this.processCountNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.processCountNumericUpDown.Name = "processCountNumericUpDown";
            this.processCountNumericUpDown.Size = new System.Drawing.Size(88, 22);
            this.processCountNumericUpDown.TabIndex = 8;
            this.processCountNumericUpDown.Tag = "";
            this.processCountNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTipInfo.SetToolTip(this.processCountNumericUpDown, "Maximum number of Blender processes that will run in parallel when rendering your" +
        " video.\r\n\r\nIf \"Auto\" is checked, Max = number of your logical processors.");
            this.processCountNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.processCountNumericUpDown.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Enter_GotoNext);
            // 
            // processCountLabel
            // 
            this.processCountLabel.AutoSize = true;
            this.processCountLabel.Location = new System.Drawing.Point(99, 3);
            this.processCountLabel.Name = "processCountLabel";
            this.processCountLabel.Size = new System.Drawing.Size(66, 15);
            this.processCountLabel.TabIndex = 15;
            this.processCountLabel.Text = "Max Cores";
            this.toolTipInfo.SetToolTip(this.processCountLabel, "Maximum number of Blender processes that will run in parallel when rendering your" +
        " video.\r\n\r\nIf \"Auto\" is checked, Max = number of your logical processors.");
            // 
            // toolTipInfo
            // 
            this.toolTipInfo.AutoPopDelay = 32767;
            this.toolTipInfo.InitialDelay = 500;
            this.toolTipInfo.ReshowDelay = 100;
            this.toolTipInfo.ShowAlways = true;
            // 
            // chunkLengthLabel
            // 
            this.chunkLengthLabel.AutoSize = true;
            this.chunkLengthLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chunkLengthLabel.Location = new System.Drawing.Point(1, 3);
            this.chunkLengthLabel.Name = "chunkLengthLabel";
            this.chunkLengthLabel.Size = new System.Drawing.Size(69, 15);
            this.chunkLengthLabel.TabIndex = 13;
            this.chunkLengthLabel.Text = "Chunk Size";
            this.toolTipInfo.SetToolTip(this.chunkLengthLabel, "The size in frames of the segment each process will work on.\r\n\r\nIf \"Auto\" is chec" +
        "ked, the video frames will be evenly divided across all avalible cores \r\n- this " +
        "is recomended for most cases.");
            // 
            // chunkLengthNumericUpDown
            // 
            this.chunkLengthNumericUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.projectBindingSrc, "ChunkSize", true));
            this.chunkLengthNumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chunkLengthNumericUpDown.Location = new System.Drawing.Point(4, 21);
            this.chunkLengthNumericUpDown.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.chunkLengthNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.chunkLengthNumericUpDown.Name = "chunkLengthNumericUpDown";
            this.chunkLengthNumericUpDown.Size = new System.Drawing.Size(86, 22);
            this.chunkLengthNumericUpDown.TabIndex = 7;
            this.chunkLengthNumericUpDown.Tag = "";
            this.chunkLengthNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTipInfo.SetToolTip(this.chunkLengthNumericUpDown, "The size in frames of the segment each process will work on.\r\n\r\nIf \"Auto\" is chec" +
        "ked, the video frames will be evenly divided across all avalible cores \r\n- this " +
        "is recomended for most cases.");
            this.chunkLengthNumericUpDown.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.chunkLengthNumericUpDown.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Enter_GotoNext);
            // 
            // renderInfoLabel
            // 
            this.renderInfoLabel.AutoSize = true;
            this.renderInfoLabel.Image = global::BlenderRenderController.Properties.Resources.info_icon;
            this.renderInfoLabel.Location = new System.Drawing.Point(308, 441);
            this.renderInfoLabel.Name = "renderInfoLabel";
            this.renderInfoLabel.Padding = new System.Windows.Forms.Padding(8, 3, 8, 3);
            this.renderInfoLabel.Size = new System.Drawing.Size(16, 19);
            this.renderInfoLabel.TabIndex = 35;
            this.toolTipInfo.SetToolTip(this.renderInfoLabel, resources.GetString("renderInfoLabel.ToolTip"));
            // 
            // btnStartWork
            // 
            this.btnStartWork.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.projectBindingSrc, "ProjectLoaded", true));
            this.btnStartWork.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartWork.Image = global::BlenderRenderController.Properties.Resources.render_icon;
            this.btnStartWork.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStartWork.Location = new System.Drawing.Point(155, 441);
            this.btnStartWork.Name = "btnStartWork";
            this.btnStartWork.Padding = new System.Windows.Forms.Padding(10, 0, 12, 0);
            this.btnStartWork.Size = new System.Drawing.Size(147, 34);
            this.btnStartWork.TabIndex = 13;
            this.btnStartWork.Tag = "";
            this.btnStartWork.Text = "Start Render";
            this.btnStartWork.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTipInfo.SetToolTip(this.btnStartWork, "Saves a lot of your time.");
            this.btnStartWork.UseVisualStyleBackColor = true;
            this.btnStartWork.Click += new System.EventHandler(this.BtnStartWork_Click);
            // 
            // btnStopWork
            // 
            this.btnStopWork.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopWork.Image = global::BlenderRenderController.Properties.Resources.stop_icon;
            this.btnStopWork.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStopWork.Location = new System.Drawing.Point(155, 441);
            this.btnStopWork.Name = "btnStopWork";
            this.btnStopWork.Padding = new System.Windows.Forms.Padding(10, 0, 12, 0);
            this.btnStopWork.Size = new System.Drawing.Size(147, 34);
            this.btnStopWork.TabIndex = 13;
            this.btnStopWork.Tag = "";
            this.btnStopWork.Text = "Stop Render";
            this.btnStopWork.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTipInfo.SetToolTip(this.btnStopWork, "Cancel work");
            this.btnStopWork.UseVisualStyleBackColor = true;
            this.btnStopWork.Visible = false;
            this.btnStopWork.Click += new System.EventHandler(this.BtnStopWork_Click);
            // 
            // cbRenderer
            // 
            this.cbRenderer.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.projectBindingSrc, "IsNotBusy", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbRenderer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRenderer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRenderer.FormattingEnabled = true;
            this.cbRenderer.Location = new System.Drawing.Point(0, 20);
            this.cbRenderer.Name = "cbRenderer";
            this.cbRenderer.Size = new System.Drawing.Size(184, 21);
            this.cbRenderer.TabIndex = 53;
            this.toolTipInfo.SetToolTip(this.cbRenderer, "Renderer");
            // 
            // cbAfterRenderAction
            // 
            this.cbAfterRenderAction.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", this.projectBindingSrc, "IsNotBusy", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbAfterRenderAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAfterRenderAction.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbAfterRenderAction.FormattingEnabled = true;
            this.cbAfterRenderAction.Location = new System.Drawing.Point(0, 19);
            this.cbAfterRenderAction.Name = "cbAfterRenderAction";
            this.cbAfterRenderAction.Size = new System.Drawing.Size(189, 21);
            this.cbAfterRenderAction.TabIndex = 51;
            this.toolTipInfo.SetToolTip(this.cbAfterRenderAction, "Joining action");
            // 
            // checkAutoFrame
            // 
            this.checkAutoFrame.AutoSize = true;
            this.checkAutoFrame.Checked = true;
            this.checkAutoFrame.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkAutoFrame.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.projectBindingSrc, "AutoFrameRange", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkAutoFrame.Location = new System.Drawing.Point(3, 3);
            this.checkAutoFrame.Name = "checkAutoFrame";
            this.checkAutoFrame.Size = new System.Drawing.Size(101, 19);
            this.checkAutoFrame.TabIndex = 0;
            this.checkAutoFrame.Text = "Whole project";
            this.toolTipInfo.SetToolTip(this.checkAutoFrame, "Use project\'s start and end frame values");
            this.checkAutoFrame.UseVisualStyleBackColor = true;
            // 
            // checkChunkSize
            // 
            this.checkChunkSize.AutoSize = true;
            this.checkChunkSize.Checked = true;
            this.checkChunkSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkChunkSize.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.projectBindingSrc, "AutoChunkSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkChunkSize.Location = new System.Drawing.Point(3, 3);
            this.checkChunkSize.Name = "checkChunkSize";
            this.checkChunkSize.Size = new System.Drawing.Size(50, 19);
            this.checkChunkSize.TabIndex = 0;
            this.checkChunkSize.Text = "Auto";
            this.toolTipInfo.SetToolTip(this.checkChunkSize, "Automaticaly calculate chunk size");
            this.checkChunkSize.UseVisualStyleBackColor = true;
            // 
            // checkMaxProcs
            // 
            this.checkMaxProcs.AutoSize = true;
            this.checkMaxProcs.Checked = true;
            this.checkMaxProcs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkMaxProcs.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.projectBindingSrc, "AutoMaxCores", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkMaxProcs.Location = new System.Drawing.Point(106, 3);
            this.checkMaxProcs.Margin = new System.Windows.Forms.Padding(50, 3, 3, 3);
            this.checkMaxProcs.Name = "checkMaxProcs";
            this.checkMaxProcs.Size = new System.Drawing.Size(50, 19);
            this.checkMaxProcs.TabIndex = 1;
            this.checkMaxProcs.Text = "Auto";
            this.toolTipInfo.SetToolTip(this.checkMaxProcs, "Use all available cores");
            this.checkMaxProcs.UseVisualStyleBackColor = true;
            // 
            // infoActiveScene
            // 
            this.infoActiveScene.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.projectBindingSrc, "ActiveScene", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, "..."));
            this.infoActiveScene.Location = new System.Drawing.Point(9, 9);
            this.infoActiveScene.Margin = new System.Windows.Forms.Padding(4);
            this.infoActiveScene.MinimumSize = new System.Drawing.Size(34, 36);
            this.infoActiveScene.Name = "infoActiveScene";
            this.infoActiveScene.Size = new System.Drawing.Size(105, 42);
            this.infoActiveScene.TabIndex = 0;
            this.infoActiveScene.Title = "Active Scene";
            this.toolTipInfo.SetToolTip(this.infoActiveScene, "The currently active scene. Which will be rendered by this tool.\r\n");
            this.infoActiveScene.Value = "...";
            // 
            // infoDuration
            // 
            this.infoDuration.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.projectBindingSrc, "Duration", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, "..."));
            this.infoDuration.Location = new System.Drawing.Point(122, 9);
            this.infoDuration.Margin = new System.Windows.Forms.Padding(4);
            this.infoDuration.MinimumSize = new System.Drawing.Size(34, 36);
            this.infoDuration.Name = "infoDuration";
            this.infoDuration.Size = new System.Drawing.Size(131, 42);
            this.infoDuration.TabIndex = 2;
            this.infoDuration.Title = "Duration";
            this.toolTipInfo.SetToolTip(this.infoDuration, "Total time");
            this.infoDuration.Value = "...";
            // 
            // infoFPS
            // 
            this.infoFPS.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.projectBindingSrc, "Fps", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, "...", "N2"));
            this.infoFPS.Location = new System.Drawing.Point(261, 9);
            this.infoFPS.Margin = new System.Windows.Forms.Padding(4);
            this.infoFPS.MinimumSize = new System.Drawing.Size(34, 36);
            this.infoFPS.Name = "infoFPS";
            this.infoFPS.Size = new System.Drawing.Size(78, 42);
            this.infoFPS.TabIndex = 4;
            this.infoFPS.Title = "FPS";
            this.toolTipInfo.SetToolTip(this.infoFPS, "Framerate");
            this.infoFPS.Value = "...";
            // 
            // infoResolution
            // 
            this.infoResolution.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.projectBindingSrc, "Resolution", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged, "..."));
            this.infoResolution.Location = new System.Drawing.Point(347, 9);
            this.infoResolution.Margin = new System.Windows.Forms.Padding(4);
            this.infoResolution.MinimumSize = new System.Drawing.Size(34, 36);
            this.infoResolution.Name = "infoResolution";
            this.infoResolution.Size = new System.Drawing.Size(91, 42);
            this.infoResolution.TabIndex = 3;
            this.infoResolution.Title = "Resolution";
            this.toolTipInfo.SetToolTip(this.infoResolution, "Video resolution");
            this.infoResolution.Value = "...";
            // 
            // recentBlendsMenu
            // 
            this.recentBlendsMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.recentBlendsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolStripMenuItem,
            this.toolStripSeparator3,
            this.miEmptyPH});
            this.recentBlendsMenu.Name = "recentBlendsMenu";
            this.recentBlendsMenu.OwnerItem = this.miOpenRecent;
            this.recentBlendsMenu.Size = new System.Drawing.Size(113, 62);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Image = global::BlenderRenderController.Properties.Resources.CleanData_16x;
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(112, 26);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.ToolTipText = "Clear recent blends";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearRecentProjectsListToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(109, 6);
            // 
            // miEmptyPH
            // 
            this.miEmptyPH.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.miEmptyPH.Enabled = false;
            this.miEmptyPH.Name = "miEmptyPH";
            this.miEmptyPH.Size = new System.Drawing.Size(112, 26);
            this.miEmptyPH.Text = "Empty";
            // 
            // openRecentsTSButton
            // 
            this.openRecentsTSButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openRecentsTSButton.DropDown = this.recentBlendsMenu;
            this.openRecentsTSButton.Image = global::BlenderRenderController.Properties.Resources.Time_16x;
            this.openRecentsTSButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openRecentsTSButton.Margin = new System.Windows.Forms.Padding(3, 1, 3, 2);
            this.openRecentsTSButton.Name = "openRecentsTSButton";
            this.openRecentsTSButton.Size = new System.Drawing.Size(33, 24);
            this.openRecentsTSButton.Text = "Open Recent";
            // 
            // miOpenRecent
            // 
            this.miOpenRecent.DropDown = this.recentBlendsMenu;
            this.miOpenRecent.Image = global::BlenderRenderController.Properties.Resources.Time_16x;
            this.miOpenRecent.Name = "miOpenRecent";
            this.miOpenRecent.Size = new System.Drawing.Size(151, 22);
            this.miOpenRecent.Text = "Open Re&cent";
            this.miOpenRecent.ToolTipText = "Open Recent";
            // 
            // blendFileLabel
            // 
            this.blendFileLabel.AutoSize = true;
            this.blendFileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Underline);
            this.blendFileLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.blendFileLabel.Location = new System.Drawing.Point(9, 62);
            this.blendFileLabel.Name = "blendFileLabel";
            this.blendFileLabel.Size = new System.Drawing.Size(94, 20);
            this.blendFileLabel.TabIndex = 25;
            this.blendFileLabel.Text = "Project info";
            // 
            // outputFolderLabel
            // 
            this.outputFolderLabel.AutoSize = true;
            this.outputFolderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Underline);
            this.outputFolderLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.outputFolderLabel.Location = new System.Drawing.Point(9, 343);
            this.outputFolderLabel.Name = "outputFolderLabel";
            this.outputFolderLabel.Size = new System.Drawing.Size(111, 20);
            this.outputFolderLabel.TabIndex = 25;
            this.outputFolderLabel.Text = "Output Folder";
            // 
            // optionsLabel
            // 
            this.optionsLabel.AutoSize = true;
            this.optionsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Underline);
            this.optionsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.optionsLabel.Location = new System.Drawing.Point(9, 160);
            this.optionsLabel.Name = "optionsLabel";
            this.optionsLabel.Size = new System.Drawing.Size(67, 20);
            this.optionsLabel.TabIndex = 25;
            this.optionsLabel.Text = "Options";
            // 
            // openOutputFolderButton
            // 
            this.openOutputFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.openOutputFolderButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openOutputFolderButton.Image = global::BlenderRenderController.Properties.Resources.FolderOpen_16x;
            this.openOutputFolderButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.openOutputFolderButton.Location = new System.Drawing.Point(369, 27);
            this.openOutputFolderButton.Name = "openOutputFolderButton";
            this.openOutputFolderButton.Padding = new System.Windows.Forms.Padding(8, 0, 5, 0);
            this.openOutputFolderButton.Size = new System.Drawing.Size(80, 25);
            this.openOutputFolderButton.TabIndex = 16;
            this.openOutputFolderButton.Tag = "";
            this.openOutputFolderButton.Text = "Open";
            this.openOutputFolderButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.openOutputFolderButton.UseVisualStyleBackColor = true;
            this.openOutputFolderButton.Click += new System.EventHandler(this.openOutputFolderButton_Click);
            // 
            // outputFolderBrowseButton
            // 
            this.outputFolderBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.outputFolderBrowseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputFolderBrowseButton.Image = global::BlenderRenderController.Properties.Resources.FolderOpen_16x;
            this.outputFolderBrowseButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.outputFolderBrowseButton.Location = new System.Drawing.Point(268, 27);
            this.outputFolderBrowseButton.Name = "outputFolderBrowseButton";
            this.outputFolderBrowseButton.Padding = new System.Windows.Forms.Padding(8, 0, 7, 1);
            this.outputFolderBrowseButton.Size = new System.Drawing.Size(95, 25);
            this.outputFolderBrowseButton.TabIndex = 12;
            this.outputFolderBrowseButton.Tag = "";
            this.outputFolderBrowseButton.Text = "Change";
            this.outputFolderBrowseButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.outputFolderBrowseButton.UseVisualStyleBackColor = true;
            this.outputFolderBrowseButton.Click += new System.EventHandler(this.outputFolderBrowseButton_Click);
            // 
            // openBlendDialog
            // 
            this.openBlendDialog.Filter = "Blend|*.blend";
            // 
            // flpStartEnd
            // 
            this.flpStartEnd.Controls.Add(this.checkAutoFrame);
            this.flpStartEnd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpStartEnd.Location = new System.Drawing.Point(0, 61);
            this.flpStartEnd.Name = "flpStartEnd";
            this.flpStartEnd.Size = new System.Drawing.Size(223, 31);
            this.flpStartEnd.TabIndex = 39;
            // 
            // blendNameLabel
            // 
            this.blendNameLabel.AutoSize = true;
            this.blendNameLabel.DataBindings.Add(new System.Windows.Forms.Binding("Visible", this.projectBindingSrc, "ProjectLoaded", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.blendNameLabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.projectBindingSrc, "Header", true));
            this.blendNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold);
            this.blendNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.blendNameLabel.Location = new System.Drawing.Point(122, 62);
            this.blendNameLabel.Name = "blendNameLabel";
            this.blendNameLabel.Size = new System.Drawing.Size(105, 20);
            this.blendNameLabel.TabIndex = 40;
            this.blendNameLabel.Text = "blend name";
            this.blendNameLabel.Visible = false;
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileTSButton,
            this.openRecentsTSButton,
            this.reloadTSButton,
            this.toolStripSeparator1,
            this.aboutToolStripButton});
            this.toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new System.Windows.Forms.Padding(10, 0, 1, 0);
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(472, 27);
            this.toolStrip.TabIndex = 41;
            this.toolStrip.Text = "menuToolStrip";
            // 
            // openFileTSButton
            // 
            this.openFileTSButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openFileTSButton.Image = global::BlenderRenderController.Properties.Resources.blender_icon;
            this.openFileTSButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openFileTSButton.Name = "openFileTSButton";
            this.openFileTSButton.Size = new System.Drawing.Size(24, 24);
            this.openFileTSButton.Tag = "load_blend";
            this.openFileTSButton.Text = "&Open";
            this.openFileTSButton.ToolTipText = "Open blend file";
            this.openFileTSButton.Click += new System.EventHandler(this.OpenBlend_Click);
            // 
            // reloadTSButton
            // 
            this.reloadTSButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.reloadTSButton.Image = global::BlenderRenderController.Properties.Resources.Refresh_grey_16x;
            this.reloadTSButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.reloadTSButton.Name = "reloadTSButton";
            this.reloadTSButton.Size = new System.Drawing.Size(24, 24);
            this.reloadTSButton.Text = "Reload";
            this.reloadTSButton.ToolTipText = "Reload current blend file";
            this.reloadTSButton.Click += new System.EventHandler(this.ReloadBlend_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // aboutToolStripButton
            // 
            this.aboutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.aboutToolStripButton.Image = global::BlenderRenderController.Properties.Resources.UIAboutBox_16x;
            this.aboutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.aboutToolStripButton.Name = "aboutToolStripButton";
            this.aboutToolStripButton.Size = new System.Drawing.Size(24, 24);
            this.aboutToolStripButton.Text = "About";
            this.aboutToolStripButton.Click += new System.EventHandler(this.AboutBRC_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // panelFrameRange
            // 
            this.panelFrameRange.Controls.Add(this.flpStartEnd);
            this.panelFrameRange.Controls.Add(this.totalStartNumericUpDown);
            this.panelFrameRange.Controls.Add(this.totalEndNumericUpDown);
            this.panelFrameRange.Controls.Add(this.startFrameLabel);
            this.panelFrameRange.Controls.Add(this.totalFrameCountLabel);
            this.panelFrameRange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFrameRange.Location = new System.Drawing.Point(3, 3);
            this.panelFrameRange.Name = "panelFrameRange";
            this.panelFrameRange.Size = new System.Drawing.Size(223, 92);
            this.panelFrameRange.TabIndex = 43;
            // 
            // flpChunkMode
            // 
            this.flpChunkMode.Controls.Add(this.checkChunkSize);
            this.flpChunkMode.Controls.Add(this.checkMaxProcs);
            this.flpChunkMode.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpChunkMode.Location = new System.Drawing.Point(0, 61);
            this.flpChunkMode.Name = "flpChunkMode";
            this.flpChunkMode.Size = new System.Drawing.Size(210, 31);
            this.flpChunkMode.TabIndex = 44;
            // 
            // panelChunkSize
            // 
            this.panelChunkSize.Controls.Add(this.processCountLabel);
            this.panelChunkSize.Controls.Add(this.processCountNumericUpDown);
            this.panelChunkSize.Controls.Add(this.chunkLengthNumericUpDown);
            this.panelChunkSize.Controls.Add(this.flpChunkMode);
            this.panelChunkSize.Controls.Add(this.chunkLengthLabel);
            this.panelChunkSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChunkSize.Location = new System.Drawing.Point(232, 3);
            this.panelChunkSize.Name = "panelChunkSize";
            this.panelChunkSize.Size = new System.Drawing.Size(210, 92);
            this.panelChunkSize.TabIndex = 45;
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip.Size = new System.Drawing.Size(472, 24);
            this.menuStrip.TabIndex = 50;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miOpenFile,
            this.miOpenRecent,
            this.miReloadCurrent,
            this.unloadToolStripMenuItem,
            this.toolStripSeparator2,
            this.miSettings,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // miOpenFile
            // 
            this.miOpenFile.Image = global::BlenderRenderController.Properties.Resources.blender_icon;
            this.miOpenFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.miOpenFile.Name = "miOpenFile";
            this.miOpenFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.miOpenFile.Size = new System.Drawing.Size(151, 22);
            this.miOpenFile.Tag = "proj_new";
            this.miOpenFile.Text = "&Open";
            this.miOpenFile.ToolTipText = "Open blend file";
            this.miOpenFile.Click += new System.EventHandler(this.OpenBlend_Click);
            // 
            // miReloadCurrent
            // 
            this.miReloadCurrent.Image = global::BlenderRenderController.Properties.Resources.Refresh_grey_16x;
            this.miReloadCurrent.Name = "miReloadCurrent";
            this.miReloadCurrent.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.miReloadCurrent.Size = new System.Drawing.Size(151, 22);
            this.miReloadCurrent.Text = "&Reload";
            this.miReloadCurrent.ToolTipText = "Reload current blend file";
            this.miReloadCurrent.Click += new System.EventHandler(this.ReloadBlend_Click);
            // 
            // unloadToolStripMenuItem
            // 
            this.unloadToolStripMenuItem.Name = "unloadToolStripMenuItem";
            this.unloadToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.unloadToolStripMenuItem.Text = "&Unload";
            this.unloadToolStripMenuItem.Click += new System.EventHandler(this.UnloadCurrent_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(148, 6);
            // 
            // miSettings
            // 
            this.miSettings.Image = global::BlenderRenderController.Properties.Resources.settings_icon;
            this.miSettings.Name = "miSettings";
            this.miSettings.Size = new System.Drawing.Size(151, 22);
            this.miSettings.Text = "&Settings";
            this.miSettings.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(148, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRenderMixdown,
            this.miJoinChunks});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // miRenderMixdown
            // 
            this.miRenderMixdown.Image = global::BlenderRenderController.Properties.Resources.SoundFile_16x;
            this.miRenderMixdown.Name = "miRenderMixdown";
            this.miRenderMixdown.Size = new System.Drawing.Size(163, 22);
            this.miRenderMixdown.Text = "Render Mixdown";
            this.miRenderMixdown.Click += new System.EventHandler(this.mixDownButton_Click);
            // 
            // miJoinChunks
            // 
            this.miJoinChunks.Image = global::BlenderRenderController.Properties.Resources.Join_16x;
            this.miJoinChunks.Name = "miJoinChunks";
            this.miJoinChunks.Size = new System.Drawing.Size(163, 22);
            this.miJoinChunks.Text = "Join Chunks";
            this.miJoinChunks.Click += new System.EventHandler(this.concatenatePartsButton_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miGithub,
            this.miReportBug,
            this.miDonate,
            this.toolStripSeparator7,
            this.miAbout});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // miGithub
            // 
            this.miGithub.Image = global::BlenderRenderController.Properties.Resources.github_logo;
            this.miGithub.Name = "miGithub";
            this.miGithub.Size = new System.Drawing.Size(142, 22);
            this.miGithub.Text = "&Github";
            this.miGithub.Click += new System.EventHandler(this.miGithub_Click);
            // 
            // miReportBug
            // 
            this.miReportBug.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline);
            this.miReportBug.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.miReportBug.Image = global::BlenderRenderController.Properties.Resources.bug_icon;
            this.miReportBug.Name = "miReportBug";
            this.miReportBug.Size = new System.Drawing.Size(142, 22);
            this.miReportBug.Text = "Report a &Bug";
            this.miReportBug.Click += new System.EventHandler(this.toolStripMenuItemBug_Click);
            // 
            // miDonate
            // 
            this.miDonate.Image = ((System.Drawing.Image)(resources.GetObject("miDonate.Image")));
            this.miDonate.Name = "miDonate";
            this.miDonate.Size = new System.Drawing.Size(142, 22);
            this.miDonate.Text = "&Donate";
            this.miDonate.ToolTipText = "Feeling specially awesome? Donate!";
            this.miDonate.Click += new System.EventHandler(this.donateButton_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(139, 6);
            // 
            // miAbout
            // 
            this.miAbout.Image = global::BlenderRenderController.Properties.Resources.UIAboutBox_16x;
            this.miAbout.Name = "miAbout";
            this.miAbout.Size = new System.Drawing.Size(142, 22);
            this.miAbout.Text = "&About...";
            this.miAbout.Click += new System.EventHandler(this.AboutBRC_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusMessage,
            this.statusETR});
            this.statusStrip.Location = new System.Drawing.Point(0, 514);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(472, 22);
            this.statusStrip.TabIndex = 51;
            // 
            // statusMessage
            // 
            this.statusMessage.Name = "statusMessage";
            this.statusMessage.Size = new System.Drawing.Size(382, 17);
            this.statusMessage.Spring = true;
            this.statusMessage.Text = "Some Status message";
            this.statusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusETR
            // 
            this.statusETR.Name = "statusETR";
            this.statusETR.Size = new System.Drawing.Size(75, 17);
            this.statusETR.Text = "ETR: 00:00:00";
            // 
            // frOutputFolder
            // 
            this.frOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frOutputFolder.Controls.Add(this.outputFolderTextBox);
            this.frOutputFolder.Controls.Add(this.openOutputFolderButton);
            this.frOutputFolder.Controls.Add(this.outputFolderBrowseButton);
            this.frOutputFolder.Location = new System.Drawing.Point(12, 368);
            this.frOutputFolder.Name = "frOutputFolder";
            this.frOutputFolder.Size = new System.Drawing.Size(449, 55);
            this.frOutputFolder.TabIndex = 53;
            // 
            // infoBox
            // 
            this.infoBox.AutoScroll = true;
            this.infoBox.BackColor = System.Drawing.SystemColors.Info;
            this.infoBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.infoBox.Controls.Add(this.infoActiveScene);
            this.infoBox.Controls.Add(this.infoDuration);
            this.infoBox.Controls.Add(this.infoFPS);
            this.infoBox.Controls.Add(this.infoResolution);
            this.infoBox.Location = new System.Drawing.Point(15, 87);
            this.infoBox.Name = "infoBox";
            this.infoBox.Padding = new System.Windows.Forms.Padding(5);
            this.infoBox.Size = new System.Drawing.Size(446, 64);
            this.infoBox.TabIndex = 54;
            this.infoBox.WrapContents = false;
            // 
            // frOptions
            // 
            this.frOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frOptions.ColumnCount = 2;
            this.frOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.48742F));
            this.frOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.51258F));
            this.frOptions.Controls.Add(this.panelChunkSize, 1, 0);
            this.frOptions.Controls.Add(this.panelFrameRange, 0, 0);
            this.frOptions.Controls.Add(this.panel2, 1, 1);
            this.frOptions.Controls.Add(this.panel3, 0, 1);
            this.frOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.frOptions.Location = new System.Drawing.Point(15, 183);
            this.frOptions.Name = "frOptions";
            this.frOptions.RowCount = 2;
            this.frOptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 68.18182F));
            this.frOptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.81818F));
            this.frOptions.Size = new System.Drawing.Size(445, 145);
            this.frOptions.TabIndex = 55;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.cbRenderer);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(232, 101);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(210, 41);
            this.panel2.TabIndex = 46;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 15);
            this.label1.TabIndex = 52;
            this.label1.Text = "Renderer";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.cbAfterRenderAction);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 101);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(223, 41);
            this.panel3.TabIndex = 47;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 15);
            this.label3.TabIndex = 50;
            this.label3.Text = "Joining action";
            // 
            // BrcForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(472, 536);
            this.Controls.Add(this.frOptions);
            this.Controls.Add(this.infoBox);
            this.Controls.Add(this.frOutputFolder);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.blendNameLabel);
            this.Controls.Add(this.renderInfoLabel);
            this.Controls.Add(this.blendFileLabel);
            this.Controls.Add(this.btnStartWork);
            this.Controls.Add(this.btnStopWork);
            this.Controls.Add(this.optionsLabel);
            this.Controls.Add(this.outputFolderLabel);
            this.Controls.Add(this.renderProgressBar);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.projectBindingSrc, "Title", true));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(498, 596);
            this.MinimumSize = new System.Drawing.Size(485, 295);
            this.Name = "BrcForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blender Render Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BrcForm_FormClosing);
            this.Load += new System.EventHandler(this.BrcForm_Load);
            this.Shown += new System.EventHandler(this.BrcForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.totalStartNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectBindingSrc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.totalEndNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.processCountNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chunkLengthNumericUpDown)).EndInit();
            this.recentBlendsMenu.ResumeLayout(false);
            this.flpStartEnd.ResumeLayout(false);
            this.flpStartEnd.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.panelFrameRange.ResumeLayout(false);
            this.panelFrameRange.PerformLayout();
            this.flpChunkMode.ResumeLayout(false);
            this.flpChunkMode.PerformLayout();
            this.panelChunkSize.ResumeLayout(false);
            this.panelChunkSize.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.frOutputFolder.ResumeLayout(false);
            this.frOutputFolder.PerformLayout();
            this.infoBox.ResumeLayout(false);
            this.frOptions.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ProgressBar renderProgressBar;
        private System.Windows.Forms.NumericUpDown totalStartNumericUpDown;
        private System.Windows.Forms.Label startFrameLabel;
        private System.Windows.Forms.Button outputFolderBrowseButton;
        private System.Windows.Forms.TextBox outputFolderTextBox;
        private System.Windows.Forms.Label totalFrameCountLabel;
        private System.Windows.Forms.NumericUpDown totalEndNumericUpDown;
        private System.Windows.Forms.NumericUpDown processCountNumericUpDown;
        private System.Windows.Forms.Label processCountLabel;
		private System.Windows.Forms.Button btnStartWork;
        private System.Windows.Forms.Button btnStopWork;
        private System.Windows.Forms.ToolTip toolTipInfo;
        private System.Windows.Forms.Label blendFileLabel;
        private System.Windows.Forms.NumericUpDown chunkLengthNumericUpDown;
        private System.Windows.Forms.Label chunkLengthLabel;
        private System.Windows.Forms.Label outputFolderLabel;
        private System.Windows.Forms.Label optionsLabel;
        private System.Windows.Forms.Button openOutputFolderButton;
        private System.Windows.Forms.Label renderInfoLabel;
        private System.Windows.Forms.ContextMenuStrip recentBlendsMenu;
        private System.Windows.Forms.OpenFileDialog openBlendDialog;
        private System.Windows.Forms.FlowLayoutPanel flpStartEnd;
        private System.Windows.Forms.Label blendNameLabel;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Panel panelFrameRange;
        private System.Windows.Forms.FlowLayoutPanel flpChunkMode;
        private System.Windows.Forms.Panel panelChunkSize;
        private System.Windows.Forms.ComboBox cbRenderer;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem miAbout;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miOpenFile;
        private System.Windows.Forms.ToolStripMenuItem miOpenRecent;
        private System.Windows.Forms.ToolStripMenuItem miReloadCurrent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem miSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miRenderMixdown;
        private System.Windows.Forms.ToolStripMenuItem miJoinChunks;
        private System.Windows.Forms.ToolStripMenuItem miGithub;
        private System.Windows.Forms.ToolStripMenuItem miReportBug;
        private System.Windows.Forms.ToolStripMenuItem miDonate;
        private System.Windows.Forms.ToolStripButton openFileTSButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton aboutToolStripButton;
        private System.Windows.Forms.ToolStripDropDownButton openRecentsTSButton;
        private System.Windows.Forms.ToolStripButton reloadTSButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem miEmptyPH;
        private System.Windows.Forms.ToolStripStatusLabel statusMessage;
        private System.Windows.Forms.ToolStripStatusLabel statusETR;
        private System.Windows.Forms.FlowLayoutPanel infoBox;
        private System.Windows.Forms.Panel frOutputFolder;
        private InfoBoxItem infoActiveScene;
        private InfoBoxItem infoDuration;
        private InfoBoxItem infoResolution;
        private InfoBoxItem infoFPS;
        private System.Windows.Forms.TableLayoutPanel frOptions;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbAfterRenderAction;
        private System.Windows.Forms.ToolStripMenuItem unloadToolStripMenuItem;
        private System.Windows.Forms.BindingSource projectBindingSrc;
        private System.Windows.Forms.CheckBox checkChunkSize;
        private System.Windows.Forms.CheckBox checkMaxProcs;
        private System.Windows.Forms.CheckBox checkAutoFrame;
    }
}

