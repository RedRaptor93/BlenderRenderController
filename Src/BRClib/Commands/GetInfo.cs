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

        public GetInfoCmd() : base(Global.Settings.BlenderProgram)
        {
        }


        public string BlendFile { get; set; }
        public string ProjInfoScript { get; set; } = Global.GetProjInfoScript;


        protected override string GetArgs()
        {
            var fmt = Global.Settings.ArgFormats["getinfo"];
            return String.Format(fmt, BlendFile, ProjInfoScript);
        }

    }
}
