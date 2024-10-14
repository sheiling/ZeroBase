using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;


namespace ZeroBase
{
    public abstract class XEventServerBase : XObject
    {
        private Dictionary<XEventID, List<XEventHandler>> eventHandlers = new Dictionary<XEventID, List<XEventHandler>>();

        public void RegisterForNotification(XEventHandler observer, XEventID eventID)
        {
            if (eventHandlers.ContainsKey(eventID) == false)
            {
                eventHandlers.Add(eventID, new List<XEventHandler>());
            }
            List<XEventHandler> observerList = eventHandlers[eventID];
            if (observerList.Contains(observer) == false)
            {
                observerList.Add(observer);
            }
        }

        public void UnRegister(XEventHandler observer, XEventID eventID)
        {
            if (eventHandlers.ContainsKey(eventID) == false)
            {
                return;
            }
            List<XEventHandler> observerList = eventHandlers[eventID];
            if (observerList.Contains(observer) == true)
            {
                observerList.Remove(observer);
            }
        }

        public int NotifyObservers(XEventID eventID, bool highPriorityFlag = false)
        {
            if (eventHandlers.ContainsKey(eventID) == false)
            {
                return -1;
            }
            foreach (XEventHandler observer in eventHandlers[eventID])
            {
                observer.PostEvent(observer, eventID, highPriorityFlag);
            }
            return 0;
        }

        public abstract void PostEvent(XEventHandler target, XEventID eventID, XEventArgs eventArgs = null,
            XObject sender = null, bool isPriority = false, int currenttaskid = -2);

        protected abstract void DispatchEvent();

        protected abstract void ProcessEventQueue(CancellationToken cancellationToken);


        #region Task

        private Task worker;
        private CancellationTokenSource stopToken;

        public bool IsRunning
        {
            get { return this.worker != null && !this.worker.IsCompleted; }
        }

        public void Start()
        {
            if (this.IsRunning)
            {
                return;
            }
            this.stopToken = new CancellationTokenSource();
            this.worker = Task.Factory.StartNew(
                () => this.ProcessEventQueue(this.stopToken.Token),
                this.stopToken.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        public void Stop()
        {
            if (!this.IsRunning || this.stopToken.IsCancellationRequested)
            {
                return;
            }

            try
            {
                this.stopToken.Cancel();
            }
            catch
            {
            }
            this.worker = null;
        }
        #endregion


        #region XEventPool

        private ConcurrentQueue<XEvent> eventQueue = new ConcurrentQueue<XEvent>();
        private int count;
        protected void InitEventPool(int count)
        {
            this.count = count;
            for (int i = 0; i < count; i++)
            {
                eventQueue.Enqueue(new XEvent());
            }
        }

        public XEvent CreateEvent(XEventHandler target, XEventID eventID, XEventArgs eventArgs = null, XObject sender = null, int currenttaskid = -2)
        {
            XEvent xEvent;
            if (eventQueue.TryDequeue(out xEvent))
            {
                xEvent.EventHandler = target;
                xEvent.EventID = eventID;
                xEvent.EventArgs = eventArgs;
                xEvent.Sender = sender;
                xEvent.CurrentTaskID = currenttaskid;
            }
            else
            {
                xEvent = new XEvent(target, eventID, eventArgs, sender, currenttaskid);
            }
            return xEvent;
        }

        protected void Store(XEvent e)
        {
            if (eventQueue.Count < this.count)
            {
                eventQueue.Enqueue(e);
            }
        }

        #endregion
    }
}
