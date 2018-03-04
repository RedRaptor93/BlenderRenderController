// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using BlenderRenderController.Services;
using BRClib;
using BRClib.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            System.Diagnostics.Process.Start(url);
        }

        public async Task GetBlendInfo(string blendFile)
        {
            if (!File.Exists(blendFile))
            {
                // error: file does not exist
                return;
            }

            var giScript = Scripts.GetProjectInfo;
            var cmd = new GetInfoCmd(Settings.Current.BlenderProgram, blendFile, giScript);
            await cmd.RunAsync();

            if (cmd.StdOutput.Length == 0)
            {
                // error: no info received
                return;
            }

            BlendData blendData = BlendData.FromPyOutput(cmd.StdOutput);

            if (blendData == null)
            {
                // error: Unexpected output.
                return;
            }

            _proj = new Project(blendData)
            {
                BlendFilePath = blendFile,
                MaxConcurrency = Environment.ProcessorCount
            };

            if (RenderFormats.IMAGES.Contains(blendData.FileFormat))
            {
                // warning: Render format is Img
            }

            if (string.IsNullOrWhiteSpace(_proj.OutputPath))
            {
                // warning: outputPath is unset, use blend path

                _proj.OutputPath = Path.GetDirectoryName(blendFile);
            }
            else
                _proj.OutputPath = Path.GetDirectoryName(_proj.OutputPath);

            // notify UI of changed
            OnPropertyChanged(nameof(Project));
        }
    }
}
