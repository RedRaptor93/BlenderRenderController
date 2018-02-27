using System;
using System.IO;
using System.Reflection;
using Gtk;

using Debug = System.Diagnostics.Debug;

namespace BlenderRenderController
{
    public class GtkWindow : Window
    {
        private Builder _builder;
        private string _styleFile;

        protected GtkWindow(string gladeFile, string styleFile, string rootElement)
            : this(gladeFile, rootElement)
        {
            StyleFile = styleFile;
        }
        
        protected GtkWindow(string gladeFile, string rootElement)
            : this(CreateBuilder(gladeFile), rootElement) 
        { }



        private GtkWindow(Builder builder, string root)
            : base(builder.GetObject(root).Handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            DeleteEvent += GtkWindow_DeleteEvent;
        }


        public string StyleFile
        {
            get => _styleFile;
            private set
            {
                if (value == null)
                {
                    _styleFile = null;
                    return;
                }

                using (Stream stream = GetEmbeddedStream(value))
                using (StreamReader reader = new StreamReader(stream))
                {
                    var provider = new CssProvider();
                    provider.LoadFromData(reader.ReadToEnd());

                    StyleContext.AddProviderForScreen(Screen, provider, 800);
                }

                _styleFile = value;
            }
        }

        void GtkWindow_DeleteEvent(object o, DeleteEventArgs args)
        {
            Application.Quit();
            args.RetVal = true;
        }


        static Builder CreateBuilder(string filename)
        {
            Builder builder;

            using (Stream stream = GetEmbeddedStream(filename))
            {
                builder = new Builder(stream);
            }

            return builder;
        }

        static Stream GetEmbeddedStream(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assbName = assembly.GetName().Name;
            var resourceName = assbName + '.' + name;

            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}
