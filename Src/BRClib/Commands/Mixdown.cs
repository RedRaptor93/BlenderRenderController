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
            ArgFormat = MIXDOWN_FMT;
        }


        public string BlendFile { get; set; }
        public Chunk Range { get; set; }
        public string MixdownScript { get; set; } = Global.MixdownScript;
        public string OutputFolder { get; set; }


        protected override string GetArgs()
        {
            return String.Format(ArgFormat, 
                                    BlendFile,
                                    Range.Start,
                                    Range.End,
                                    MixdownScript,
                                    OutputFolder);
        }

        // 0=Blend file, 1=start frame, 2=end frame, 3=mixdown_audio.py, 4=Output Folder
        const string MIXDOWN_FMT = "-b \"{0}\" -s {1} -e {2} -P \"{3}\" -- \"{4}\"";

    }
}
