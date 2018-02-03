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
        Sound _mbSound;
        Icon _mbIcon;

        protected DetailedMessageBox()
        {
            InitializeComponent();
            detailsCGB.Resize += DetailsCGB_Resize;
        }


        public DetailedMessageBox(string message, string title, IEnumerable<string> contents, 
                        MessageBoxButtons bnts = MessageBoxButtons.OK, 
                        MessageBoxIcon icon = MessageBoxIcon.Error)
            : this()
        {
            _mbIcon = GetMBIcon(icon);
            _mbSound = GetMBSound(icon);

            msgBoxPic.Image = _mbIcon.ToBitmap();
            msgBoxLabel.Text = message;
            this.Text = title;
            detailsTextBox.Lines = contents.ToArray();
            SetupButtons(bnts);

        }


        public DetailedMessageBox(string message, string title, string contents, 
                        MessageBoxButtons bnts = MessageBoxButtons.OK, 
                        MessageBoxIcon icon = MessageBoxIcon.Error)
            :this(message, title, contents.Split('\n'), bnts, icon)
        {
        }

        private void Bnt_Click(object sender, EventArgs e)
        {
            var b = sender as Button;
            this.DialogResult = (DialogResult)b.Tag;
            this.Close();
        }

        private void MsgBox_Shown(object sender, EventArgs e)
        {
            // play sound
            if (_mbSound != null)
            {
                _mbSound.Play();
            }
        }

        private void DetailsCGB_Resize(object sender, EventArgs e)
        {
            var cgb = sender as CollapsibleGroupBox;
            Height += cgb.HeightRequest;
        }


        private void SetupButtons(MessageBoxButtons bnts)
        {
            // TODO: Localize text
            switch (bnts)
            {
                case MessageBoxButtons.OK:
                    SetupButtons(null, null, DialogResult.OK);
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    SetupButtons(DialogResult.Abort, DialogResult.Retry, DialogResult.Ignore);
                    break;
                case MessageBoxButtons.YesNo:
                    SetupButtons(null, DialogResult.Yes, DialogResult.No);
                    break;
                case MessageBoxButtons.OKCancel:
                    SetupButtons(null, DialogResult.OK, DialogResult.Cancel);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    SetupButtons(DialogResult.Yes, DialogResult.No, DialogResult.Cancel);
                    break;
                case MessageBoxButtons.RetryCancel:
                    SetupButtons(null, DialogResult.Retry, DialogResult.Cancel);
                    break;
                default:
                    throw new Exception("Unknown buttons");
            }
        }

        private void SetupButtons(DialogResult? left, DialogResult? middle, DialogResult? right)
        {
            BtnLeft.Visible = left.HasValue;
            BtnLeft.Tag = left;
            if (left.HasValue)
            {
                BtnLeft.Text = left.Value.ToString();
            }

            BtnMiddle.Visible = middle.HasValue;
            BtnMiddle.Tag = middle;
            if (middle.HasValue)
            {
                BtnMiddle.Text = middle.Value.ToString();
            }

            BtnRight.Visible = right.HasValue;
            BtnRight.Tag = right;
            if (right.HasValue)
            {
                BtnRight.Text = right.Value.ToString();
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

        private Sound GetMBSound(MessageBoxIcon icon)
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
