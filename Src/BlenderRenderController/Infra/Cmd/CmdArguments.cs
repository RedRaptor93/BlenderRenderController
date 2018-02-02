using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;

namespace BlenderRenderController.Infra.Cmd
{

    class Arguments
    {
        [Value(0, HelpText = "A .blend file to load on startup", MetaName = "blend file", MetaValue = "Path\\to\\project.blend")]
        public string BlendFile { get; set; }

        [Option("clear", HelpText = "scripts, settings, all")]
        public string ClearAction { get; set; }

        [Option(HelpText = "Sets if the app will run in portable mode (using the local directory). " +
            "Overrides the 'portable' setting in the .config file.")]
        public string Portable { get; set; }
    }
}
