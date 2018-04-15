using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading;
using Gtk;


namespace BlenderRenderController
{
    public class WindowBase : Window, ISynchronizeInvoke
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

        private readonly object _sync = new object();
        public bool InvokeRequired => true;

        public IAsyncResult BeginInvoke(Delegate method, object[] args)
        {
            var result = new SimpleAsyncResult();
            Application.Invoke(delegate
            {
                result.AsyncWaitHandle = new ManualResetEvent(false);

                try
                {
                    result.AsyncState = Invoke(method, args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    result.Exception = ex;
                }

                result.IsCompleted = true;
            });

            return result;
        }

        public object EndInvoke(IAsyncResult result)
        {
            if (!result.IsCompleted)
            {
                result.AsyncWaitHandle.WaitOne();
            }

            return result.AsyncState;
        }

        public object Invoke(Delegate method, object[] args)
        {
            lock (_sync)
            {
                return method.DynamicInvoke(args);
            }
        }

        public class SimpleAsyncResult : IAsyncResult
        {
            private object _state;


            public bool IsCompleted { get; set; }


            public WaitHandle AsyncWaitHandle { get; internal set; }


            public object AsyncState
            {
                get
                {
                    if (Exception != null)
                    {
                        throw Exception;
                    }
                    return _state;
                }
                internal set { _state = value; }
            }

            public bool CompletedSynchronously
            {
                get { return IsCompleted; }
            }


            internal Exception Exception { get; set; }

        }
    }
}
