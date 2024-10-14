using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ZeroBase
{
    public class XAlarmEventArgs : EventArgs
    {
        private int code;
        private string category;
        private string description;

        public XAlarmEventArgs(int code, string category, string description)
        {
            this.code = code;
            this.category = category;
            this.description = description;
        }

        public int Code
        {
            get { return this.code; }
            set { this.code = value; }
        }

        public int StationId { get; set; }

        public DateTime StartTime { get; set; }

        public string Category
        {
            get { return this.category; }
            set { this.category = value; }
        }

        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        public TimeSpan Duration { get; set; }
        public int AlarmLevel { get; set; }
    }
}
