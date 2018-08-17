using System;
using System.IO;
using System.Reflection;
using BRClib;
using Gtk;

namespace BlenderRenderController
{
    static class Glade
    {
        public static Builder Load(string gladeFile)
        {
            Builder builder;

            var assembly = Assembly.GetExecutingAssembly();
            var assbName = assembly.GetName().Name;
            var resourceName = $"{assbName}.Ui.{gladeFile}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                builder = new Builder(stream);
            }

            return builder;
        }

        public static Tuple<Builder, CssProvider> LoadUI(string glade, string style)
        {
            Builder builder;
            var styleProvider = new CssProvider();

            var assembly = Assembly.GetExecutingAssembly();
            var assbName = assembly.GetName().Name;

            using (var builder_stream = assembly.GetManifestResourceStream(assbName + ".Ui." + glade))
            using (var style_stream = assembly.GetManifestResourceStream(assbName + ".Ui." + style))
            using (var reader = new StreamReader(style_stream))
            {
                builder = new Builder(builder_stream);
                styleProvider.LoadFromData(reader.ReadToEnd());
            }

            return Tuple.Create(builder, styleProvider);
        }

    }

}
