namespace BlenderRenderController
{
    partial class SettingsForm
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
            this.blenderPathTextBox = new System.Windows.Forms.TextBox();
            this.settingsBindingSrc = new System.Windows.Forms.BindingSource(this.components);
            this.ffmpegPathTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.blenderLabel = new System.Windows.Forms.Label();
            this.ffmpegLabel = new System.Windows.Forms.Label();
            this.ffmpegChangePathButton = new System.Windows.Forms.Button();
            this.blenderChangePathButton = new System.Windows.Forms.Button();
            this.settingsToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.chkBoxDelChunks = new System.Windows.Forms.CheckBox();
            this.chkBoxShowTooltips = new System.Windows.Forms.CheckBox();
            this.cbLoggingLvl = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.findBlenderDialog = new System.Windows.Forms.OpenFileDialog();
            this.findFFmpegDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.settingsBindingSrc)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // blenderPathTextBox
            // 
            this.blenderPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.blenderPathTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.settingsBindingSrc, "BlenderProgram", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.blenderPathTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blenderPathTextBox.Location = new System.Drawing.Point(6, 29);
            this.blenderPathTextBox.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.blenderPathTextBox.Name = "blenderPathTextBox";
            this.blenderPathTextBox.Size = new System.Drawing.Size(488, 22);
            this.blenderPathTextBox.TabIndex = 0;
            this.blenderPathTextBox.WordWrap = false;
            // 
            // settingsBindingSrc
            // 
            this.settingsBindingSrc.DataSource = typeof(BRClib.ConfigModel);
            // 
            // ffmpegPathTextBox
            // 
            this.ffmpegPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ffmpegPathTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.settingsBindingSrc, "FFmpegProgram", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ffmpegPathTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ffmpegPathTextBox.Location = new System.Drawing.Point(6, 29);
            this.ffmpegPathTextBox.Name = "ffmpegPathTextBox";
            this.ffmpegPathTextBox.Size = new System.Drawing.Size(488, 22);
            this.ffmpegPathTextBox.TabIndex = 2;
            this.ffmpegPathTextBox.WordWrap = false;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okButton.Location = new System.Drawing.Point(438, 260);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(93, 28);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // blenderLabel
            // 
            this.blenderLabel.AutoSize = true;
            this.blenderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blenderLabel.Location = new System.Drawing.Point(3, 5);
            this.blenderLabel.Name = "blenderLabel";
            this.blenderLabel.Size = new System.Drawing.Size(50, 15);
            this.blenderLabel.TabIndex = 29;
            this.blenderLabel.Text = "Blender";
            // 
            // ffmpegLabel
            // 
            this.ffmpegLabel.AutoSize = true;
            this.ffmpegLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ffmpegLabel.Location = new System.Drawing.Point(3, 5);
            this.ffmpegLabel.Name = "ffmpegLabel";
            this.ffmpegLabel.Size = new System.Drawing.Size(53, 15);
            this.ffmpegLabel.TabIndex = 29;
            this.ffmpegLabel.Text = "FFmpeg";
            // 
            // ffmpegChangePathButton
            // 
            this.ffmpegChangePathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ffmpegChangePathButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ffmpegChangePathButton.Image = global::BlenderRenderController.Properties.Resources.FolderOpen_16x;
            this.ffmpegChangePathButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ffmpegChangePathButton.Location = new System.Drawing.Point(383, 57);
            this.ffmpegChangePathButton.Name = "ffmpegChangePathButton";
            this.ffmpegChangePathButton.Size = new System.Drawing.Size(111, 29);
            this.ffmpegChangePathButton.TabIndex = 3;
            this.ffmpegChangePathButton.Text = "   Change";
            this.ffmpegChangePathButton.UseVisualStyleBackColor = true;
            this.ffmpegChangePathButton.Click += new System.EventHandler(this.ffmpegChangePathButton_Click);
            // 
            // blenderChangePathButton
            // 
            this.blenderChangePathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.blenderChangePathButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blenderChangePathButton.Image = global::BlenderRenderController.Properties.Resources.FolderOpen_16x;
            this.blenderChangePathButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.blenderChangePathButton.Location = new System.Drawing.Point(383, 57);
            this.blenderChangePathButton.Name = "blenderChangePathButton";
            this.blenderChangePathButton.Size = new System.Drawing.Size(111, 29);
            this.blenderChangePathButton.TabIndex = 1;
            this.blenderChangePathButton.Text = "   Change";
            this.blenderChangePathButton.UseVisualStyleBackColor = true;
            this.blenderChangePathButton.Click += new System.EventHandler(this.blenderChangePathButton_Click);
            // 
            // chkBoxDelChunks
            // 
            this.chkBoxDelChunks.AutoSize = true;
            this.chkBoxDelChunks.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsBindingSrc, "DeleteChunksFolder", true));
            this.chkBoxDelChunks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxDelChunks.Location = new System.Drawing.Point(3, 28);
            this.chkBoxDelChunks.Name = "chkBoxDelChunks";
            this.chkBoxDelChunks.Size = new System.Drawing.Size(168, 19);
            this.chkBoxDelChunks.TabIndex = 31;
            this.chkBoxDelChunks.Text = "Delete chunks when done";
            this.settingsToolTip.SetToolTip(this.chkBoxDelChunks, "Individual Chunks will be deleted after the joining process is completed.\r\n\r\nObs:" +
        " This setting is ignored if no joining action is chosen.");
            this.chkBoxDelChunks.UseVisualStyleBackColor = true;
            // 
            // chkBoxShowTooltips
            // 
            this.chkBoxShowTooltips.AutoSize = true;
            this.chkBoxShowTooltips.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.settingsBindingSrc, "DisplayToolTips", true));
            this.chkBoxShowTooltips.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBoxShowTooltips.Location = new System.Drawing.Point(3, 3);
            this.chkBoxShowTooltips.Name = "chkBoxShowTooltips";
            this.chkBoxShowTooltips.Size = new System.Drawing.Size(103, 19);
            this.chkBoxShowTooltips.TabIndex = 32;
            this.chkBoxShowTooltips.Text = "Show Tooltips";
            this.chkBoxShowTooltips.UseVisualStyleBackColor = true;
            // 
            // cbLoggingLvl
            // 
            this.cbLoggingLvl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLoggingLvl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLoggingLvl.FormattingEnabled = true;
            this.cbLoggingLvl.Items.AddRange(new object[] {
            "Warnigs and Errors (default)",
            "Detailed",
            "Developer"});
            this.cbLoggingLvl.Location = new System.Drawing.Point(3, 27);
            this.cbLoggingLvl.Name = "cbLoggingLvl";
            this.cbLoggingLvl.Size = new System.Drawing.Size(499, 23);
            this.cbLoggingLvl.TabIndex = 33;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbLoggingLvl);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 143);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(508, 71);
            this.groupBox3.TabIndex = 35;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Logging level";
            // 
            // findBlenderDialog
            // 
            this.findBlenderDialog.Title = "Find ";
            // 
            // findFFmpegDialog
            // 
            this.findFFmpegDialog.Title = "Find ";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.SetColumnSpan(this.tabControl1, 2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(528, 251);
            this.tabControl1.TabIndex = 36;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(520, 223);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Paths";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(520, 223);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Others";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(514, 217);
            this.tableLayoutPanel1.TabIndex = 30;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.blenderLabel);
            this.panel1.Controls.Add(this.blenderPathTextBox);
            this.panel1.Controls.Add(this.blenderChangePathButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(508, 102);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ffmpegPathTextBox);
            this.panel2.Controls.Add(this.ffmpegChangePathButton);
            this.panel2.Controls.Add(this.ffmpegLabel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 111);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(508, 103);
            this.panel2.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 64.7619F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35.23809F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(514, 217);
            this.tableLayoutPanel2.TabIndex = 36;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.chkBoxShowTooltips);
            this.flowLayoutPanel1.Controls.Add(this.chkBoxDelChunks);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(508, 134);
            this.flowLayoutPanel1.TabIndex = 36;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.okButton, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 88.64266F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.35734F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(534, 291);
            this.tableLayoutPanel3.TabIndex = 37;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 291);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 400);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 320);
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsForm_FormClosed);
            this.Load += new System.EventHandler(this.onFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.settingsBindingSrc)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button blenderChangePathButton;
        private System.Windows.Forms.Button ffmpegChangePathButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label blenderLabel;
        private System.Windows.Forms.Label ffmpegLabel;
        private System.Windows.Forms.TextBox blenderPathTextBox;
        private System.Windows.Forms.TextBox ffmpegPathTextBox;
        private System.Windows.Forms.ToolTip settingsToolTip;
        private System.Windows.Forms.CheckBox chkBoxDelChunks;
        private System.Windows.Forms.CheckBox chkBoxShowTooltips;
        private System.Windows.Forms.ComboBox cbLoggingLvl;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.OpenFileDialog findBlenderDialog;
        private System.Windows.Forms.OpenFileDialog findFFmpegDialog;
        private System.Windows.Forms.BindingSource settingsBindingSrc;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    }
}