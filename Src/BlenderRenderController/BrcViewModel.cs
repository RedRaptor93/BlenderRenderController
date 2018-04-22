// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using BlenderRenderController.Services;
using BRClib;
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

namespace BlenderRenderController
{
    class BrcViewModel : BindingBase
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

        public async Task<Tuple<int, string>> GetBlendInfo(string blendFile)
        {
            if (!File.Exists(blendFile))
            {
                // error: file does not exist
                return Tuple.Create(1, "File does not exist");
            }

            var giScript = Scripts.GetProjectInfo;
            var cmd = new GetInfoCmd(Settings.Current.BlenderProgram, blendFile, giScript);

            await cmd.RunAsync();

            var report = string.Format(BRCRes.Extern_Report, "blender", cmd.ExitCode, cmd.StdOutput, cmd.StdError);

            if (cmd.StdOutput.Length == 0)
            {
                // error: no info received
                Console.WriteLine("No information recived from Blender");
                return Tuple.Create(2, report);
            }

            BlendData blendData = BlendData.FromPyOutput(cmd.StdOutput);

            if (blendData == null)
            {
                // error: Unexpected output.
                Console.WriteLine("Unexpected get_project_info output");
                return Tuple.Create(3, report);
            }

            var proj = new Project(blendData)
            {
                BlendFilePath = blendFile,
            };

            string warns = string.Empty;

            if (RenderFormats.IMAGES.Contains(blendData.FileFormat))
            {
                // warning: Render format is Img
                Console.WriteLine("Render format is Img");
                warns += "- Render format is Img\n";
            }

            if (string.IsNullOrWhiteSpace(proj.OutputPath))
            {
                // warning: outputPath is unset, use blend path
                Console.WriteLine("OutputPath is unset, using .blend dir");
                proj.OutputPath = Path.GetDirectoryName(blendFile);

                warns += "- OutputPath is unset, using .blend dir\n";
            }
            else
                proj.OutputPath = Path.GetDirectoryName(proj.OutputPath);

            Console.WriteLine("Loaded Project");
            _proj = proj;
            OnPropertyChanged(nameof(Project));

            return Tuple.Create(0, warns);
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
