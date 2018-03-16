// Part of the Blender Render Controller project
// https://github.com/RedRaptor93/BlenderRenderController
// Copyright 2017-present Pedro Oliva Rodrigues
// This code is released under the MIT licence

using System;
using System.Diagnostics;
using System.Text;

namespace BRClib.Commands
{
    public class RenderCmd : ExternalCommand
    {
        public RenderCmd(string program, string blendFile, string outputFile, 
                      Renderer renderer, Chunk range) : base(program)
        {
            BlendFile = blendFile;
            OutputFile = outputFile;
            Renderer = renderer;
            Range = range;
        }

        //public RenderCmd(string program, Project project, Renderer renderer, Chunk range) 
        //    : base(program)
        //{

        //}

        public string BlendFile { get; set; }
        public string OutputFile { get; set; }
        public Renderer Renderer { get; set; }
        public Chunk Range { get; set; }


        // 0=Blend file, 1=output, 2=Renderer, 3=Frame start, 4=Frame end
        const string RENDER_FMT = "-b \"{0}\" -o \"{1}\" -E {2} -s {3} -e {4} -a";

        protected override string GetArgs()
        {
            return String.Format(RENDER_FMT,
                                    BlendFile, 
                                    OutputFile,
                                    Renderer,
                                    Range.Start,
                                    Range.End);
        }
    }
}
