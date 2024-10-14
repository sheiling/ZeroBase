using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace ZeroBase
{
    public sealed class XController : XTask
    {
        private XEventServer eventServer = new XEventServer();
        private XAlarmEventServer alarmEventServer = new XAlarmEventServer(); 
  
        private readonly static XController instance = new XController();
        XController()
        {
        }

        public static XController Instance
        {
            get { return instance; }
        }

        public XEventServer EventServer
        {
            get { return this.eventServer; }
        }

        public XAlarmEventServer AlarmEventServer
        {
            get { return this.alarmEventServer; }
        }

        public void Start()
        {
            this.eventServer.Start();
            this.alarmEventServer.Start();
        }

        public void Stop()
        {
            this.eventServer.Stop();
            this.alarmEventServer.Stop();
            foreach (XTask task in XTaskManager.Instance.Tasks.Values)
            {
                task.Cancel();
            }
        }

        public int _MoveHome(int axidId)
        {
            return MoveHome(axidId);
        }

        public int _MoveAbs(int axisId, double pos, double vel, bool checkLmt = true)
        {
            return MoveAbs(axisId, pos, vel, checkLmt);
        }

        public int _MoveAbs(int[] axisId, double[] pos, double vel, bool checkLmt = true)
        {
            return MoveAbs(axisId, pos, vel, checkLmt);
        }

        public int _MovePosition(XPosition position, double vel)
        {
            return MovePosition(position, vel);
        }

        public int _MoveRel(int axisId, double distance, double vel)
        {
            return MoveRel(axisId, distance, vel);
        }
        public int _MoveJog(int axisId, int isStart)
        {
            return MoveJog(axisId, isStart);
        }

        public int _setPosition(ushort axisId,int position)
        {
            return SetPosition(axisId, position);
        }

        public int _CleanALM(ushort axisId)
        {
            return CleanALM((int)axisId);
        }

        public int _MoveStop()
        {
            return MoveStop();
        }

        public bool _WaitMoveDone()
        {
            return WaitMoveDone();
        }
    }

  
}
