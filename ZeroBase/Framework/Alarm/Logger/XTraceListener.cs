using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ZeroBase
{
    class MyTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            XLogger.Instance.WriteLine(message);
        }

        public override void WriteLine(string message)
        {
            XLogger.Instance.WriteLine(message);
        }

        public override void WriteLine(object o, string category)
        {
            string msg = "";
            if (string.IsNullOrWhiteSpace(category) == false) 
            {
                msg = category + " : ";
            }
            if (o is Exception) 
            {
                var ex = (Exception)o;
                msg += ex.Message + " => ";
                msg += ex.StackTrace;
            }
            else if (o != null)
            {
                msg = o.ToString();
            }
            WriteLine(msg);
        }


    }
}
