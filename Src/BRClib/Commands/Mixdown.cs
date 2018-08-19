// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System;

namespace BRClib.Commands
{
    public class MixdownCmd : ExternalCommand
    {
        public MixdownCmd() : base(Global.Settings.BlenderProgram)
        {
        }


        public string BlendFile { get; set; }
        public Chunk Range { get; set; }
        public string MixdownScript { get; set; } = Global.MixdownScript;
        public string OutputFolder { get; set; }


        protected override string GetArgs()
        {
            var fmt = Global.Settings.ArgFormats["mixdown"];
            return String.Format(fmt, 
                                    BlendFile,
                                    Range.Start,
                                    Range.End,
                                    MixdownScript,
                                    OutputFolder);
        }

    }
}
