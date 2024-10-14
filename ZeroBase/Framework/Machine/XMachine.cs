using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZeroBase
{
    public enum MachineModeType
    {
        Production,
        Engineering,
        CPK,
        GRR,
        IPQC,
        None
    }
    public sealed class XMachine : XMachineEventHandler
    {
        private XDi signalEStop = null;
        private List<XDi> signalDoor = new List<XDi>();
        private bool m_DoorEnabled;
        private XDi signalReset = null;
        private XDi signalStart = null;
        private XDi signalStop = null;
        public int signalBeer;
        private bool eStop;
        private bool lastEStop;
        private bool[] roboetalarm;
        private bool[] lastrrobortalarm;
        private bool[] roboetProRuning;
        private bool[] lastrrobortProRuning;
        private MachineModeType machinemode = MachineModeType.None;
        private Thread _thread;
        private static readonly XMachine instance = new XMachine();
        private Dictionary<int, RobortInformation> robort_inf = new Dictionary<int, RobortInformation>();
        XMachine()
        {

        }
        public static XMachine Instance
        {
            get { return instance; }
        }
        public void BindRobortInf(int robortid, RobortInformation robortinf)
        {
            if (robort_inf.ContainsKey(robortid) == false)
            {
                robort_inf.Add(robortid, robortinf);
            }

        }
        public void SendErrorMessage(int robortid, XAlarmEventArgs _alarmEventArgs, string errnum)
        {
            if (robort_inf.ContainsKey(robortid))
            {
                robort_inf[robortid]._alarmEventArgs = _alarmEventArgs;
                robort_inf[robortid].ErrNo = robort_inf[robortid].RobortName + ":" + errnum;
                robort_inf[robortid].RobortWaitErrorMessage.Set();
            }
        }
        public MachineModeType MachineMode
        {
            get { return machinemode; }
            set { machinemode = value; }
        }
        public override int HandleEvent(XEvent xEvent)
        {
            switch (xEvent.EventID)
            {
                case XEventID.SIGNAL:
                    PrimOnSignal();
                    break;
            }
            return 0;
        }
        public void Start()
        {
            Stop();
            if (signalEStop != null)
            {
                XDevice.Instance.CardMap[signalEStop.CardId].Update();
                signalEStop.Update();
            }
            if (robort_inf.Count != 0)
            {
                roboetalarm = new bool[robort_inf.Count];
                lastrrobortalarm = new bool[robort_inf.Count];
                roboetProRuning = new bool[robort_inf.Count];
                lastrrobortProRuning = new bool[robort_inf.Count];
                for (int i = 0; i < robort_inf.Count; i++)  //gaodianping zhenghchang
                {
                    roboetProRuning[i] = true;
                    lastrrobortProRuning[i] = true;
                }

            }
            _thread = new Thread(new ThreadStart(T_PrimOnSignal));
            _thread.IsBackground = true;
            _thread.Start();
        }

        public void Stop()
        {
            if (this._thread != null)
            {
                this._thread.Abort();
            }
        }
        private void T_PrimOnSignal()
        {
            while (true)
            {
                if (signalEStop != null)
                {
                    eStop = !signalEStop.STS;
                    if (eStop == true && lastEStop == false)
                    {
                        foreach (XStation station in XStationManager.Instance.Stations.Values)
                        {
                            PostEventEStop(station);
                        }
                        XEventArgs e = new XEventArgs();
                        e.StationId = 0;
                        e.AlarmLevel = (int)XAlarmLevel.STOP;
                        XController.Instance.AlarmEventServer.PostEvent(XAlarmReporter.Instance, XEventID.ESTOP, e, null, true);
                    }
                    else if (eStop == false && lastEStop == true)
                    {
                        foreach (XStation station in XStationManager.Instance.Stations.Values)
                        {
                            PostEvent(station, XEventID.WAITRESET);
                        }
                    }
                    lastEStop = eStop;
                }

                if (eStop == false)
                {
                    if (signalReset != null)
                    {
                        if (signalReset.STS == true)
                        {
                            Thread.Sleep(3000);
                            if (signalReset.STS == true)
                            {
                                foreach (XStation station in XStationManager.Instance.Stations.Values)
                                {
                                    PostEvent(station, XEventID.RST);
                                    PostEvent(station, XEventID.RESET);
                                }
                            }
                        }
                    }
                    if (robort_inf.Count != 0)
                    {
                        int i = 0;
                        foreach (RobortInformation robortinf in robort_inf.Values)
                        {

                            if (robortinf.Di_Alarm != null)
                            {
                                roboetalarm[i] = robortinf.Di_Alarm.STS;
                                if (roboetalarm[i] == true && lastrrobortalarm[i] == false)
                                {
                                    if (robortinf.RobortWaitErrorMessage.WaitOne(1000))
                                    {
                                        PostEvent(XStationManager.Instance.Stations[robortinf.StationId], XAlarmLevel.STOP, robortinf._alarmEventArgs, robortinf.ErrNo);
                                    }
                                    else
                                    {
                                        PostEvent(XStationManager.Instance.Stations[robortinf.StationId], XAlarmLevel.STOP, XSysAlarmId.RobortAlarm, robortinf.RobortId);
                                    }

                                }
                                lastrrobortalarm[i] = roboetalarm[i];
                            }
                            if (robortinf.Di_ProgramRunnig != null)
                            {
                                roboetProRuning[i] = robortinf.Di_ProgramRunnig.STS;

                                if (roboetProRuning[i] == false && lastrrobortProRuning[i] == true)
                                {
                                    if (XStationManager.Instance.Stations[robortinf.StationId].State == XStationState.RUNNING)
                                    {
                                        PostEvent(XStationManager.Instance.Stations[robortinf.StationId], XAlarmLevel.STOP, XSysAlarmId.RobortProgramError, robortinf.RobortId);
                                    }
                                }
                                lastrrobortProRuning[i] = roboetProRuning[i];
                            }
                            i++;
                        }


                    }

                }
                if (signalStart != null)
                {
                    if (signalStart.STS == true)
                    {
                        foreach (XStation station in XStationManager.Instance.Stations.Values)
                        {
                            PostEvent(station, XEventID.START);
                        }
                    }
                }
                Thread.Sleep(3);
            }
        }

        private int PrimOnSignal()
        {
            if (this.m_DoorEnabled)
            {
                foreach (XDi di in signalDoor)
                {
                    if (di.PLF)
                    {
                        foreach (XStation station in XStationManager.Instance.Stations.Values)
                        {
                            PostEvent(station, XAlarmLevel.PAUSE, XSysAlarmId.DOOR_OPEN);
                        }
                    }
                }
            }

            if (signalStop != null)
            {
                if (signalStop.PLF == true)
                {
                    foreach (XStation station in XStationManager.Instance.Stations.Values)
                    {
                        PostEvent(station, XEventID.STOPMUSTRESET, true);
                    }
                }
            }
            return 0;
        }

        public void SetEStopDi(int setDiId)
        {
            signalEStop = XDevice.Instance.FindDiById(setDiId);
        }
        public void SetBerrDo(int setDoId)
        {
            signalBeer = setDoId;
        }
        public void AddDoorDi(int setDiId)
        {
            signalDoor.Add(XDevice.Instance.FindDiById(setDiId));
        }

        public void SetResetDi(int setDiId)
        {
            signalReset = XDevice.Instance.FindDiById(setDiId);
        }

        public void SetStartDi(int setDiId)
        {
            signalStart = XDevice.Instance.FindDiById(setDiId);
        }

        public void SetStopDi(int setDiId)
        {
            signalStop = XDevice.Instance.FindDiById(setDiId);
        }

        public bool DoorEnabled
        {
            get { return this.m_DoorEnabled; }
            set { this.m_DoorEnabled = value; }
        }

    }
}
