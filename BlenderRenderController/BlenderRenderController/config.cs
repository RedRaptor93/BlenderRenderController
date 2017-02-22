using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlenderRenderController.Properties;
using System.IO;

namespace BlenderRenderController
{
    public partial class config : Form
    {
        Settings set = Settings.Default;

        // saves before val of settings
        string vbBlender = Settings.Default.blender_path;
        string vbFFmpeg = Settings.Default.ffmpeg_path;
        string vbSegLen = Settings.Default.segment_len.ToString();

        public config()
        {
            InitializeComponent();
            //string execPath = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            string execPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        }

        private void config_Load(object sender, EventArgs e)
        {
            // this mess of code checks for the path values and sets "USE PATH" checkbox on load
            // works, but I want to use App.config
            if ((set.blender_path == null) || (set.blender_path == "") || (set.blender_path == set.def_blender))
            {
                getFromPATH_blender.Checked = true;
                blenderPathBox.Enabled = false;
            }
            else
            {
                getFromPATH_blender.Checked = false;
                blenderPathBox.Enabled = true;

            }
            if ((set.ffmpeg_path == null) || (set.ffmpeg_path == "") || (set.ffmpeg_path == set.def_ffmpeg))
            {
                getFromPATH_ffmpeg.Checked = true;
                blenderPathBox.Enabled = false;

            }
            else
            {
                getFromPATH_ffmpeg.Checked = false;
                blenderPathBox.Enabled = true;

            }

            pathToggle();
            //set.Save();
        }


        private void setBlender_Click(object sender, EventArgs e)
        {

            var blenderExecBrowse = new OpenFileDialog();
            blenderExecBrowse.Filter = "Blender Executable|blender.exe";

            var result = blenderExecBrowse.ShowDialog();

            if (result == DialogResult.OK)
            {
                getFromPATH_blender.Checked = false;
                set.blender_path = blenderExecBrowse.FileName;
                blenderPathBox.Text = set.blender_path;
                pathToggle();
            }
        }


        private void setFFmpeg_Click(object sender, EventArgs e)
        {
            var ffmpegExecBrowse = new OpenFileDialog();
            ffmpegExecBrowse.Filter = "FFmpeg Executable|ffmpeg.exe";

            var result = ffmpegExecBrowse.ShowDialog();

            if (result == DialogResult.OK)
            {
                getFromPATH_ffmpeg.Checked = false;
                set.ffmpeg_path = ffmpegExecBrowse.FileName;
                ffmpegPathBox.Text = set.ffmpeg_path;
                //getFromPATH_ffmpeg.Checked = false;
                //pathToggle();
            }
        }

        private void getFromPATH_blender_click(object sender, EventArgs e)
        {
            pathToggle();
        }

        /// <summary>
        /// If the "USE PATH" option is checked, sets EXE_path to def_EXE and disables the EXEPathBox
        /// </summary>
        void pathToggle()
        {
            // empty strings before setting
            BlenderLabel.Text = string.Empty;
            FFmpegLabel.Text = string.Empty;

            var L11 = "Blender EXE";
            var L22 = "FFMpeg EXE";
            string up = "(using PATH)";
            //StringBuilder LM = new StringBuilder();
            //LM.

            switch (getFromPATH_blender.CheckState)
            {
                case CheckState.Unchecked:
                    set.blender_path = blenderPathBox.Text;
                    BlenderLabel.Text = L11;
                    blenderPathBox.Enabled = true;
                    break;
                case CheckState.Checked:
                    set.blender_path = set.def_blender;
                    BlenderLabel.Text = (L11 + up);
                    blenderPathBox.Enabled = false;
                    break;
                case CheckState.Indeterminate:
                    break;
                default:
                    break;
            }
            switch (getFromPATH_ffmpeg.CheckState)
            {
                case CheckState.Unchecked:
                    set.ffmpeg_path = ffmpegPathBox.Text;
                    FFmpegLabel.Text = L22;
                    ffmpegPathBox.Enabled = true;
                    break;
                case CheckState.Checked:
                    set.ffmpeg_path = set.def_ffmpeg;
                    FFmpegLabel.Text = (L22 + up);
                    ffmpegPathBox.Enabled = false;
                    break;
                case CheckState.Indeterminate:
                    break;
                default:
                    break;
            }

        }

        private void getFromPATH_ffmpeg_click(object sender, EventArgs e)
        {
            pathToggle();
        }

        private void saveAll_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigBRC.EXECheck();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("One (or both) paths are invalid");
                //throw;
                return;
            }
            set.Save();
            Close();
        }


        private void config_FormClosed(object sender, FormClosedEventArgs e)
        { } //breakpoint

        private void blenderPathBox_TextChanged(object sender, EventArgs e)
        {
            //set.blender_path = blenderPathBox.Text;
        }

        private void ffmpegPathBox_TextChanged(object sender, EventArgs e)
        {
            //set.ffmpeg_path = ffmpegPathBox.Text;
        }

        private void config_FormClosing(object sender, FormClosingEventArgs e)
        {
            string[] after = { set.blender_path, set.ffmpeg_path, set.segment_len.ToString() };
            string[] before = { vbBlender, vbFFmpeg, vbSegLen };
            bool compare = after.SequenceEqual(before);

            try
            {
                ConfigBRC.EXECheck();
            }
            catch (FileNotFoundException)
            {
               var er = MessageBox.Show("One (or both) paths are invalid");
                e.Cancel = (er == DialogResult.OK);
                return;
            }


            if (!compare)
            {
                var message = "Save settings?";
                var caption = "";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                e.Cancel = (result == DialogResult.No);
                if (result == DialogResult.Yes)
                {
                    set.Save();
                }

            }

        }

        
        //to do: return to previous val if uncheck
        private void getFromPATH_blender_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void getFromPATH_ffmpeg_CheckedChanged(object sender, EventArgs e)
        {
        }
    }

    public struct check4changes
    {
        private string config;

        public check4changes(string name)
        {
            this.config = name;
        }

        public override string ToString()
        {
            return this.config;
        }
    }
}


