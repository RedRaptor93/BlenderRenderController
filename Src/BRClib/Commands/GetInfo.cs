// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System;

namespace BRClib.Commands
{
    public class GetInfoCmd : ExternalCommand
    {

        public GetInfoCmd(string blendFile) : this()
        {
            BlendFile = blendFile;
        }

        public GetInfoCmd() : base(Global.Settings.BlenderProgram) { }


        public string BlendFile { get; set; }
        public string ProjInfoScript { get; set; } = Global.GetProjInfoScript;


        // 0=Blend file, 1=get_project_info.py
        const string GETINFO_FMT = "-b \"{0}\" -P \"{1}\"";

        protected override string GetArgs()
        {
            return String.Format(GETINFO_FMT,
                                    BlendFile, 
                                    ProjInfoScript);
        }
    }
}
