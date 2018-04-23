using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Gtk;

namespace BlenderRenderController
{
    static class Glade
    {
        public static Builder LoadUI(string gladeFile)
        {
            Builder builder;

            using (Stream stream = GetEmbedded(gladeFile))
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


        static Stream GetEmbedded(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assbName = assembly.GetName().Name;
            var resourceName = assbName + '.' + name;

            return assembly.GetManifestResourceStream(resourceName);
        }

    }

    static class Extentions
    {
        public static int Px(this IconSize iconSize)
        {
            int size;

            switch (iconSize)
            {
                case IconSize.Menu:
                case IconSize.SmallToolbar:
                case IconSize.Button:
                    size = 16;
                    break;
                case IconSize.LargeToolbar:
                    size = 24;
                    break;
                case IconSize.Dnd:
                    size = 32;
                    break;
                case IconSize.Dialog:
                    size = 48;
                    break;
                case IconSize.Invalid:
                default:
                    size = -1;
                    break;
            }

            return size;
        }
    }
}
