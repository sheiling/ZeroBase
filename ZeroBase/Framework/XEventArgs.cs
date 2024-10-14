using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ZeroBase
{
    public class XEventArgs : EventArgs
    {
        public DateTime DateTime { get; set; }
        public int StationId { get; set; }
        public int AlarmLevel { get; set; }
        public int AlarmId { get; set; }
        public int IntValue { get; set; }
        public string StringValue { get; set; }
        public bool BoolValue { get; set; }
        public double DoubleValue { get; set; }
        public XAlarmEventArgs AlarmEventArgs { get; set; }
    }
}
