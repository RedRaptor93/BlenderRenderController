// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using BlenderRenderController.Infra;
using BRClib;
using System;
using Newtonsoft.Json;
using BRClib.Scripts;

namespace BlenderRenderController
{
    [JsonObject(Description = "Brc settings")]
    public class BrcSettings : IRenderContext
    {
        [JsonProperty("RecentBlends")]
        public RecentBlendsCollection RecentProjects { get; set; }

        public string BlenderProgram { get; set; }
        public string FFmpegProgram { get; set; }
        public bool DisplayToolTips { get; set; }
        public AfterRenderAction AfterRender { get; set; }
        public Renderer Renderer { get; set; }
        public bool DeleteChunksFolder { get; set; }
        public int LoggingLevel { get; set; }


        string IRenderContext.Blender => BlenderProgram;

        string IRenderContext.FFmpeg => FFmpegProgram;

        string IScriptPath.GetProjectInfo => Services.Scripts.GetProjectInfo;

        string IScriptPath.MixdownAudio => Services.Scripts.MixdownAudio;
    }
}
