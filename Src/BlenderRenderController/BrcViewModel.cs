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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BRClib.Extentions;

namespace BlenderRenderController
{
    class BrcViewModel : BindingBase
    {

        public BrcViewModel()
        {
        }

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

        public void OpenOutputFolder()
        {
            if (Directory.Exists(Project.OutputPath))
            {
                Process.Start(Project.OutputPath);
            }
            else
            {
                MessageBox.Show("Output folder does not exist.", "",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
            }
        }

        public async Task GetBlendInfo(string blendFile)
        {
            if (!File.Exists(blendFile))
            {
                // error: file does not exist
                //Trace.TraceError("File does not exist");
                Console.WriteLine("File does not exist");
                MessageBox.Show("File does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var giScript = Scripts.GetProjectInfo;
            var cmd = new GetInfoCmd(Settings.Current.BlenderProgram, blendFile, giScript);

            await cmd.RunAsync();

            if (cmd.StdOutput.Length == 0)
            {
                // error: no info received
                Console.WriteLine("No information recived from Blender");
                return;
            }

            BlendData blendData = BlendData.FromPyOutput(cmd.StdOutput);

            if (blendData == null)
            {
                // error: Unexpected output.
                Console.WriteLine("Unexpected get_project_info output");
                //ShowMsgBox("Unexpected get_project_info output", MessageType.Error);
                MessageBox.Show("Unexpected get_project_info output", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var proj = new Project(blendData)
            {
                BlendFilePath = blendFile,
            };

            if (RenderFormats.IMAGES.Contains(blendData.FileFormat))
            {
                // warning: Render format is Img
                Console.WriteLine("Render format is Img");
                MessageBox.Show("Render format is Img", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (string.IsNullOrWhiteSpace(proj.OutputPath))
            {
                // warning: outputPath is unset, use blend path
                Console.WriteLine("OutputPath is unset, using .blend dir");

                proj.OutputPath = Path.GetDirectoryName(blendFile);
            }
            else
                proj.OutputPath = Path.GetDirectoryName(proj.OutputPath);

            Console.WriteLine("Loaded Project");
            _proj = proj;
            OnPropertyChanged(nameof(Project));
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
