// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using BRClib;
using System;
using System.Collections.Generic;
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

    }
}
