// http://lists.dot.net/pipermail/gtk-sharp-list/2005-September/006374.html

using System;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;

namespace Coversant.CGtk
{
    //public delegate void EmptyDelegate();
    //public delegate T EmptyDelegate<T>();

    public class GtkSynchronizeInvoke : ISynchronizeInvoke
    {
        private Queue<WorkItem> work;
        private volatile int threadid;
        private static GtkSynchronizeInvoke _MainThread;
        public static GtkSynchronizeInvoke MainThread
        {
            get { return _MainThread; }
        }
        static GtkSynchronizeInvoke()
        {
            _MainThread = new GtkSynchronizeInvoke();
        }
        public static void Init()
        {

        }
        public GtkSynchronizeInvoke()
        {
            //Assume this was created in the Gtk thread.
            threadid = Thread.CurrentThread.ManagedThreadId;

            //Make sure we have the correct threadid for Gtk.
            GLib.Idle.Add(new GLib.IdleHandler(SetThreadID));
        }

        /// <summary>
        /// Adds delegate to the queue to be invoked in the Gtk main loop.
        /// </summary>
        /// <param name="method">Method to Invoke</param>
        /// <param name="args">Method's Arguments</param>
        /// <returns>IAsyncResult</returns>
        public IAsyncResult BeginInvoke(Delegate method, params object[] args)
        {
            if (args == null) args = new object[] { };
            WorkItem result = new WorkItem(null, method, args);
            Enqueue(result);
            return result;
        }
        private void Enqueue(WorkItem result)
        {
            lock (this) if (work == null) work = new Queue<WorkItem>();
            lock (work)
            {
                work.Enqueue(result);
            }
            WakeupMain();
        }
        /// <summary>
        /// Blocks the current thread while waiting for a response.
        /// </summary>
        /// <param name="result">The IAsyncResult returned by BeginInvoke</param>
        /// <returns>Return value of the method invoked by BeginInvoke</returns>
        public object EndInvoke(IAsyncResult result)
        {
            WorkItem item = (WorkItem)result;
            WaitFor(item);
            if (item.ExceptionThrown) throw item.Exception;
            return item.ReturnValue;
        }
        /// <summary>
        /// Invokes the method in the Gtk Main loop. Do not use this in a Gui thread, it may block it.
        /// </summary>
        /// <param name="method">Method</param>
        /// <param name="args">Arguments to pass to Method</param>
        /// <returns>Method's return value.</returns>
        public object Invoke(Delegate method, params object[] args)
        {
            IAsyncResult asyncResult;
            asyncResult = BeginInvoke(method, args);
            return EndInvoke(asyncResult);
        }
        public bool InvokeRequired
        {
            get
            {
                return Thread.CurrentThread.ManagedThreadId != threadid;
            }
        }

#if NET_2_0
        /// <summary>
        /// Adds delegate to the queue to be invoked in the Gtk main loop.
        /// </summary>
        /// <typeparam name="T">Return value type.</typeparam>
        /// <param name="method">Method to Invoke</param>
        /// <param name="args">Method's Arguments</param>
        /// <returns>IAsyncResult</returns>
        public IAsyncResult BeginInvoke<T>(Delegate method, params object[] args)
        {
            if (args == null) args = new object[] { };
            WorkItem<T> result = new WorkItem<T>(null, method, args);
            Enqueue(result);
            WakeupMain();
            return result;
        }
        public T EndInvoke<T>(IAsyncResult result)
        {
            WorkItem<T> item = (WorkItem<T>)result;
            WaitFor(item);
            if (item.ExceptionThrown) throw item.Exception;
            return item.ReturnValue;
        }
        /// <summary>
        /// Invokes the method in the Gtk Main loop. Do not use this in a Gui thread, it may block it.
        /// </summary>
        /// <typeparam name="T">Return Value Type</typeparam>
        /// <param name="method">Method</param>
        /// <param name="args">Arguments to pass to Method</param>
        /// <returns>Method's return value.</returns>
        public T Invoke<T>(Delegate method, params object[] args)
        {
            IAsyncResult asyncResult;
            asyncResult = BeginInvoke<T>(method, args);
            return EndInvoke<T>(asyncResult);
        }
#endif

        private void WakeupMain()
        {
            GLib.Idle.Add(new GLib.IdleHandler(Ready));
        }
        private bool Ready()
        {
            int count;
            lock (work) count = work.Count;
            while (count > 0)
            {
                WorkItem item;
                lock (work) item = work.Dequeue();
                if (item != null)
                {
                    item.ThreadID = Thread.CurrentThread.ManagedThreadId;
                    item.CallBack();
                }
                lock (work) count = work.Count;
            }
            return false;
        }
        private bool SetThreadID()
        {
            threadid = Thread.CurrentThread.ManagedThreadId;
            return false;
        }
        private void WaitFor(WorkItem item)
        {
            IAsyncResult result = ((IAsyncResult)item);
            if (threadid == Thread.CurrentThread.ManagedThreadId)
            {
                while (!result.IsCompleted)
                {
                    if (Gtk.Application.EventsPending())
                        Gtk.Application.RunIteration();
                    else
                        System.Threading.Thread.Sleep(50);
                }
            }
            else
            {
                //We have a problem if this is happening in a Gtk.MainLoop thread
                result.AsyncWaitHandle.WaitOne();
            }
        }

        #region ISynchronizeInvoke Members

        IAsyncResult ISynchronizeInvoke.BeginInvoke(Delegate method, object[] args)
        {
            return BeginInvoke(method, args);
        }

        object ISynchronizeInvoke.EndInvoke(IAsyncResult result)
        {
            return EndInvoke(result);
        }

        object ISynchronizeInvoke.Invoke(Delegate method, object[] args)
        {
            return Invoke(method, args);
        }

        bool ISynchronizeInvoke.InvokeRequired
        {
            get
            {
                return InvokeRequired;
            }
        }

        #endregion
    }

#if NET_2_0
    internal class WorkItem<T> : WorkItem
    {
        public new T ReturnValue
        {
            get
            {
                return (T)base.ReturnValue;
            }
            set
            {
                base.ReturnValue = value;
            }
        }
        public WorkItem(object state, Delegate method, params object[] args)
            : base(state , method, args)
        { }
    }
#endif

    internal class WorkItem : IAsyncResult
    {
        object[] _Args;
        object _AsyncState;
        volatile bool _Completed;
        Delegate _Method;
        ManualResetEvent _Event;
        object _ReturnValue;
        volatile int _ThreadID = -1;
        volatile int _CreatedThreadID;
        Exception _Exception;
        volatile bool _ExceptionThrown;

        internal WorkItem(object state, Delegate method, params object[] args)
        {
            _AsyncState = state;
            _Method = method;
            _Args = args;
            _Event = new ManualResetEvent(false);
            _Completed = false;
            _CreatedThreadID = Thread.CurrentThread.ManagedThreadId;
        }

        internal int ThreadID
        {
            get { return _ThreadID; }
            set { _ThreadID = value; }
        }
        internal object ReturnValue
        {
            get
            {
                object retval;
                lock (this)
                {
                    retval = _ReturnValue;
                }
                return retval;
            }
            set
            {
                lock (this)
                {
                    _ReturnValue = value;
                }
            }
        }
        internal Exception Exception
        {
            get
            {
                Exception ex;
                lock (this)
                {
                    ex = _Exception;
                }
                return ex;
            }
            set
            {
                lock (this)
                {
                    _Exception = value;
                }
                _ExceptionThrown = (_Exception != null);
            }
        }
        internal bool ExceptionThrown
        {
            get
            {
                return _ExceptionThrown;
            }
        }
        internal void CallBack()
        {
            try
            {
                ReturnValue = _Method.DynamicInvoke(_Args);
            }
            catch (Exception ex)
            {
                this.Exception = ex;
            }
            _ThreadID = Thread.CurrentThread.ManagedThreadId;
            _Event.Set();
            _Completed = true;
        }

        #region IAsyncResult Members

        object IAsyncResult.AsyncState
        {
            get { return _AsyncState; }
        }

        WaitHandle IAsyncResult.AsyncWaitHandle
        {
            get { return _Event; }
        }

        bool IAsyncResult.CompletedSynchronously
        {
            get { return false; }
        }

        bool IAsyncResult.IsCompleted
        {
            get { return _Completed; }
        }

        #endregion

    }
}