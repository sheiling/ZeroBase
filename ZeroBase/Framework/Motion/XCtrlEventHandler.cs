using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroBase
{
    public abstract class XCtrlEventHandler : XEventHandler
    {
        public XCtrlEventHandler()
        {
            XController.Instance.EventServer.RegisterForNotification(this, XEventID.SIGNAL);
        }
    }
}
