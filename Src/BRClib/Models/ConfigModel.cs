// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using Newtonsoft.Json;
using System.Collections.Generic;

namespace BRClib
{
    [JsonObject(Description = "Brc settings")]
    public class ConfigModel
    {
        [JsonProperty("RecentBlends")]
        public Infra.RecentBlendsCollection RecentProjects { get; set; }

        public string BlenderProgram { get; set; }
        public string FFmpegProgram { get; set; }
        public bool DisplayToolTips { get; set; }
        public AfterRenderAction AfterRender { get; set; }
        public Renderer Renderer { get; set; }
        public bool DeleteChunksFolder { get; set; }
        public int LoggingLevel { get; set; }
    }
}
