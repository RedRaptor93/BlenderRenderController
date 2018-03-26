﻿using System;
using System.IO;
using System.Reflection;
using Gtk;


namespace BlenderRenderController
{
    public class WindowBase : Window
    {
        private Builder _builder;
        private string _styleFile;

        protected WindowBase(string gladeFile, string styleFile, string rootElement)
            : this(gladeFile, rootElement)
        {
            StyleFile = styleFile;
        }
        
        protected WindowBase(string gladeFile, string rootElement)
            : this(CreateBuilder(gladeFile), rootElement) 
        { }



        private WindowBase(Builder builder, string root)
            : base(builder.GetObject(root).Handle)
        {
            _builder = builder;
            builder.Autoconnect(this);

            //DeleteEvent += GtkWindow_DeleteEvent;
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

        protected Builder Builder { get => _builder; }

        //void GtkWindow_DeleteEvent(object o, DeleteEventArgs args)
        //{
        //    Application.Quit();
        //    args.RetVal = true;
        //}


        static Builder CreateBuilder(string filename)
        {
            Builder builder;

            using (Stream stream = GetEmbeddedStream(filename))
            {
                builder = new Builder(stream);
            }

            return builder;
        }

        protected static Stream GetEmbeddedStream(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assbName = assembly.GetName().Name;
            var resourceName = assbName + '.' + name;

            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}
