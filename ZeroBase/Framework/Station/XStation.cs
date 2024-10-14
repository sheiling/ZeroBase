using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Diagnostics;


namespace ZeroBase
{
    public delegate void OnStationStateRaised(XStationState obj);
    public class XStation : XStationEventHandler
    {
        public event Action RedLightON;
        public event Action OrangeLightON;
        public event Action GreenLightON;
        public event Action AllLightsOFF;
        public event Action<XStationState> OnStationStateChanged;

        public event OnStationStateRaised StationStateRaised;

        private List<XTask> tasks = new List<XTask>();
        private XDo redLight = null;
        private XDo orangeLight = null;
        private XDo greenLight = null;
        private Timer m_Timer;
        private LightType m_LightType;
        private int m_LightFlashFlag;
        private bool m_LightFlash;
        private XStationState m_State;
        private XStationState m_LastState;
        private int stationId;
        private string name;
        private object m_RunMode;
        private Stopwatch m_StopWatch = new Stopwatch();
        private int currenttaskid;
        private bool IsAllHomeOk = false;
        private bool m_isEnable = true;
        //private bool m_IsStationRuning = false;

        public XStation(int stationId, string name)
        {
            this.stationId = stationId;
            this.name = name;
            m_Timer = new Timer(new TimerCallback(Changing), null, 0, 800);
            m_LightFlash = false;
            m_LightFlashFlag = 1;
            RedLightON += new Action(OnlyRedLightOn);
            OrangeLightON += new Action(OnlyOrangeLightOn);
            GreenLightON += new Action(OnlyGreenLightOn);
            AllLightsOFF += new Action(AllLightsOff);
            m_State = XStationState.WAITRESET;
            m_LastState = XStationState.NONE;
            SetState(m_State);
            IsEnable = true;
        }

        public int StationId { get { return this.stationId; } }

        public string Name { get { return this.name; } }

        public bool IsEnable
        {
            get
            {
                return m_isEnable;
            }
            set
            {
                m_isEnable = value;
            }
        }

        public XStationState State
        {
            get
            {
                lock (this)
                {
                    return m_State;
                }
            }
        }
        public List<XTask> Tasks
        {
            get { return tasks; }
        }
        public int TaskCouts
        {
            get { return tasks.Count(); }
        }
        public void BindTask(int taskId)
        {
            XTask task = XTaskManager.Instance.FindTaskById(taskId);
            task.StationId = StationId;
            foreach (XTask item in tasks)
            {
                if (item.GetType() == task.GetType())
                {
                    return;
                }
            }
            tasks.Add(task);
        }


        #region Signal

        public void SetLightGreenDo(int setDoId)
        {
            greenLight = XDevice.Instance.FindDoById(setDoId);
        }

        public void SetLightOrangeDo(int setDoId)
        {
            orangeLight = XDevice.Instance.FindDoById(setDoId);
        }

        public void SetLightRedDo(int setDoId)
        {
            redLight = XDevice.Instance.FindDoById(setDoId);
        }

        #endregion


        #region HandleEvent

        public override int HandleEvent(XEvent xEvent)
        {
            currenttaskid = xEvent.CurrentTaskID;
            switch (xEvent.EventID)
            {

                case XEventID.RST:
                    if (xEvent.CurrentTaskID == -2) //全部任务  目前只测试控制不同任务单独回原点 其他方法待后续完善
                    {
                        foreach (XTask task in tasks)
                        {

                            task.HandleEvent(xEvent);
                        }
                    }
                    else
                    {
                        foreach (XTask task in tasks)
                        {
                            if (task.TaskId == xEvent.CurrentTaskID)
                            {
                                task.HandleEvent(xEvent);
                                break;
                            }
                        }
                    }
                    break;
                case XEventID.START:
                    PrimOnStart();
                    break;
                case XEventID.RESET:
                    PrimOnReset();
                    break;
                case XEventID.ALARM:
                    foreach (XTask task in tasks)
                    {
                        task.HandleEvent(xEvent);
                    }
                    PrimOnAlarm((XAlarmLevel)xEvent.EventArgs.AlarmLevel);
                    break;
                case XEventID.ESTOP:
                    foreach (XTask task in tasks)
                    {
                        task.HandleEvent(xEvent);
                    }
                    PrimOnEStop();
                    break;
                case XEventID.PAUSE:
                    foreach (XTask task in tasks)
                    {
                        task.HandleEvent(xEvent);
                    }
                    PrimOnPause();
                    break;
                case XEventID.CONTINUE:
                    foreach (XTask task in tasks)
                    {
                        task.HandleEvent(xEvent);
                    }
                    PrimOnContinue();
                    break;
                case XEventID.STOPMUSTRESET:
                    foreach (XTask task in tasks)
                    {
                        task.HandleEvent(xEvent);
                    }
                    PrimOnStop();
                    break;
            }
            return 0;
        }

        private void PrimOnStart()
        {
            if (m_State == XStationState.WAITRUN)
            {
                SetState(XStationState.RUNNING);
                foreach (XTask task in tasks)
                {
                    task.Start(m_RunMode);
                }
            }
        }

        private void PrimOnReset()
        {


            if (m_State == XStationState.ESTOP ||
               m_State == XStationState.ALARM ||
               m_State == XStationState.STOP ||
               m_State == XStationState.WAITRESET ||
               m_State == XStationState.WAITRUN ||
               m_State == XStationState.PAUSE)
            {
                SetState(XStationState.RESETING);
                if (currenttaskid == -2)
                {
                    foreach (XTask task in tasks)
                    {
                        task.Reset();
                    }
                }
                else
                {
                    foreach (XTask task in tasks)
                    {
                        if (task.TaskId == currenttaskid)
                        {
                            task.Reset();
                            break;
                        }
                    }
                }
            }
        }

        private void PrimOnEStop()
        {
            SetState(XStationState.ESTOP);
            foreach (XTask task in tasks)
            {
                task.Cancel();
            }
        }

        private void PrimOnAlarm(XAlarmLevel level)
        {
            switch (level)
            {
                case XAlarmLevel.STOP:
                    if (m_State != XStationState.ESTOP)
                    {
                        SetState(XStationState.ALARM);
                    }
                    break;
                case XAlarmLevel.PAUSE:
                    PrimOnPause();
                    break;
            }
        }

        private void PrimOnStop()
        {
            if (m_State != XStationState.ESTOP &&
                m_State != XStationState.ALARM)
            {
                SetState(XStationState.STOP);
                foreach (XTask task in tasks)
                {
                    task.Cancel();
                }
            }
        }

        private void PrimOnPause()
        {
            if (m_State == XStationState.RUNNING)
            {
                SetState(XStationState.PAUSE);
            }
        }

        private void PrimOnContinue()
        {
            if (m_State == XStationState.PAUSE)
            {
                SetState(XStationState.RUNNING);
            }
        }

        #endregion


        #region Light

        private enum LightType
        {
            RED,
            ORANGE,
            GREEN
        }

        private void Changing(object state)
        {
            //if (m_State != m_LastState)
            {
                if (OnStationStateChanged != null)
                {
                    OnStationStateChanged(m_State);
                }
            }
            if (m_LightFlash == false)
            {
                m_LightFlashFlag = 1;
                return;
            }
            switch (m_LightFlashFlag)
            {
                case 0:
                    if (AllLightsOFF != null)
                    {
                        AllLightsOFF();
                    }
                    m_LightFlashFlag = 1;
                    break;
                case 1:
                    SetLightOn();
                    m_LightFlashFlag = 0;
                    break;
            }
            m_LastState = m_State;
        }

        private void SetLightOn()
        {
            switch (m_LightType)
            {
                case LightType.RED:
                    if (RedLightON != null)
                    {
                        RedLightON();
                    }
                    break;
                case LightType.ORANGE:
                    if (OrangeLightON != null)
                    {
                        OrangeLightON();
                    }
                    break;
                case LightType.GREEN:
                    if (GreenLightON != null)
                    {
                        GreenLightON();
                    }
                    break;
            }
        }

        private void OnlyRedLightOn()
        {
            if (redLight != null)
            {
                redLight.SetDo(1);
            }
            if (orangeLight != null)
            {
                orangeLight.SetDo(0);
            }
            if (greenLight != null)
            {
                greenLight.SetDo(0);
            }
        }

        private void OnlyOrangeLightOn()
        {
            if (redLight != null)
            {
                redLight.SetDo(0);
            }
            if (orangeLight != null)
            {
                orangeLight.SetDo(1);
            }
            if (greenLight != null)
            {
                greenLight.SetDo(0);
            }
        }

        private void OnlyGreenLightOn()
        {
            if (redLight != null)
            {
                redLight.SetDo(0);
            }
            if (orangeLight != null)
            {
                orangeLight.SetDo(0);
            }
            if (greenLight != null)
            {
                greenLight.SetDo(1);
            }
        }

        private void AllLightsOff()
        {
            if (redLight != null)
            {
                redLight.SetDo(0);
            }
            if (orangeLight != null)
            {
                orangeLight.SetDo(0);
            }
            if (greenLight != null)
            {
                greenLight.SetDo(0);
            }
        }

        #endregion


        public void SetState(XStationState sts)
        {
            m_LightFlash = false;
            lock (this)
            {
                m_State = sts;
            }
            if (OnStationStateChanged != null)
            {
                OnStationStateChanged(m_State);
            }
            StationStateRaised?.Invoke(m_State);
            switch (sts)
            {
                case XStationState.ESTOP:
                    m_LightType = LightType.RED;
                    m_LightFlash = true;
                    StopWatch_Stop();
                    break;
                case XStationState.ALARM:
                    m_LightType = LightType.RED;
                    m_LightFlash = true;
                    StopWatch_Stop();
                    break;
                case XStationState.RESETING:
                    m_LightType = LightType.ORANGE;
                    m_LightFlash = false;
                    SetLightOn();
                    StopWatch_Reset();
                    break;
                case XStationState.RUNNING:
                    m_LightType = LightType.GREEN;
                    m_LightFlash = false;
                    SetLightOn();
                    //StopWatch_Start();
                    break;
                case XStationState.PAUSE:
                    m_LightType = LightType.GREEN;
                    m_LightFlash = true;
                    StopWatch_Stop();
                    break;
                case XStationState.STOP:
                    m_LightType = LightType.RED;
                    m_LightFlash = true;
                    StopWatch_Stop();
                    break;
                case XStationState.WAITRESET:
                    m_LightType = LightType.ORANGE;
                    m_LightFlash = true;
                    StopWatch_Stop();
                    break;
                case XStationState.WAITRUN:
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        if (!tasks[i].TaskHomeOK)
                        {
                            IsAllHomeOk = false;
                            break;
                        }
                        else
                        {
                            IsAllHomeOk = true;
                        }
                    }
                    if (IsAllHomeOk)
                    {
                        m_LightType = LightType.GREEN;
                        m_LightFlash = false;
                        SetLightOn();
                        StopWatch_Stop();
                    }
                    break;
            }
        }


        #region StopWatch

        public void StopWatch_Start()
        {
            this.m_StopWatch.Start();
        }

        public void StopWatch_Reset()
        {
            this.m_StopWatch.Reset();
        }

        public void StopWatch_ReStart()
        {
            this.m_StopWatch.Restart();
        }

        public void StopWatch_Stop()
        {
            this.m_StopWatch.Stop();
        }

        public double ElapsedMilliseconds
        {
            get { return this.m_StopWatch.ElapsedMilliseconds; }
        }

        #endregion

        /// <summary>
        /// 工站开始，工站绑定的所有任务开始，运行Task.Running()
        /// </summary>
        public void Start(object runMode)
        {
            if (!IsEnable)
                return;
            m_RunMode = runMode;
            switch (m_State)
            {
                case XStationState.WAITRUN:
                    PostEvent(this, XEventID.START);
                    break;
                case XStationState.PAUSE:
                    foreach (XTask task in tasks)
                    {
                        if (task.XRobortCommand != null)
                        {
                            task.RobortGoOn();
                        }

                    }
                    PostEvent(this, XEventID.CONTINUE, true);
                    break;
            }

        }
        /// <summary>
        /// 工站暂停，工站绑定的所有任务暂停
        /// </summary>
        public void Pause()
        {
            if (!IsEnable)
                return;
            PostEvent(this, XEventID.PAUSE, true);
            foreach (XTask task in tasks)
            {
                if (task.XRobortCommand != null)
                {
                    task.RobortPauseOn();
                }

            }
        }
        /// <summary>
        /// 工站继续，工站绑定的所有任务继续
        /// </summary>
        public void Continue()
        {
            if (!IsEnable)
                return;
            foreach (XTask task in tasks)
            {
                if (task.XRobortCommand != null)
                {
                    task.RobortGoOn();
                }

            }
            PostEvent(this, XEventID.CONTINUE, true);
        }
        /// <summary>
        /// 工站复位，工站绑定的所有任务复位，运行Task.Homing()
        /// </summary>
        public void Reset(int currenttaskid)
        {
            if (!IsEnable)
                return;
            PostEvent(this, XEventID.RST, true, currenttaskid);
            PostEvent(this, XEventID.RESET, true, currenttaskid);
        }
        public void Reset()
        {
            if (!IsEnable)
                return;
            PostEvent(this, XEventID.RST);
            PostEvent(this, XEventID.RESET);
        }
        /// <summary>
        /// 工站停止，工站绑定的所有任务停止
        /// </summary>
        public void Stop()
        {
            if (!IsEnable)
                return;
            foreach (XTask task in tasks)
            {
                if (task.XRobortCommand != null)
                {
                    task.RobortStopByIO();
                    task.RobortDisConnect();
                }

            }
            PostEvent(this, XEventID.STOPMUSTRESET, true);
        }

    }

    public enum XStationState
    {
        NONE,
        ESTOP,
        ALARM,
        STOP,
        WAITRESET,
        RESETING,
        WAITRUN,
        RUNNING,
        PAUSE,
    }
}
