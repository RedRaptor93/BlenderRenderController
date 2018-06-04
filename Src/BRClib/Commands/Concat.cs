// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System;

namespace BRClib.Commands
{
    public class ConcatCmd : ExternalCommand
    {

        public ConcatCmd(string program, string concatTextFile, string outputFile, 
                      string mixdownFile = null, TimeSpan? duration = null)
            : base(program)
        {
            ConcatTextFile = concatTextFile;
            OutputFile = outputFile;
            MixdownFile = mixdownFile;
            Duration = duration;
        }

        public ConcatCmd(string program) : base(program) { }

        public ConcatCmd() : base(Global.Settings.FFmpegProgram) { }


        public string ConcatTextFile { get; set; }
        public string OutputFile { get; set; }
        public string MixdownFile { get; set; }
        public TimeSpan? Duration { get; set; }


        protected override string GetArgs()
        {
            // ref: https://ffmpeg.org/ffmpeg-all.html#concat-1
            // 0=ChunkTxtPath, 1=codec specs, 2=Optional duration, 3=Final file path + .EXT
            const string CONCAT_FMT = "-f concat -safe 0 -i \"{0}\" {1} {2} \"{3}\" -y";

            string codecText;

            if (string.IsNullOrWhiteSpace(MixdownFile))
            {
                codecText = "-c copy";
            }
            else
            {
                codecText = string.Format("-i \"{0}\" -c copy -map 0:v:0 -map 1:a:0", MixdownFile);
            }

            var durText = Duration.HasValue
                ? "-t " + Duration.Value.ToString(@"hh\:mm\:ss") : string.Empty;

            return string.Format(CONCAT_FMT, 
                                    ConcatTextFile, 
                                    codecText, 
                                    durText, 
                                    OutputFile);
        }
        
    }
}
