namespace BlenderRenderController
{
    partial class config
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
            this.browseBlender = new System.Windows.Forms.Button();
            this.browseFFmpeg = new System.Windows.Forms.Button();
            this.layoutPaths = new System.Windows.Forms.TableLayoutPanel();
            this.panelBlenderEXE = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.getFromPATH_ffmpeg = new System.Windows.Forms.CheckBox();
            this.getFromPATH_blender = new System.Windows.Forms.CheckBox();
            this.panelFFmpegEXE = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.saveAllButton = new System.Windows.Forms.Button();
            this.blenderPathBox = new System.Windows.Forms.TextBox();
            this.ffmpegPathBox = new System.Windows.Forms.TextBox();
            this.layoutPaths.SuspendLayout();
            this.panelBlenderEXE.SuspendLayout();
            this.panelFFmpegEXE.SuspendLayout();
            this.SuspendLayout();
            // 
            // browseBlender
            // 
            this.browseBlender.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browseBlender.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.browseBlender.Location = new System.Drawing.Point(380, 27);
            this.browseBlender.Name = "browseBlender";
            this.browseBlender.Size = new System.Drawing.Size(72, 23);
            this.browseBlender.TabIndex = 4;
            this.browseBlender.Text = "Browse";
            this.browseBlender.UseVisualStyleBackColor = true;
            this.browseBlender.Click += new System.EventHandler(this.setBlender_Click);
            // 
            // browseFFmpeg
            // 
            this.browseFFmpeg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browseFFmpeg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.browseFFmpeg.Location = new System.Drawing.Point(380, 81);
            this.browseFFmpeg.Name = "browseFFmpeg";
            this.browseFFmpeg.Size = new System.Drawing.Size(72, 23);
            this.browseFFmpeg.TabIndex = 5;
            this.browseFFmpeg.Text = "Browse";
            this.browseFFmpeg.UseVisualStyleBackColor = true;
            this.browseFFmpeg.Click += new System.EventHandler(this.setFFmpeg_Click);
            // 
            // layoutPaths
            // 
            this.layoutPaths.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.layoutPaths.ColumnCount = 3;
            this.layoutPaths.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.44579F));
            this.layoutPaths.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.layoutPaths.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 87F));
            this.layoutPaths.Controls.Add(this.panelBlenderEXE, 0, 0);
            this.layoutPaths.Controls.Add(this.getFromPATH_ffmpeg, 2, 1);
            this.layoutPaths.Controls.Add(this.getFromPATH_blender, 2, 0);
            this.layoutPaths.Controls.Add(this.browseBlender, 1, 0);
            this.layoutPaths.Controls.Add(this.browseFFmpeg, 1, 1);
            this.layoutPaths.Controls.Add(this.panelFFmpegEXE, 0, 1);
            this.layoutPaths.Location = new System.Drawing.Point(12, 12);
            this.layoutPaths.Name = "layoutPaths";
            this.layoutPaths.RowCount = 2;
            this.layoutPaths.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutPaths.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutPaths.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutPaths.Size = new System.Drawing.Size(542, 107);
            this.layoutPaths.TabIndex = 6;
            // 
            // panelBlenderEXE
            // 
            this.panelBlenderEXE.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBlenderEXE.Controls.Add(this.label1);
            this.panelBlenderEXE.Controls.Add(this.blenderPathBox);
            this.panelBlenderEXE.Location = new System.Drawing.Point(3, 3);
            this.panelBlenderEXE.Name = "panelBlenderEXE";
            this.panelBlenderEXE.Size = new System.Drawing.Size(371, 47);
            this.panelBlenderEXE.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Blender EXE";
            // 
            // getFromPATH_ffmpeg
            // 
            this.getFromPATH_ffmpeg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.getFromPATH_ffmpeg.AutoSize = true;
            this.getFromPATH_ffmpeg.Checked = global::BlenderRenderController.Properties.Settings.Default.usePath_ffmpeg;
            this.getFromPATH_ffmpeg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.getFromPATH_ffmpeg.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::BlenderRenderController.Properties.Settings.Default, "usePath_ffmpeg", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.getFromPATH_ffmpeg.Location = new System.Drawing.Point(458, 87);
            this.getFromPATH_ffmpeg.Name = "getFromPATH_ffmpeg";
            this.getFromPATH_ffmpeg.Size = new System.Drawing.Size(81, 17);
            this.getFromPATH_ffmpeg.TabIndex = 8;
            this.getFromPATH_ffmpeg.Text = "Use PATH";
            this.getFromPATH_ffmpeg.UseVisualStyleBackColor = true;
            this.getFromPATH_ffmpeg.Click += new System.EventHandler(this.getFromPATH_ffmpeg_click);
            // 
            // getFromPATH_blender
            // 
            this.getFromPATH_blender.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.getFromPATH_blender.AutoSize = true;
            this.getFromPATH_blender.Checked = global::BlenderRenderController.Properties.Settings.Default.usePath_blender;
            this.getFromPATH_blender.CheckState = System.Windows.Forms.CheckState.Checked;
            this.getFromPATH_blender.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::BlenderRenderController.Properties.Settings.Default, "usePath_blender", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.getFromPATH_blender.Location = new System.Drawing.Point(458, 33);
            this.getFromPATH_blender.Name = "getFromPATH_blender";
            this.getFromPATH_blender.Size = new System.Drawing.Size(81, 17);
            this.getFromPATH_blender.TabIndex = 7;
            this.getFromPATH_blender.Text = "Use PATH";
            this.getFromPATH_blender.UseVisualStyleBackColor = true;
            this.getFromPATH_blender.Click += new System.EventHandler(this.getFromPATH_blender_click);
            // 
            // panelFFmpegEXE
            // 
            this.panelFFmpegEXE.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFFmpegEXE.Controls.Add(this.label2);
            this.panelFFmpegEXE.Controls.Add(this.ffmpegPathBox);
            this.panelFFmpegEXE.Location = new System.Drawing.Point(3, 56);
            this.panelFFmpegEXE.Name = "panelFFmpegEXE";
            this.panelFFmpegEXE.Size = new System.Drawing.Size(371, 48);
            this.panelFFmpegEXE.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "FFmepg EXE";
            // 
            // saveAllButton
            // 
            this.saveAllButton.Location = new System.Drawing.Point(436, 125);
            this.saveAllButton.Name = "saveAllButton";
            this.saveAllButton.Size = new System.Drawing.Size(115, 38);
            this.saveAllButton.TabIndex = 9;
            this.saveAllButton.Text = "Save settings";
            this.saveAllButton.UseVisualStyleBackColor = true;
            this.saveAllButton.Click += new System.EventHandler(this.saveAll_Click);
            // 
            // blenderPathBox
            // 
            this.blenderPathBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::BlenderRenderController.Properties.Settings.Default, "blender_path", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.blenderPathBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.blenderPathBox.Location = new System.Drawing.Point(0, 27);
            this.blenderPathBox.Name = "blenderPathBox";
            this.blenderPathBox.Size = new System.Drawing.Size(371, 20);
            this.blenderPathBox.TabIndex = 0;
            this.blenderPathBox.Text = global::BlenderRenderController.Properties.Settings.Default.blender_path;
            this.blenderPathBox.TextChanged += new System.EventHandler(this.blenderPathBox_TextChanged);
            // 
            // ffmpegPathBox
            // 
            this.ffmpegPathBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::BlenderRenderController.Properties.Settings.Default, "ffmpeg_path", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ffmpegPathBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ffmpegPathBox.Location = new System.Drawing.Point(0, 28);
            this.ffmpegPathBox.Name = "ffmpegPathBox";
            this.ffmpegPathBox.Size = new System.Drawing.Size(371, 20);
            this.ffmpegPathBox.TabIndex = 1;
            this.ffmpegPathBox.Text = global::BlenderRenderController.Properties.Settings.Default.ffmpeg_path;
            this.ffmpegPathBox.TextChanged += new System.EventHandler(this.ffmpegPathBox_TextChanged);
            // 
            // config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 171);
            this.Controls.Add(this.saveAllButton);
            this.Controls.Add(this.layoutPaths);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.Name = "config";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configs";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.config_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.config_FormClosed);
            this.Load += new System.EventHandler(this.config_Load);
            this.layoutPaths.ResumeLayout(false);
            this.layoutPaths.PerformLayout();
            this.panelBlenderEXE.ResumeLayout(false);
            this.panelBlenderEXE.PerformLayout();
            this.panelFFmpegEXE.ResumeLayout(false);
            this.panelFFmpegEXE.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox blenderPathBox;
        private System.Windows.Forms.TextBox ffmpegPathBox;
        private System.Windows.Forms.Button browseBlender;
        private System.Windows.Forms.Button browseFFmpeg;
        private System.Windows.Forms.TableLayoutPanel layoutPaths;
        private System.Windows.Forms.CheckBox getFromPATH_ffmpeg;
        private System.Windows.Forms.CheckBox getFromPATH_blender;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelBlenderEXE;
        private System.Windows.Forms.Panel panelFFmpegEXE;
        private System.Windows.Forms.Button saveAllButton;
    }
}