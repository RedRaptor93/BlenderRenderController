// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using BRClib.Commands;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRClib.Extentions;
using BRCRes = BRClib.Properties.Resources;

namespace BRClib
{
    public class BrcViewModel : BindingBase
    {
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

        public bool ProjectLoaded
        {
            get { return Project != null; }
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

        /// <summary>
        /// Loads a blend file
        /// </summary>
        /// <returns>
        /// A tuple containing a code and a string
        /// Error codes
        /// 0 = no error, message field might contain warnings
        /// 1 = Blend file not found
        /// 2 = No info receved
        /// 3 = Unexpected output
        /// </returns>
        public async Task<Tuple<int, string>> GetBlendInfo(string blendFile)
        {
            if (!File.Exists(blendFile))
            {
                // error: file does not exist
                return Tuple.Create(1, "File not found");
            }

            var giScript = Global.GetProjInfoScript;
            var cmd = new GetInfoCmd(Global.Settings.BlenderProgram, blendFile, giScript);

            await cmd.RunAsync();

            var report = cmd.GenerateReport();

            if (cmd.StdOutput.Length == 0)
            {
                // error: no info received
                return Tuple.Create(2, report);
            }

            BlendData blendData = BlendData.FromPyOutput(cmd.StdOutput);

            if (blendData == null)
            {
                // error: Unexpected output.
                return Tuple.Create(3, report);
            }

            var proj = new Project(blendData)
            {
                BlendFilePath = blendFile,
            };

            var warnings = new List<string>();

            if (RenderFormats.IMAGES.Contains(blendData.FileFormat))
            {
                // warning: Render format is Img
                warnings.Add(BRCRes.AppErr_RenderFormatIsImage);
            }

            if (string.IsNullOrWhiteSpace(proj.OutputPath))
            {
                // warning: outputPath is unset, use blend path
                proj.OutputPath = Path.GetDirectoryName(blendFile);
                warnings.Add(BRCRes.AppErr_BlendOutputInvalid);
            }
            else
                proj.OutputPath = Path.GetDirectoryName(proj.OutputPath);

            _proj = proj;
            OnPropertyChanged(nameof(Project));

            return Tuple.Create(0, string.Join("\n", warnings));
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



        public override string ToString()
        {
            var sb = new StringBuilder(GetType().Name + ": \n");
            string elemFmt = "\t{0} = {1}\n";

            sb.AppendFormat(elemFmt, nameof(ProjectLoaded), ProjectLoaded);
            sb.AppendFormat(elemFmt, nameof(IsBusy), IsBusy);
            sb.AppendFormat(elemFmt, nameof(ConfigOk), ConfigOk);

            return sb.ToString();
        }
    }
}
