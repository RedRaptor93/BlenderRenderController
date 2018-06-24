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

    static class Helpers
    {
        

        public static VMDialogResult VMDR(int response)
        {
            switch (response)
            {
                // Special cases
                case 1:
                    return VMDialogResult.Retry;
                case 2:
                    return VMDialogResult.Cancel;
            }

            switch ((ResponseType)response)
            {
                case ResponseType.Ok:
                    return VMDialogResult.Ok;
                case ResponseType.Cancel:
                    return VMDialogResult.Cancel;
                case ResponseType.Yes:
                    return VMDialogResult.Yes;
                case ResponseType.No:
                    return VMDialogResult.No;
            }

            throw new ArgumentException("value not mapped to ResponseType", nameof(response));
        }
    }
}
