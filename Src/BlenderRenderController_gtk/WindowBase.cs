using System;
using Gtk;


namespace BlenderRenderController
{
    public class WindowBase : Window
    {
        private Builder _builder;
        private string _styleFile;

        protected WindowBase(string gladeFile, string styleFile, string rootElement)
            : this(Glade.LoadUI(gladeFile, styleFile), rootElement)
        { }
        
        protected WindowBase(string gladeFile, string rootElement)
            : this(Glade.Load(gladeFile), rootElement) 
        { }

        private WindowBase(Builder builder, string root)
            : base(builder.GetObject(root).Handle)
        {
            _builder = builder;
            builder.Autoconnect(this);
        }

        private WindowBase(Tuple<Builder, CssProvider> elements, string root)
            : this(elements.Item1, root)
        {
            StyleContext.AddProviderForScreen(Screen, elements.Item2, 800);
        }


        protected Builder Builder { get => _builder; }

    }
}
