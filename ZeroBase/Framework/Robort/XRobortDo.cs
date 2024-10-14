using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroBase
{
    public class XRobortDo
    {
        private XCommandRobot command;
        private int actid;
        private string name;
        private string robotname;
        private int m_STS;
        private DOSTSTYPE sts;
        private object obj = new object();
        public XRobortDo(XCommandRobot command, int actid, string name, string robotname)
        {
            this.command = command;
            this.actid = actid;
            this.name = name;
            this.robotname = robotname;
        }
        public int RobotId { get; set; }

        public int TaskId { get; set; }
        public int Update()
        {
            int result = 0;

            lock (obj)
            {
                m_STS = GetDo(ref result);
            }
            return 0;
        }
        public int SetDo(int sts)
        {
            DOSTSTYPE dosts = sts == 1 ? DOSTSTYPE.HIGH : DOSTSTYPE.LOW;
            command.SetDOSts(actid, dosts);
            return 0;

        }
        private int GetDo(ref int result)
        {
            sts = DOSTSTYPE.LOW;
            result = 0;
            if (command.GetDOSts(actid, out sts))
            {
                if (sts == DOSTSTYPE.HIGH)
                {
                    result = 1;
                }
            }
            return result;

        }
        public int ActId
        {
            get
            {
                return actid;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }
        public string RobortName
        {
            get
            {
                return robotname;
            }
        }
        public bool STS
        {
            set
            {
                lock (obj)
                {
                    m_STS = value ? 1 : 0;
                }
            }
            get
            {
                lock (obj)
                {
                    return m_STS > 0;
                }
            }
        }

    }
}
