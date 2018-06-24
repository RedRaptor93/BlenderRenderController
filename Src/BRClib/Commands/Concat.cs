// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System;

namespace BRClib.Commands
{
    public class ConcatCmd : ExternalCommand
    {
        public ConcatCmd() : base(Global.Settings.FFmpegProgram)
        {
        }


        public string ConcatTextFile { get; set; }
        public string OutputFile { get; set; }
        public string MixdownFile { get; set; }
        public TimeSpan? Duration { get; set; }


        protected override string GetArgs()
        {
            string args;
            string durText = Duration.HasValue
                     ? "-t " + Duration.Value.ToString(@"hh\:mm\:ss") : string.Empty;

            if (System.IO.File.Exists(MixdownFile))
            {
                var fmt = GetFormat("concat", "withMixdown");
                args = string.Format(fmt, ConcatTextFile, MixdownFile, durText, OutputFile);
            }
            else
            {
                var fmt = GetFormat("concat", "noMixdown");
                args = string.Format(fmt, ConcatTextFile, durText, OutputFile);
            }

            return args;
        }

    }
}
