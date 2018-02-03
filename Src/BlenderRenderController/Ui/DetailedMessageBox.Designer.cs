namespace BlenderRenderController.Ui
{
    partial class DetailedMessageBox
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
            this.BtnLeft = new System.Windows.Forms.Button();
            this.BtnRight = new System.Windows.Forms.Button();
            this.msgBoxLabel = new System.Windows.Forms.Label();
            this.BtnMiddle = new System.Windows.Forms.Button();
            this.msgBoxPic = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.detailsCGB = new BlenderRenderController.Ui.CollapsibleGroupBox();
            this.detailsTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.msgBoxPic)).BeginInit();
            this.panel1.SuspendLayout();
            this.detailsCGB.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnLeft
            // 
            this.BtnLeft.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.BtnLeft.Location = new System.Drawing.Point(251, 12);
            this.BtnLeft.Margin = new System.Windows.Forms.Padding(4);
            this.BtnLeft.Name = "BtnLeft";
            this.BtnLeft.Size = new System.Drawing.Size(100, 37);
            this.BtnLeft.TabIndex = 0;
            this.BtnLeft.Text = "btn left";
            this.BtnLeft.UseVisualStyleBackColor = true;
            this.BtnLeft.Click += new System.EventHandler(this.Bnt_Click);
            // 
            // BtnRight
            // 
            this.BtnRight.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.BtnRight.Location = new System.Drawing.Point(467, 12);
            this.BtnRight.Margin = new System.Windows.Forms.Padding(4);
            this.BtnRight.Name = "BtnRight";
            this.BtnRight.Size = new System.Drawing.Size(100, 37);
            this.BtnRight.TabIndex = 1;
            this.BtnRight.Text = "btn right";
            this.BtnRight.UseVisualStyleBackColor = true;
            this.BtnRight.Click += new System.EventHandler(this.Bnt_Click);
            // 
            // msgBoxLabel
            // 
            this.msgBoxLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.msgBoxLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msgBoxLabel.Location = new System.Drawing.Point(78, 13);
            this.msgBoxLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.msgBoxLabel.MaximumSize = new System.Drawing.Size(533, 246);
            this.msgBoxLabel.MinimumSize = new System.Drawing.Size(427, 49);
            this.msgBoxLabel.Name = "msgBoxLabel";
            this.msgBoxLabel.Size = new System.Drawing.Size(475, 57);
            this.msgBoxLabel.TabIndex = 3;
            this.msgBoxLabel.Text = "Main message...";
            // 
            // BtnMiddle
            // 
            this.BtnMiddle.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.BtnMiddle.Location = new System.Drawing.Point(359, 12);
            this.BtnMiddle.Margin = new System.Windows.Forms.Padding(4);
            this.BtnMiddle.Name = "BtnMiddle";
            this.BtnMiddle.Size = new System.Drawing.Size(100, 37);
            this.BtnMiddle.TabIndex = 4;
            this.BtnMiddle.Text = "btn middle";
            this.BtnMiddle.UseVisualStyleBackColor = true;
            this.BtnMiddle.Click += new System.EventHandler(this.Bnt_Click);
            // 
            // msgBoxPic
            // 
            this.msgBoxPic.BackColor = System.Drawing.Color.White;
            this.msgBoxPic.Location = new System.Drawing.Point(13, 13);
            this.msgBoxPic.Margin = new System.Windows.Forms.Padding(4);
            this.msgBoxPic.MaximumSize = new System.Drawing.Size(67, 62);
            this.msgBoxPic.Name = "msgBoxPic";
            this.msgBoxPic.Size = new System.Drawing.Size(57, 57);
            this.msgBoxPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.msgBoxPic.TabIndex = 5;
            this.msgBoxPic.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.BtnRight);
            this.panel1.Controls.Add(this.BtnLeft);
            this.panel1.Controls.Add(this.BtnMiddle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 111);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(583, 62);
            this.panel1.TabIndex = 7;
            // 
            // detailsCGB
            // 
            this.detailsCGB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.detailsCGB.Controls.Add(this.detailsTextBox);
            this.detailsCGB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.detailsCGB.FullHeight = 220;
            this.detailsCGB.IsCollapsed = true;
            this.detailsCGB.Location = new System.Drawing.Point(13, 83);
            this.detailsCGB.Margin = new System.Windows.Forms.Padding(4);
            this.detailsCGB.Name = "detailsCGB";
            this.detailsCGB.Padding = new System.Windows.Forms.Padding(9);
            this.detailsCGB.Size = new System.Drawing.Size(554, 20);
            this.detailsCGB.TabIndex = 8;
            this.detailsCGB.TabStop = false;
            this.detailsCGB.Text = "Details";
            // 
            // detailsTextBox
            // 
            this.detailsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.detailsTextBox.Location = new System.Drawing.Point(9, 25);
            this.detailsTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.detailsTextBox.Multiline = true;
            this.detailsTextBox.Name = "detailsTextBox";
            this.detailsTextBox.ReadOnly = true;
            this.detailsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.detailsTextBox.Size = new System.Drawing.Size(536, 186);
            this.detailsTextBox.TabIndex = 2;
            this.detailsTextBox.TabStop = false;
            this.detailsTextBox.Text = "large bodies of text goes here!";
            // 
            // DetailedMessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(583, 173);
            this.Controls.Add(this.detailsCGB);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.msgBoxPic);
            this.Controls.Add(this.msgBoxLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DetailedMessageBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Title";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.MsgBox_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.msgBoxPic)).EndInit();
            this.panel1.ResumeLayout(false);
            this.detailsCGB.ResumeLayout(false);
            this.detailsCGB.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button BtnLeft;
        public System.Windows.Forms.Button BtnRight;
        public System.Windows.Forms.TextBox detailsTextBox;
        public System.Windows.Forms.Button BtnMiddle;
        private System.Windows.Forms.PictureBox msgBoxPic;
        private System.Windows.Forms.Panel panel1;
        private CollapsibleGroupBox detailsCGB;
        private System.Windows.Forms.Label msgBoxLabel;
    }
}