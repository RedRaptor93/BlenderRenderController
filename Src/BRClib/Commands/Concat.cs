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


        public string ConcatTextFile { get; set; }
        public string OutputFile { get; set; }
        public string MixdownFile { get; set; }
        public TimeSpan? Duration { get; set; }

        // ref: https://ffmpeg.org/ffmpeg-all.html#concat-1
        // 0=ChunkTxtPath, 1=Optional mixdown input, 2=Optional duration, 3=Final file path + .EXT
        const string CONCAT_FMT = "-f concat -safe 0 -i \"{0}\" {1} {2} \"{3}\" -y";

        protected override string GetArgs()
        {
            var codecText = !string.IsNullOrWhiteSpace(MixdownFile) 
                ? "-i \"" + MixdownFile + "\" -c:v copy -map 0:v:0 -map 1:a:0" : "-c copy";

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
