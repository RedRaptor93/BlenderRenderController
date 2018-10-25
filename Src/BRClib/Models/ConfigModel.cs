// Part of the Blender Render Controller project
// https://github.com/rehdi93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using Env = System.Environment;
using static BRClib.Global;

namespace BRClib
{
    [JsonObject(Description = "Brc settings")]
    public class ConfigModel
    {
        public ConfigModel()
        {
            string blender = "blender", ffmpeg = "ffmpeg";
            string defBlenderPath, defFFmpegPath;

            switch (Env.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    var varName = Env.Is64BitOperatingSystem ? "ProgramW6432" : "ProgramFiles";
                    var pf = Env.GetEnvironmentVariable(varName);

                    defBlenderPath = FindProgram(blender, Path.Combine(pf, "Blender Foundation", "Blender"));
                    defFFmpegPath = FindProgram(ffmpeg);
                    break;
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                default:
                    defBlenderPath = FindProgram(blender);
                    defFFmpegPath = FindProgram(ffmpeg);
                    break;
            }

            BlenderProgram = defFFmpegPath;
            FFmpegProgram = defFFmpegPath;
            LoggingLevel = 0;
            DisplayToolTips = true;
            AfterRender = AfterRenderAction.MIX_JOIN;
            Renderer = Renderer.BLENDER_RENDER;
            RecentProjects = new List<string>();
            DeleteChunksFolder = false;
            ArgFormats = new Dictionary<string, string>
            {
                ["concat"] = "f concat -safe 0 -i \"{0}\" -c copy {1} \"{2}\" -y",
                ["concatmix"] = "-f concat -safe 0 -i \"{0}\" -i \"{1}\" -c copy -map 0:v:0 -map 1:a:0 {2} \"{3}\" -y",
                ["getinfo"] = "-b \"{0}\" -P \"{1}\"",
                ["mixdown"] = "-b \"{0}\" -s {1} -e {2} -P \"{3}\" -- \"{4}\"",
                ["render"] = "-b \"{0}\" -o \"{1}\" -E {2} -s {3} -e {4} -a"
            };
        }

        [JsonProperty("RecentBlends")]
        public IList<string> RecentProjects { get; set; }

        public string BlenderProgram { get; set; }
        public string FFmpegProgram { get; set; }
        public bool DisplayToolTips { get; set; }
        public AfterRenderAction AfterRender { get; set; }
        public Renderer Renderer { get; set; }
        public bool DeleteChunksFolder { get; set; }
        public int LoggingLevel { get; set; }

        public IDictionary<string, string> ArgFormats { get; set; }
    }
}
