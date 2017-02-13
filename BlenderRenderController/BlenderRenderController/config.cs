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

        public config()
        {
            InitializeComponent();
            //string execPath = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            string execPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        }

        private void config_Load(object sender, EventArgs e)
        {

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
            blenderExecBrowse.Filter = "Blender Executable|*.exe";
            
            var result = blenderExecBrowse.ShowDialog();

            if (result == DialogResult.OK)
            {
                set.blender_path = blenderExecBrowse.FileName;
                blenderPathBox.Text = set.blender_path;
                //savePaths(saveMode.blender);
            }
        }

        void savePaths()
        {
            try
            {
                ConfigBRC.EXECheck();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("One (or both) paths are invalid");
                //throw;
            }

            set.Save();
        }

        private void setFFmpeg_Click(object sender, EventArgs e)
        {
            var ffmpegExecBrowse = new OpenFileDialog();
            ffmpegExecBrowse.Filter = "FFmpeg Executable|*.exe";

            var result = ffmpegExecBrowse.ShowDialog();

            if (result == DialogResult.OK)
            {
                set.ffmpeg_path = ffmpegExecBrowse.FileName;
                ffmpegPathBox.Text = set.ffmpeg_path;
                //savePaths(saveMode.ffmpeg);
            }
        }

        private void getFromPATH_blender_click(object sender, EventArgs e)
        {
            pathToggle();
        }

        void pathToggle()
        {        
            switch (getFromPATH_blender.CheckState)
            {
                case CheckState.Unchecked:
                    set.blender_path = blenderPathBox.Text;
                    blenderPathBox.Enabled = true;
                    break;
                case CheckState.Checked:
                    set.blender_path = set.def_blender;
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
                    ffmpegPathBox.Enabled = true;
                    break;
                case CheckState.Checked:
                    set.ffmpeg_path = set.def_ffmpeg;
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
            set.Save();
        }

        
        private void config_FormClosed(object sender, FormClosedEventArgs e)
        { } //breakpoint

        private void blenderPathBox_TextChanged(object sender, EventArgs e)
        {
            set.blender_path = blenderPathBox.Text;
        }

        private void ffmpegPathBox_TextChanged(object sender, EventArgs e)
        {
            set.ffmpeg_path = ffmpegPathBox.Text;
        }

        private void config_FormClosing(object sender, FormClosingEventArgs e)
        {
            var message = "Save settings?";
            var caption = "";
            MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            switch (DialogResult)
            {
                case DialogResult.Yes:
                    MainForm.EXEcheck();
                    set.Save();
                    this.Close();
                    break;
                case DialogResult.No:
                    break;
                default:
                    //MessageBox.Show("something went wrong");
                    break;
            }
        }
    }
}
