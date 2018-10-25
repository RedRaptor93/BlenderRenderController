// Part of the Blender Render Controller project
// https://github.com/rehdi93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System;
using System.IO;
using System.Windows.Forms;
using static BRClib.Global;


namespace BlenderRenderController
{

    public partial class SettingsForm : Form
    {

        public SettingsForm()
        {
            InitializeComponent();
            settingsBindingSrc.Add(Settings);
        }

        private void onFormLoad(object sender, EventArgs e)
        {
            // load settings
            cbLoggingLvl.SelectedIndex = Settings.LoggingLevel;
            cbLoggingLvl.SelectedIndexChanged += CbLoggingLvl_SelectedIndexChanged;

            var blenderExe = Path.GetFileName(Settings.BlenderProgram);
            var ffmpegExe = Path.GetFileName(Settings.FFmpegProgram);

            findBlenderDialog.Filter = "Blender|" + blenderExe;
            findBlenderDialog.Title += blenderExe;

            findFFmpegDialog.Filter = "FFmpeg|" + ffmpegExe;
            findFFmpegDialog.Title += ffmpegExe;

            if (!File.Exists(Settings.BlenderProgram))
            {
                blenderPathTextBox.Text = string.Empty;
            }
            if (!File.Exists(Settings.FFmpegProgram))
            {
                ffmpegPathTextBox.Text = string.Empty;
            }
        }

        private void CbLoggingLvl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.LoggingLevel = cbLoggingLvl.SelectedIndex;
        }

        private void blenderChangePathButton_Click(object sender, EventArgs e)
        {
            findBlenderDialog.InitialDirectory = blenderPathTextBox.Text.Trim();

            var result = findBlenderDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                blenderPathTextBox.Text = findBlenderDialog.FileName;
            }
        }

        private void ffmpegChangePathButton_Click(object sender, EventArgs e)
        {
            findFFmpegDialog.InitialDirectory = ffmpegPathTextBox.Text.Trim();

            var result = findFFmpegDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                ffmpegPathTextBox.Text = findFFmpegDialog.FileName;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!CheckProgramPaths())
            {
                this.DialogResult = DialogResult.Abort;
            }

        }

    }
}
