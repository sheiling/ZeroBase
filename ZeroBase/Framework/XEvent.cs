using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ZeroBase
{
    public class XEvent : XObject
    {
        private XObject _sender;
        private XEventHandler _target;
        private XEventArgs _eventArgs;
        private XEventID _eventID;
        private int _currenttaskID;

        public XEvent() : this(null, 0, null, null, -2)
        {
        }

        public XEvent(XEventHandler target, XEventID eventID, XEventArgs eventArgs = null, XObject sender = null, int currenttaskid = -2)
        {
            this._target = target;
            this._eventID = eventID;
            this._eventArgs = eventArgs;
            this._sender = sender;
            this._currenttaskID = currenttaskid;
        }

        public XObject Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        public XEventHandler EventHandler
        {
            get { return _target; }
            set { _target = value; }
        }

        public XEventID EventID
        {
            get { return _eventID; }
            set { _eventID = value; }
        }

        public XEventArgs EventArgs
        {
            get { return _eventArgs; }
            set { _eventArgs = value; }
        }
        public int CurrentTaskID
        {
            get { return _currenttaskID; }
            set { _currenttaskID = value; }
        }
        public int Execute()
        {
            if (_target == null)
            {
                return -1;
            }
            _target.HandleEvent(this);
            return 0;
        }
    }

    public enum XEventID
    {
        SIGNAL,

        SETSERVO,
        MOVE,
        MOVESTOP,
        SETDO,
        WAITDI,
        TIMEOUT,
        RST,

        ESTOP,
        ALARM,
        STOPMUSTRESET,
        RESET,
        WAITRESET,
        START,
        PAUSE,
        CONTINUE,
        DICONTINUE,
        LOG
    }
}
