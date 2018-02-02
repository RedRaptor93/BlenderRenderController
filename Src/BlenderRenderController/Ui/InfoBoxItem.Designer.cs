namespace BlenderRenderController.Ui
{
    partial class InfoBoxItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.titleLabel = new System.Windows.Forms.Label();
            this.valueLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(35, 18);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Title";
            // 
            // valueLabel
            // 
            this.valueLabel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.valueLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.valueLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.valueLabel.Location = new System.Drawing.Point(0, 20);
            this.valueLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.valueLabel.Name = "valueLabel";
            this.valueLabel.Size = new System.Drawing.Size(104, 24);
            this.valueLabel.TabIndex = 1;
            this.valueLabel.Text = "value";
            this.valueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // InfoBoxItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.valueLabel);
            this.Controls.Add(this.titleLabel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "InfoBoxItem";
            this.Size = new System.Drawing.Size(104, 44);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label valueLabel;
    }
}
