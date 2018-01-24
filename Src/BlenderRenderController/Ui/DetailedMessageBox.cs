using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlenderRenderController.Ui
{
    using Sound = System.Media.SystemSound;

    public partial class DetailedMessageBox : Form
    {
        Sound _windowSound;
        Icon _mbIcon;

        protected DetailedMessageBox()
        {
            InitializeComponent();
            detailsCGB.Resize += DetailsCGB_Resize;
        }


        public DetailedMessageBox(string label, string title, IEnumerable<string> contents, 
                        MessageBoxButtons bnts = MessageBoxButtons.OK, 
                        MessageBoxIcon icon = MessageBoxIcon.Error)
            : this()
        {
            //msgBoxPic.Image = GetIconBitmap(icon);
            _mbIcon = GetMBIcon(icon);
            msgBoxPic.Image = _mbIcon.ToBitmap();
            _windowSound = GetWindowSound(icon);

            msgBoxLabel.Text = label;
            this.Text = title;
            detailsTextBox.Lines = contents.ToArray();
            DefineButtons(bnts);

        }


        public DetailedMessageBox(string message, string title, string contents, 
                        MessageBoxButtons bnts = MessageBoxButtons.OK, 
                        MessageBoxIcon icon = MessageBoxIcon.Error)
            :this(message, title, contents.Split('\n'), bnts)
        {
        }

        private void Bnt_Click(object sender, EventArgs e)
        {
            var b = (sender as Button);
            this.DialogResult = (DialogResult)b.Tag;
            this.Close();
        }

        private void MsgBox_Shown(object sender, EventArgs e)
        {
            // play sound
            if (_windowSound != null)
            {
                _windowSound.Play();
            }
        }

        private void DetailsCGB_Resize(object sender, EventArgs e)
        {
            var cgb = sender as CollapsibleGroupBox;
            var heightRequest = cgb.FullHeight - cgb.CollapsedHeight;

            if (cgb.IsCollapsed)
            {
                Height -= heightRequest;
            }
            else
            {
                Height += heightRequest;
            }
        }


        private void DefineButtons(MessageBoxButtons bnts)
        {
            // TODO: Localize text
            switch (bnts)
            {
                case MessageBoxButtons.OK:
                    BtnLeft.Visible =
                    BtnRight.Visible = false;
                    BtnMiddle.Text = "OK";

                    BtnMiddle.Tag = DialogResult.OK;
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    BtnLeft.Text = "Retry";
                    BtnMiddle.Text = "Abort";
                    BtnRight.Text = "Ignore";

                    BtnLeft.Tag = DialogResult.Retry;
                    BtnMiddle.Tag = DialogResult.Abort;
                    BtnRight.Tag = DialogResult.Ignore;
                    break;
                case MessageBoxButtons.YesNo:
                    BtnLeft.Text = "Yes";
                    BtnRight.Text = "No";

                    BtnLeft.Tag = DialogResult.Yes;
                    BtnRight.Tag = DialogResult.No;
                    break;
                case MessageBoxButtons.OKCancel:
                    BtnMiddle.Visible = false;

                    BtnLeft.Text = "OK";
                    BtnRight.Text = "Cancel";

                    BtnLeft.Tag = DialogResult.OK;
                    BtnRight.Tag = DialogResult.Cancel;
                    break;
                case MessageBoxButtons.YesNoCancel:
                    BtnLeft.Text = "Yes";
                    BtnMiddle.Text = "No";
                    BtnRight.Text = "Cancel";

                    BtnLeft.Tag = DialogResult.Yes;
                    BtnMiddle.Tag = DialogResult.No;
                    BtnRight.Tag = DialogResult.Cancel;
                    break;
                case MessageBoxButtons.RetryCancel:
                    BtnMiddle.Visible = false;

                    BtnLeft.Text = "Retry";
                    BtnRight.Text = "Cancel";

                    BtnLeft.Tag = DialogResult.Retry;
                    BtnRight.Tag = DialogResult.Cancel;
                    break;
                default:
                    throw new Exception("Unknown buttons");
            }
        }

        private Icon GetMBIcon(MessageBoxIcon icon)
        {
            Icon result;

            switch (icon)
            {
                case MessageBoxIcon.Hand: // 16 - Stop, Error
                    result = SystemIcons.Hand;
                    break;
                case MessageBoxIcon.Question:
                    result = SystemIcons.Question;
                    break;
                case MessageBoxIcon.Warning: // 48 - Exclamation
                    result = SystemIcons.Warning;
                    break;
                case MessageBoxIcon.Information: // 64 - Asterisk
                    result = SystemIcons.Information;
                    break;
                case MessageBoxIcon.None:
                default:
                    result = null;
                    break;
            }

            return result;
        }

        private Sound GetWindowSound(MessageBoxIcon icon)
        {
            Sound result;

            switch (icon)
            {
                case MessageBoxIcon.Hand: // 16 - Stop, Error
                    result = System.Media.SystemSounds.Hand;
                    break;
                case MessageBoxIcon.Question:
                    result =  System.Media.SystemSounds.Question;
                    break;
                case MessageBoxIcon.Warning: // 48 - Exclamation
                    result =  System.Media.SystemSounds.Exclamation;
                    break;
                case MessageBoxIcon.Information: // 64 - Asterisk
                    result =  System.Media.SystemSounds.Asterisk;
                    break;
                case MessageBoxIcon.None:
                default:
                    result = null;
                    break;
            }

            return result;
        }

    }
}
