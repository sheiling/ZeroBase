using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;


namespace ZeroBase
{
    public delegate void OnWarningChangedDelegate(string warningCode, string warningInfo, int level = 2);

    public sealed class XAlarmReporter : XAlarmEventHandler
    {
        private string SYSTEM = AlarmCategory.SYSTEM.ToString();
        private XAlarmEventArgs currentAlarm = new XAlarmEventArgs(0, "NONE", "NONE");
        private bool IsAlarming = false;
        private bool IsAlarmingForHive = false;
        private Dictionary<XSysAlarmId, XAlarmEventArgs> systemAlarms = new Dictionary<XSysAlarmId, XAlarmEventArgs>();
        private Dictionary<XAlarmLevel, XAlarmColor> alarmsColor = new Dictionary<XAlarmLevel, XAlarmColor>();
        private readonly static XAlarmReporter instance = new XAlarmReporter();
        private ConcurrentQueue<XAlarmEventArgs> OnAlarmingPageQueue = new ConcurrentQueue<XAlarmEventArgs>();
        private ConcurrentQueue<XAlarmEventArgs> OnProductingPageQueue = new ConcurrentQueue<XAlarmEventArgs>();
        private Thread _thread;
        private bool isAlarmingPageCreated = false;
        private bool isProductingPageCreated = false;

        XAlarmReporter()
        {
            InitSystemAlarms();
            InitAlarmsColor();
        }

        public bool IsAlarmingPageCreated
        {
            get { return isAlarmingPageCreated; }
            set { isAlarmingPageCreated = value; }
        }
        public bool AlarmingForHiveFlag
        {
            get { return IsAlarmingForHive; }
            set { IsAlarmingForHive = value; }
        }


        public bool IsProductingPageCreated
        {
            get { return isProductingPageCreated; }
            set { isProductingPageCreated = value; }
        }
        public static XAlarmReporter Instance
        {
            get { return instance; }
        }

        public event OnWarningChangedDelegate OnWarningChanged;

        public event Action<XAlarmEventArgs> OnAlarmingPage;

        public event Action<XAlarmEventArgs> OnProductingPage;

        public event Action<XAlarmEventArgs> OnAlarmCleared;
        public event Action<XAlarmEventArgs> OnAlarmClearedForHive;
        public event Action<XAlarmEventArgs> OnSaveAlarmReport;

        public Dictionary<XAlarmLevel, XAlarmColor> AlarmsColor
        {
            get { return alarmsColor; }
        }
        private void InitAlarmsColor()
        {
            alarmsColor.Add(XAlarmLevel.ONLYLOG, XAlarmColor.Green);
            alarmsColor.Add(XAlarmLevel.PAUSE, XAlarmColor.Yellow);
            alarmsColor.Add(XAlarmLevel.STOP, XAlarmColor.Red);
            alarmsColor.Add(XAlarmLevel.TIP, XAlarmColor.Red);
        }
        public void Stop()
        {
            if (this._thread != null)
            {
                this._thread.Abort();
            }
        }
        public void Start()
        {
            Stop();
            _thread = new Thread(new ThreadStart(ProcessEventQueue));
            _thread.IsBackground = true;
            _thread.Start();
        }

        private async void WarningHandleAsync(string warningCode, string warningInfo, int level)
        {
            await Task.Run(() =>
            {
                OnWarningChanged?.Invoke(warningCode, warningInfo, level);
                XTask.LogOutputHandleAsync($"{warningCode}:{warningInfo}");
            });
        }

        private void InitSystemAlarms()
        {
            systemAlarms.Add(XSysAlarmId.ESTOP, new XAlarmEventArgs((int)XSysAlarmId.ESTOP, SYSTEM, "紧急停止！"));
            systemAlarms.Add(XSysAlarmId.DOOR_OPEN, new XAlarmEventArgs((int)XSysAlarmId.DOOR_OPEN, SYSTEM, "触发门限！"));
            systemAlarms.Add(XSysAlarmId.CURTAIN_ACT, new XAlarmEventArgs((int)XSysAlarmId.CURTAIN_ACT, SYSTEM, "触发光幕！"));
            systemAlarms.Add(XSysAlarmId.AXIS_SERVON_FAIL, new XAlarmEventArgs((int)XSysAlarmId.AXIS_SERVON_FAIL, SYSTEM, "轴使能失败！"));
            systemAlarms.Add(XSysAlarmId.AXIS_ASTP, new XAlarmEventArgs((int)XSysAlarmId.AXIS_ASTP, SYSTEM, "轴异常停止！"));
            systemAlarms.Add(XSysAlarmId.AXIS_ALM, new XAlarmEventArgs((int)XSysAlarmId.AXIS_ALM, SYSTEM, "轴伺服报警！"));
            systemAlarms.Add(XSysAlarmId.AXIS_PEL, new XAlarmEventArgs((int)XSysAlarmId.AXIS_PEL, SYSTEM, "轴触发正极限！"));
            systemAlarms.Add(XSysAlarmId.AXIS_MEL, new XAlarmEventArgs((int)XSysAlarmId.AXIS_MEL, SYSTEM, "轴触发负极限！"));
            systemAlarms.Add(XSysAlarmId.AXIS_POSERROR, new XAlarmEventArgs((int)XSysAlarmId.AXIS_POSERROR, SYSTEM, "轴跟随误差过大！"));
            systemAlarms.Add(XSysAlarmId.WAITDI_TIMEOUT, new XAlarmEventArgs((int)XSysAlarmId.WAITDI_TIMEOUT, SYSTEM, "等待信号超时！"));
            systemAlarms.Add(XSysAlarmId.CARD_INIT_FAIL, new XAlarmEventArgs((int)XSysAlarmId.CARD_INIT_FAIL, SYSTEM, "板卡初始化失败！"));
            systemAlarms.Add(XSysAlarmId.CARD_LOAD_PARAM_FAIL, new XAlarmEventArgs((int)XSysAlarmId.CARD_LOAD_PARAM_FAIL, SYSTEM, "板卡加载参数失败！"));
        }

        public Dictionary<XSysAlarmId, XAlarmEventArgs> SystemAlarms
        {
            get { return this.systemAlarms; }
        }

        public XAlarmEventArgs CurrentAlarm
        {
            get { return currentAlarm; }
        }

        public void ClearAlarm()
        {
            if (IsAlarming)
            {
                if (OnAlarmCleared != null)
                {
                    OnAlarmCleared(this.currentAlarm);
                }
                IsAlarming = false;
            }
        }
        public void ClearAlarmForHive()
        {
            if (XAlarmReporter.Instance.AlarmingForHiveFlag)
            {
                if (OnAlarmClearedForHive != null)
                {
                    OnAlarmClearedForHive(this.currentAlarm);
                }
                XAlarmReporter.Instance.AlarmingForHiveFlag = false;
            }
        }

        public override int HandleEvent(XEvent xEvent)
        {
            try
            {
                int stationId = 0;
                int alarmId = 0;
                int alarmlevel = 0;
                string description = "";
                switch (xEvent.EventID)
                {
                    case XEventID.ALARM:
                        {
                            stationId = xEvent.EventArgs.StationId;
                            alarmId = xEvent.EventArgs.AlarmId;
                            alarmlevel = xEvent.EventArgs.AlarmLevel;
                            string append = "";
                            switch (alarmId)
                            {
                                case (int)XSysAlarmId.AXIS_SERVON_FAIL:
                                    append = "(AxisId:" + xEvent.EventArgs.IntValue + "[" +
                                        XDevice.Instance.FindAxisById(xEvent.EventArgs.IntValue).Name + "])";
                                    description = SystemAlarms[(XSysAlarmId)alarmId].Description + append;
                                    break;
                                case (int)XSysAlarmId.AXIS_ASTP:
                                    append = "(AxisId:" + xEvent.EventArgs.IntValue + "[" +
                                        XDevice.Instance.FindAxisById(xEvent.EventArgs.IntValue).Name + "])";
                                    description = SystemAlarms[(XSysAlarmId)alarmId].Description + append;
                                    break;
                                case (int)XSysAlarmId.AXIS_POSERROR:
                                    append = "(AxisId:" + xEvent.EventArgs.IntValue + "[" +
                                        XDevice.Instance.FindAxisById(xEvent.EventArgs.IntValue).Name + "])";
                                    description = SystemAlarms[(XSysAlarmId)alarmId].Description + append;
                                    break;
                                case (int)XSysAlarmId.AXIS_ALM:
                                    append = "(AxisId:" + xEvent.EventArgs.IntValue + "[" +
                                        XDevice.Instance.FindAxisById(xEvent.EventArgs.IntValue).Name + "])";
                                    description = SystemAlarms[(XSysAlarmId)alarmId].Description + append;
                                    break;
                                case (int)XSysAlarmId.AXIS_PEL:
                                    append = "(AxisId:" + xEvent.EventArgs.IntValue + "[" +
                                        XDevice.Instance.FindAxisById(xEvent.EventArgs.IntValue).Name + "])";
                                    description = SystemAlarms[(XSysAlarmId)alarmId].Description + append;
                                    break;
                                case (int)XSysAlarmId.AXIS_MEL:
                                    append = "(AxisId:" + xEvent.EventArgs.IntValue + "[" +
                                        XDevice.Instance.FindAxisById(xEvent.EventArgs.IntValue).Name + "])";
                                    description = SystemAlarms[(XSysAlarmId)alarmId].Description + append;
                                    break;
                                case (int)XSysAlarmId.WAITDI_TIMEOUT:
                                    string diName = XDevice.Instance.FindDiById(xEvent.EventArgs.IntValue).Name;
                                    string state = (xEvent.EventArgs.BoolValue) ? "ON" : "OFF";
                                    append = string.Format("(DiId:" + xEvent.EventArgs.IntValue + "等待["
                                        + diName + "]="
                                        + state + ")");
                                    description = SystemAlarms[(XSysAlarmId)alarmId].Description + append;
                                    break;
                                case 0:

                                    break;
                                default:
                                    append = xEvent.EventArgs.StringValue;
                                    description = SystemAlarms[(XSysAlarmId)alarmId].Description + append;
                                    break;
                            }

                        }
                        break;
                    case XEventID.ESTOP:
                        stationId = xEvent.EventArgs.StationId;
                        alarmId = (int)XSysAlarmId.ESTOP;
                        description = SystemAlarms[(XSysAlarmId)alarmId].Description;
                        alarmlevel = xEvent.EventArgs.AlarmLevel;
                        break;
                    case XEventID.LOG:
                        xEvent.EventHandler.HandleEvent(xEvent);
                        break;
                }

                if (xEvent.EventID != XEventID.LOG)
                {
                    if (alarmId == 0)
                    {
                        this.currentAlarm = xEvent.EventArgs.AlarmEventArgs;
                    }
                    else
                    {
                        this.currentAlarm = new XAlarmEventArgs(SystemAlarms[(XSysAlarmId)alarmId].Code, SystemAlarms[(XSysAlarmId)alarmId].Category, description);
                    }
                    this.currentAlarm.StationId = stationId;
                    this.currentAlarm.StartTime = DateTime.Now;
                    this.currentAlarm.AlarmLevel = alarmlevel;
                    if (xEvent.EventArgs.AlarmLevel != (int)XAlarmLevel.ONLYLOG)
                    {

                        if (OnSaveAlarmReport != null)
                        {
                            OnSaveAlarmReport(this.currentAlarm);
                            IsAlarming = true;
                        }
                        if (OnProductingPage != null || OnAlarmingPage != null)
                        {
                            WarningHandleAsync(this.currentAlarm.Code.ToString(), this.currentAlarm.Description, OnAlarmingPage != null ? 2 : 1);
                            //OnProductingPage(this.currentAlarm);
                            CloneToQueue(this.currentAlarm);
                        }
                        //if (OnAlarmingPage != null)
                        //{
                        //    OnAlarmingPage(this.currentAlarm);
                        //}
                    }
                    XLogger.Instance.WriteLine(
                            "Station" + this.currentAlarm.StationId
                          + " => AlarmCode:" + this.currentAlarm.Code.ToString()
                          + " => Category:" + this.currentAlarm.Category
                          + " => " + this.currentAlarm.Description);
                }
                return 0;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex, "");
                return -1;
            }
        }
        private void ProcessEventQueue()
        {
            while (true)
            {
                if (isProductingPageCreated)
                {
                    if (OnProductingPageQueue.Count > 0)
                    {
                        XAlarmEventArgs ProductingAlarm;
                        OnProductingPageQueue.TryDequeue(out ProductingAlarm);
                        OnProductingPage?.Invoke(ProductingAlarm);
                    }
                }
                if (isAlarmingPageCreated)
                {
                    if (OnAlarmingPageQueue.Count > 0)
                    {
                        XAlarmEventArgs Alarm;
                        if (OnAlarmingPageQueue.TryDequeue(out Alarm))
                        {
                            OnAlarmingPage(Alarm);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                Thread.Sleep(10);
            }
        }
        private void CloneToQueue(XAlarmEventArgs HistoryAlarm)
        {
            XAlarmEventArgs QueueAlarm = new XAlarmEventArgs(0, "NONE", "NONE");
            QueueAlarm.AlarmLevel = HistoryAlarm.AlarmLevel;
            QueueAlarm.Category = HistoryAlarm.Category;
            QueueAlarm.Code = HistoryAlarm.Code;
            QueueAlarm.Description = HistoryAlarm.Description;
            QueueAlarm.Duration = HistoryAlarm.Duration;
            QueueAlarm.StartTime = HistoryAlarm.StartTime;
            QueueAlarm.StationId = HistoryAlarm.StationId;
            OnAlarmingPageQueue.Enqueue(QueueAlarm);
            OnProductingPageQueue.Enqueue(QueueAlarm);
        }

    }

    public enum XSysAlarmId
    {
        ESTOP = 1,
        DOOR_OPEN = 2,
        CURTAIN_ACT = 3,
        AXIS_SERVON_FAIL = 4,
        AXIS_ASTP = 5,
        AXIS_ALM = 6,
        AXIS_PEL = 7,
        AXIS_MEL = 8,
        AXIS_POSERROR = 9,
        WAITDI_TIMEOUT = 10,
        CARD_INIT_FAIL = 11,
        CARD_LOAD_PARAM_FAIL = 12,
        CARD_REST_FAIL = 13,
        CARD_ERROR_FUNGETBACK = 14,
        RobortAlarm,
        RobortProgramError,
        RST,
    }

    public enum XAlarmLevel
    {
        TIP,
        PAUSE,
        STOP,
        ONLYLOG
    }
    public enum XAlarmColor
    {
        Red,
        Yellow,
        Green
    }
    public enum AlarmCategory
    {
        VISION,
        LASER,
        TRAY,
        PLC,
        SYSTEM,
        DI,
        DO,
        Robort

    }
}
