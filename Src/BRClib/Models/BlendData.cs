// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace BRClib
{
    /// <summary>
    /// Represents the settings from a Blender project file
    /// </summary>
    public class BlendData : BindingBase
    {
        private int _start, _end;
        private double _fps;
        private string _outPath, _projName, 
            _activeScene, _res;


        [JsonProperty("start")]
        public int Start
        {
            get => _start;
            set => SetProperty(ref _start, value);
        }

        [JsonProperty("end")]
        public int End
        {
            get => _end;
            set => SetProperty(ref _end, value);
        }

        [JsonProperty("fps")]
        public double Fps
        {
            get => _fps;
            set => SetProperty(ref _fps, value);
        }

        [JsonProperty("resolution")]
        public string Resolution
        {
            get => _res;
            set => SetProperty(ref _res, value);
        }

        [JsonProperty("outputPath")]
        public string OutputPath
        {
            get => _outPath;
            set => SetProperty(ref _outPath, value);
        }

        [JsonProperty("projectName")]
        public string ProjectName
        {
            get => _projName;
            set => SetProperty(ref _projName, value);
        }

        [JsonProperty("sceneActive")]
        public string ActiveScene
        {
            get => _activeScene;
            set => SetProperty(ref _activeScene, value);
        }

        // scene.render.image_settings.file_format
        [JsonProperty("fileFormat")]
        public string FileFormat { get; set; }

        // scene.render.ffmpeg.format
        [JsonProperty("ffmpegFmt")]
        public string FFmpegVideoFormat { get; set; }

        // scene.render.ffmpeg.audio_codec
        [JsonProperty("ffmpegAudio")]
        public string FFmpegAudioCodec { get; set; }


        /// <summary>
        /// Parses the output of get_project_info
        /// </summary>
        /// <param name="outputLines">Standard output lines</param>
        /// <returns>A <see cref="BlendData"/> object</returns>
        /// <remarks>
        /// When executing get_project_info script, Blender may also print errors
        /// alongside the project info (a commun case if there're custom plugins installed) 
        /// this method will ignore out those errors.
        /// </remarks>
        public static BlendData FromPyOutput(IEnumerable<string> outputLines)
        {
            StringBuilder jsonInfo = new StringBuilder();
            bool jsonStarted = false;
            int curlyStack = 0;

            // Filter out errors and create data
            foreach (string line in outputLines)
            {
                if (line.Contains("{"))
                {
                    jsonStarted = true;
                    curlyStack++;
                }
                if (jsonStarted)
                {
                    if (!line.ToLower().Contains("blender quit") && curlyStack > 0)
                    {
                        jsonInfo.AppendLine(line);
                    }
                    if (line.Contains("}"))
                    {
                        curlyStack--;
                        if (curlyStack == 0)
                        {
                            break;
                        }
                    }
                }
            }

            var json = jsonInfo.ToString();
            return JsonConvert.DeserializeObject<BlendData>(json);
        }

        /// <summary>
        /// Parses the output of get_project_info
        /// </summary>
        /// <param name="stdOutput">Standard output</param>
        /// <returns>A <see cref="BlendData"/> object</returns>
        /// <remarks>
        /// When executing get_project_info script, Blender may also print errors
        /// alongside the Json containig the project info (a commun case if there're
        /// custom plugins installed) this method will ignore out those errors.
        /// </remarks>
        public static BlendData FromPyOutput(string stdOutput)
        {
            return FromPyOutput(stdOutput.Split('\n'));
        }

        //public string AudioFileFormat
        //{
        //    get
        //    {
        //        if (FFmpegAudioCodec == null)
        //            return null;

        //        switch (FFmpegAudioCodec)
        //        {
        //            case "PCM":
        //                return "wav";
        //            case "VORBIS":
        //                return "ogg";
        //            case "NONE":
        //                return "ac3";
        //            default:
        //                return FFmpegAudioCodec.ToLower();
        //        }
        //    }
        //}
    }
}
