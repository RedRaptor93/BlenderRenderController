﻿// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BRClib.Extentions;
using BRClib.Commands;
using NLog;
using BRCRes = BRClib.Properties.Resources;
using System.Threading.Tasks;

namespace BRClib
{
    public class BrcViewModel : BindingBase
    {

        public delegate void ShowDialogCB(string title, string message, string details, bool retry);

        private Project _proj;

        public Project Project
        {
            get { return _proj; }
            set
            {
                if (SetProperty(ref _proj, value))
                {
                    OnPropertyChanged(nameof(CanRender));
                    OnPropertyChanged(nameof(ProjectLoaded));
                }
            }
        }

        private bool _busy;

        public bool IsBusy
        {
            get { return _busy; }
            set { SetProperty(ref _busy, value); }
        }

        private bool _configOk;

        public bool ConfigOk
        {
            get { return _configOk; }
            set { SetProperty(ref _configOk, value); }
        }

        public Action<string> StatusCb { get; set; }
        public ShowDialogCB ShowDialogCb { get; set; }

        public bool ProjectLoaded => Project != null;
        public bool CanRender => ConfigOk && ProjectLoaded;
        public bool CanLoadNewProject => ConfigOk && !IsBusy;
        public bool CanEditCurrentProject => ProjectLoaded && !IsBusy;
        public bool CanReloadCurrentProject => ConfigOk && ProjectLoaded && !IsBusy;

        public string DefaultStatusMessage
        {
            get
            {
                string msg;

                if (CanLoadNewProject)
                {
                    msg = ProjectLoaded ? "Ready" : "Select a file";
                }
                else if (IsBusy)
                {
                    msg = "BRC is busy";
                }
                else if (!ConfigOk)
                {
                    msg = "Invalid settings";
                }
                else
                {
                    msg = "No default message for current state.";
                }

                return msg;
            }
        }


        public void OpenDonationPage()
        {
            string business = "9SGQVK6TK2UJG";
            string description = "Donation%20for%20Blender%20Render%20Controller";
            string country = "BR";
            string currency = "USD";

            string url = "https://www.paypal.com/cgi-bin/webscr" +
                    "?cmd=_donations" +
                    "&business=" + business +
                    "&lc=" + country +
                    "&item_name=" + description +
                    "&item_number=BRC" +
                    "&currency_code=" + currency +
                    "&bn=PP%2dDonationsBF";

            Process.Start(url);
        }

        public bool OpenOutputFolder()
        {
            if (Directory.Exists(Project.OutputPath))
            {
                Process.Start(Project.OutputPath);
                return true;
            }

            return false;
        }


        public void UpdateCurrentChunks(IEnumerable<Chunk> chunks)
        {
            if (chunks.TotalLength() > Project.TotalFrames 
             || chunks.SequenceEqual(Project.ChunkList))
            {
                return;
            }

            if (Project.ChunkList.Count > 0)
            {
                Project.ChunkList.Clear();
            }

            foreach (var chnk in chunks)
            {
                Project.ChunkList.Add(chnk);
            }

            Project.ChunkLenght = chunks.First().Length;
        }

        public async Task<bool> OpenBlendFile(string blendFile)
        {
            logger.Info("Loading " + Path.GetFileName(blendFile) + " ...");
            StatusCb("Reading .blend file...");

            if (!File.Exists(blendFile))
            {
                ShowDialogCb("Error", "File not found", null, false);
                return false;
            }

            var getinfo = new GetInfoCmd(blendFile);
            await getinfo.RunAsync();
            var report = getinfo.GenerateReport();

            if (getinfo.StdOutput.Length == 0)
            {
                ShowDialogCb("Error", BRCRes.AppErr_NoInfoReceived, report, true);
                return false;
            }

            var data = BlendData.FromPyOutput(getinfo.StdOutput);
            if (data == null)
            {
                ShowDialogCb("Error", BRCRes.AppErr_UnexpectedOutput, report, true);
                return false;
            }

            var proj = new Project(data)
            {
                BlendFilePath = blendFile
            };

            if (RenderFormats.IMAGES.Contains(proj.FileFormat))
            {
                var eMsg = string.Format(BRCRes.AppErr_RenderFormatIsImage, proj.FileFormat);
                ShowDialogCb("Warning", eMsg, null, false);
            }

            if (string.IsNullOrWhiteSpace(proj.OutputPath))
            {
                // use .blend folder path if outputPath is unset, display a warning about it
                ShowDialogCb("Warning", BRCRes.AppErr_BlendOutputInvalid, null, false);
                proj.OutputPath = Path.GetDirectoryName(blendFile);
            }
            else
                proj.OutputPath = Path.GetDirectoryName(proj.OutputPath);

            proj.ChunksDirPath = proj.DefaultChunksDirPath;

            Debug.Assert(!string.IsNullOrEmpty(proj.ChunksDirPath));

            Project = proj;
            return true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(GetType().Name + ": \n");
            string elemFmt = "\t{0} = {1}\n";

            sb.AppendFormat(elemFmt, nameof(ProjectLoaded), ProjectLoaded);
            sb.AppendFormat(elemFmt, nameof(IsBusy), IsBusy);
            sb.AppendFormat(elemFmt, nameof(ConfigOk), ConfigOk);

            return sb.ToString();
        }


        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


    }
}
