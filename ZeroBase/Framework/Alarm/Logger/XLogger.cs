using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;


namespace ZeroBase
{
    public sealed class XLogger : XEventHandler
    {
        private static readonly XLogger instance = new XLogger();
        //public static event Action<XTask, String> OnTaskWriteLine;
        private object obj = new object();
        XLogger()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Trace.Listeners.Add(new MyTraceListener());
        }
        public static XLogger Instance
        {
            get { return instance; }
        }

        public override int HandleEvent(XEvent xEvent)
        {
            if (xEvent.EventID == XEventID.LOG)
            {
                PrimOnLog(xEvent.EventArgs.DateTime, xEvent.EventArgs.StringValue);
            }
            return 0;
        }

        private void PrimOnLog(DateTime dateTime, string str)
        {
            string fn;
            if (!Directory.Exists("Log"))
            {
                Directory.CreateDirectory("Log");
            }
            fn = "Log\\log " + DateTime.Now.ToString("yyyyMMdd ") + ".txt";
            StreamWriter sw = File.AppendText(fn);
            sw.WriteLine(dateTime.ToString("yyyy/MM/dd HH:mm:ss:fff") + " => " + str);
            sw.Dispose();
        }

        public void WriteLine(string str)
        {
            lock (obj)
            {
                XEventArgs e = new XEventArgs();
                e.StringValue = str;
                e.DateTime = DateTime.Now;
                XController.Instance.AlarmEventServer.PostEvent(this, XEventID.LOG, e, null, true);
            }
        }

        private void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Trace.WriteLine(e, "ZeroBase");
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Trace.WriteLine(e, "ZeroBase");
        }
    }
}
