﻿// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System;
using System.IO;
using System.Windows.Forms;


namespace BlenderRenderController
{
    public partial class SettingsForm : Form
    {
        private BrcSettings _setts;

        public SettingsForm()
        {
            InitializeComponent();
            _setts = Services.Settings.Current;
            settingsBindingSrc.Add(_setts);
        }

        private void onFormLoad(object sender, EventArgs e)
        {
            // load settings
            cbLoggingLvl.SelectedIndex = _setts.LoggingLevel;
            cbLoggingLvl.SelectedIndexChanged += CbLoggingLvl_SelectedIndexChanged;

            var blenderExe = Path.GetFileName(_setts.BlenderProgram);
            var ffmpegExe = Path.GetFileName(_setts.FFmpegProgram);

            findBlenderDialog.Filter = "Blender|" + blenderExe;
            findBlenderDialog.Title += blenderExe;

            findFFmpegDialog.Filter = "FFmpeg|" + ffmpegExe;
            findFFmpegDialog.Title += ffmpegExe;

            if (!File.Exists(_setts.BlenderProgram))
            {
                blenderPathTextBox.Text = string.Empty;
            }
            if (!File.Exists(_setts.FFmpegProgram))
            {
                ffmpegPathTextBox.Text = string.Empty;
            }
        }

        private void CbLoggingLvl_SelectedIndexChanged(object sender, EventArgs e)
        {
            _setts.LoggingLevel = cbLoggingLvl.SelectedIndex;
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
            if (!Services.Settings.CheckCorrectConfig())
            {
                this.DialogResult = DialogResult.Abort;
            }

        }

    }
}
