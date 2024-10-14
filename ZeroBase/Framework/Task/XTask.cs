using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using InfoShow;
using System.Threading.Tasks;

namespace ZeroBase
{
    public delegate void OnLogRaised(string msg);

    public delegate void OnWarningChangedTaskDelegate(string warningCode, string warningInfo, int level = 2);
    public abstract class XTask : XTaskEventHandler
    {
        public event Action<string, Color> OnStep;
        public event Action<int, int> OnCount;
        private Thread _thread;
        private XMove xMove;
        private XSetDo xSetDo;
        private XWaitDi xWaitDi;
        private XStation xStation;
        private Dictionary<int, IAxis> axisMap = new Dictionary<int, IAxis>();
        private Dictionary<int, IAxis> positionTableAxisMap = new Dictionary<int, IAxis>();
        private Dictionary<int, XDo> doMap = new Dictionary<int, XDo>();
        private Dictionary<int, XDi> diMap = new Dictionary<int, XDi>();
        private int stationId;
        private string logpath;
        private XCommandRobot XRobortStyle;
        public bool IsAutuRunNeedHome = false;

        public XTask()
        {
            Z_PositionAxisIdIndex = -1;

        }
        public XTask(string logpath)
            : this()
        {
            this.logpath = logpath;
        }
        public XCommandRobot XRobortCommand
        {
            get { return XRobortStyle; }
        }
        public string LogPath
        {
            get { return logpath; }
            set { logpath = value; }
        }
        public string SetStepStr { get; set; }
        public int TaskId { get; set; }

        public string Name { get; set; }

        public bool TaskHomeOK { get; set; }

        public int StationId
        {
            get { return this.stationId; }
            set
            {
                this.stationId = value;
                xStation = XStationManager.Instance.FindStationById(stationId);
                xMove = new XMove(xStation);
                xSetDo = new XSetDo(xStation);
                xWaitDi = new XWaitDi(xStation);
            }
        }

        public async void SetStep(string step, Color color, bool flag = false)
        {
            await Task.Run(() =>
            {
                LogOutputHandleAsync(step);

                if (flag)
                    OnWarningTaskChanged?.Invoke("异常:", step, 1);

                SetStepStr = step;
                if (OnStep != null)
                {
                    OnStep(step, color);
                }
                WriteLog(step);
            });



        }

        public static async void LogOutputHandleAsync(string msg)
        {
            await Task.Run(() =>
            {
                LogRaised?.Invoke(msg);
            });
        }

        public void SetCount(int currentnum, int total)
        {
            if (OnCount != null)
            {
                OnCount(currentnum, total);
            }
        }
        public double Z_Safe { get; set; }

        public int Z_PositionAxisIdIndex { get; set; }

        public static Action<string> LogOutPut;

        public static event OnLogRaised LogRaised;

        public static event OnWarningChangedTaskDelegate OnWarningTaskChanged;

        #region Device

        public void RegisterAxis(int axisSetId, bool IsShownInPositionTable = true)
        {
            if (axisMap.ContainsKey(axisSetId))
            {
                return;
            }
            axisMap.Add(axisSetId, XDevice.Instance.FindAxisById(axisSetId));
            XDevice.Instance.FindAxisById(axisSetId).TaskId = TaskId;

            if (IsShownInPositionTable)
            {
                if (positionTableAxisMap.ContainsKey(axisSetId))
                {
                    return;
                }
                positionTableAxisMap.Add(axisSetId, XDevice.Instance.FindAxisById(axisSetId));
            }
        }

        public void RegisterDo(int doSetId)
        {
            if (doMap.ContainsKey(doSetId))
            {
                return;
            }
            doMap.Add(doSetId, XDevice.Instance.FindDoById(doSetId));
            XDevice.Instance.FindDoById(doSetId).TaskId = TaskId;
        }

        public void RegisterDi(int diSetId)
        {
            if (diMap.ContainsKey(diSetId))
            {
                return;
            }
            diMap.Add(diSetId, XDevice.Instance.FindDiById(diSetId));
            XDevice.Instance.FindDiById(diSetId).TaskId = TaskId;
        }

        public Dictionary<int, IAxis> AxisMap
        {
            get { return this.axisMap; }
        }

        public Dictionary<int, IAxis> PositionTableAxisMap
        {
            get { return this.positionTableAxisMap; }
        }

        public Dictionary<int, XDo> DoMap
        {
            get { return this.doMap; }
        }

        public Dictionary<int, XDi> DiMap
        {
            get { return this.diMap; }
        }

        #endregion


        #region HandleEvent

        public override int HandleEvent(XEvent xEvent)
        {
            xMove.HandleEvent(xEvent);
            xSetDo.HandleEvent(xEvent);
            xWaitDi.HandleEvent(xEvent);

            return 0;
        }

        #endregion


        /// <summary>
        /// 启动，调用Running
        /// </summary>
        public void Start(object runMode)
        {
            if (_thread != null)
            {
                _thread.Abort();
            }

            _thread = new Thread(new ParameterizedThreadStart(Running));
            _thread.IsBackground = true;
            _thread.Start(runMode);
        }
        /// <summary>
        /// 复位，调用Homing
        /// </summary>
        public void Reset()
        {
            IsAutuRunNeedHome = false;
            if (_thread != null)
            {
                _thread.Abort();
            }
            _thread = new Thread(new ThreadStart(Homing));
            _thread.IsBackground = true;
            _thread.Start();
        }
        /// <summary>
        /// 任务线程取消
        /// </summary>
        public void Cancel()
        {
            TaskStop();
            if (_thread != null)
            {
                _thread.Abort();
            }
        }

        /// <summary>
        /// 任务初始化
        /// </summary>
        public virtual void Initialize()
        {

        }
        public virtual void WriteLog(string message)
        {

        }
        /// <summary>
        /// 任务退出
        /// </summary>
        public virtual void Exit()
        {
            Cancel();
        }
        /// <summary>
        /// 任务运行，需用户重写
        /// </summary>
        protected virtual void Running(object runMode) { }

        /// <summary>
        /// 任务复位，需用户重写
        /// </summary>
        protected virtual void Homing() { }

        protected virtual void TaskStop() { }

        #region 任务所在工站的相应操作

        /// <summary>
        /// 设置工站的状态：等待运行，复位完成后调用
        /// 对应用户控件XStationStateBar显示
        /// </summary>
        protected void SetStation_StateWaitRun()
        {
            xStation.SetState(XStationState.WAITRUN);
        }
        protected void SetStation_StateWAITRESET()
        {
            xStation.SetState(XStationState.WAITRESET);
        }
        #region 秒表，用以记录Task所在Station的CycleTime，开始和停止需用户操作
        /// <summary>
        /// 秒表开始
        /// </summary>
        protected void SetStation_StopWatchStart()
        {
            xStation.StopWatch_Start();
        }
        /// <summary>
        /// 秒表复位
        /// </summary>
        protected void SetStation_StopWatchReset()
        {
            xStation.StopWatch_Reset();
        }
        /// <summary>
        /// 秒表停止
        /// </summary>
        protected void SetStation_StopWatchStop()
        {
            xStation.StopWatch_Stop();
        }
        /// <summary>
        /// 秒表重启动
        /// </summary>
        protected void SetStation_StopWatchRestart()
        {
            xStation.StopWatch_ReStart();
        }
        /// <summary>
        /// 获取秒表开始后的毫秒数
        /// </summary>
        /// <returns></returns>
        protected double GetStation_StopWatchElapsedMilliseconds()
        {
            return xStation.ElapsedMilliseconds;
        }

        #endregion

        #endregion

        #region 发送告警

        private XAlarmEventArgs _alarmEventArgs = new XAlarmEventArgs(Int32.MinValue, "NONE", "NONE");
        /// <summary>
        /// 发送告警
        /// </summary>
        /// <param name="alarmLevel">报警等级</param>
        /// <param name="alarmCode">报警代号</param>
        /// <param name="append">用户自定义的额外报警信息</param>
        private void PostAlarm(XAlarmLevel alarmLevel, XAlarmEventArgs args, string append = "")
        {
            PostEvent(xStation, alarmLevel, args, append);
        }
        //ReportAlarm(XAlarmLevel.TIP, (int)AlarmCode.获取前站数据失败, AlarmCategory.TRAY.ToString(),
        //                            AlarmCode.获取前站数据失败.ToString(), "PAM1");
        protected void ReportAlarm(XAlarmLevel level, int code, string category, string description, string append = "")
        {
            _alarmEventArgs = new XAlarmEventArgs(Int32.MinValue, "NONE", "NONE");
            _alarmEventArgs.Code = code;
            _alarmEventArgs.Category = category;
            _alarmEventArgs.Description = description;
            PostAlarm(level, _alarmEventArgs, append);
        }
        protected void ClearAlarm()
        {
            //if (this.stationId == XAlarmReporter.Instance.CurrentAlarm.StationId)
            {
                XAlarmReporter.Instance.ClearAlarm();
            }
        }

        #endregion

        #region 轴操作

        /// <summary>
        /// 从对应的PositionTable获取点位
        /// </summary>
        /// <param name="positionName"></param>
        /// <returns></returns>
        public XPosition GetPositon(string positionName)
        {
            try
            {
                var ct = XTaskManager.Instance.FindTaskById(TaskId).PositionTableAxisMap.Keys.ToArray();
                double[] pos = new double[ct.Length];
                XPosition position = new XPosition(ct, pos, ct.Length);
                var ret = XPositionManager.Instance.PositionSet[TaskId].CheckExist(positionName, position);
                if (ret == false)
                {
                    //if (DialogResult.OK == MessageBox.Show("配置文件中不存在点位：" + positionName + "  点击确认 增加点位", "确认？", MessageBoxButtons.OKCancel))
                    //{
                    //    FrmPostion frm = new FrmPostion(TaskId, positionName);

                    //    frm.ShowDialog();
                    //    XPositionManager.Instance.PositionSet[TaskId].Add(positionName, position);
                    //    frm.Close();
                    //}

                }
                return XPositionManager.Instance.PositionSet[TaskId].PositionMap[positionName];
            }
            catch (Exception ex)
            {
                LogOutputHandleAsync($"{ex.Message}\nStackTrace:{ex.StackTrace}");
                //MessageBox.Show(ex.Message, "异常");
                //MessageManager.gOnly.Message(ex.Message, MessageType.Alarm);
                return null;
            }

        }
        /// <summary>
        /// 多轴使能
        /// </summary>
        /// <param name="axisId"></param>
        /// <param name="sts"></param>
        /// <returns></returns>
        protected int SetServo(int[] axisId, bool sts)
        {
            int ret;
            for (int i = 0; i < axisId.Length; i++)
            {
                ret = XDevice.Instance.FindAxisById(axisId[i]).SetServo(sts);
                if (ret != 0)
                {
                    return -1;
                }
            }
            return 0;

        }
        /// <summary>
        /// 单轴使能
        /// </summary>
        /// <param name="axisId"></param>
        /// <param name="sts"></param>
        /// <returns></returns>
        protected int SetServo(int axisId, bool sts)
        {
            return XDevice.Instance.FindAxisById(axisId).SetServo(sts);
        }
        /// <summary>
        /// 多轴回零
        /// </summary>
        /// <param name="axisId"></param>
        /// <returns></returns>
        protected int MoveHome(int[] axisId)
        {
            return xMove.MoveHome(axisId, axisId.Length);
        }
        /// <summary>
        /// 单轴回零
        /// </summary>
        /// <param name="axisId"></param>
        /// <returns></returns>
        protected int MoveHome(int axisId)
        {
            return xMove.MoveHome(new int[] { axisId }, 1);
        }
        /// <summary>
        /// 多轴绝对运动，不同速度
        /// </summary>
        /// <param name="axisId"></param>
        /// <param name="pos"></param>
        /// <param name="vel"></param>
        /// <returns></returns>
        protected int MoveAbs(int[] axisId, double[] pos, double[] vel, bool checkLmt = true)
        {
            return xMove.MoveAbs(axisId, pos, vel, axisId.Length, checkLmt);
        }
        /// <summary>
        /// 单轴绝对运动
        /// </summary>
        /// <param name="axisId"></param>
        /// <param name="pos"></param>
        /// <param name="vel"></param>
        /// <returns></returns>
        protected int MoveAbs(int axisId, double pos, double vel, bool checkLmt = true)
        {
            return xMove.MoveAbs(new int[] { axisId }, new double[] { pos }, new double[] { vel }, 1, checkLmt);
        }
        /// <summary>
        /// 多轴绝对运动，相同速度
        /// </summary>
        /// <param name="axisId"></param>
        /// <param name="pos"></param>
        /// <param name="vel"></param>
        /// <returns></returns>
        protected int MoveAbs(int[] axisId, double[] pos, double vel, bool checkLmt = true)
        {
            double[] vels = new double[axisId.Length];
            for (int i = 0; i < axisId.Length; i++)
            {
                vels[i] = vel;
            }
            return xMove.MoveAbs(axisId, pos, vels, axisId.Length, checkLmt);
        }
        /// <summary>
        /// 多轴根据示教点位运动，不同速度
        /// </summary>
        /// <param name="position"></param>
        /// <param name="vel"></param>
        /// <returns></returns>
        protected int MovePosition(XPosition position, double[] vel, bool checkLmt = true)
        {
            int[] axisId = position.AxisId;
            double[] pos = position.Positions;
            return xMove.MoveAbs(axisId, pos, vel, position.Count, checkLmt);
        }
        /// <summary>
        /// 多轴根据示教点位运动，相同速度
        /// </summary>
        /// <param name="position"></param>
        /// <param name="vel"></param>
        /// <returns></returns>
        protected int MovePosition(XPosition position, double vel, bool checkLmt = true)
        {
            int[] axisId = position.AxisId;
            double[] pos = position.Positions;
            int count = position.Count;
            double[] vels = new double[count];
            for (int i = 0; i < count; i++)
            {
                vels[i] = vel;
            }
            return xMove.MoveAbs(axisId, pos, vels, count, checkLmt);
        }

        protected int SetPosition(int axisId, int position)
        {
            return XDevice.Instance.FindAxisById((ushort)axisId).SetPosition(position);
        }
        protected double GetPosition(int axisId, bool IsCmd)
        {
            return IsCmd ? XDevice.Instance.FindAxisById((ushort)axisId).CommandPOS : XDevice.Instance.FindAxisById((ushort)axisId).POS;
        }

        protected int CleanALM(int axisId)
        {
            return XDevice.Instance.FindAxisById((ushort)axisId).CleanALM();
        }

        protected bool CheckMoveDone(ushort actCardId, ushort axisId)
        {
            return XDevice.Instance.FindCardById(actCardId).CheckMoveDone(axisId) == 0 ? true : false;
        }
        /// <summary>
        /// 多轴相对运动，不同速度
        /// </summary>
        /// <param name="axisId"></param>
        /// <param name="distance"></param>
        /// <param name="vel"></param>
        /// <returns></returns>
        protected int MoveRel(int[] axisId, double[] distance, double[] vel, bool checkLmt = true)
        {
            return xMove.MoveRel(axisId, distance, vel, axisId.Length, checkLmt);
        }
        /// <summary>
        /// 单轴相对运动
        /// </summary>
        /// <param name="axisId"></param>
        /// <param name="distance"></param>
        /// <param name="vel"></param>
        /// <returns></returns>
        protected int MoveRel(int axisId, double distance, double vel, bool checkLmt = true)
        {
            return xMove.MoveRel(new int[] { axisId }, new double[] { distance }, new double[] { vel }, 1, checkLmt);
        }
        /// <summary>
        /// 单轴JOG运动
        /// </summary>
        /// <param name="axisId"></param>
        /// <param name="distance"></param>
        /// <param name="vel"></param>
        /// <returns></returns>
        protected int MoveJog(int axisId, int isStart, bool checkLmt = true)
        {
            return xMove.MoveJog(new int[] { axisId }, new int[] { isStart }, 1, checkLmt);
        }
        /// <summary>
        /// 多轴相对运动，相同速度
        /// </summary>
        /// <param name="axisId"></param>
        /// <param name="distance"></param>
        /// <param name="vel"></param>
        /// <returns></returns>
        protected int MoveRel(int[] axisId, double[] distance, double vel, bool checkLmt = true)
        {
            double[] vels = new double[axisId.Length];
            for (int i = 0; i < axisId.Length; i++)
            {
                vels[i] = vel;
            }
            return xMove.MoveRel(axisId, distance, vels, axisId.Length, checkLmt);
        }

        /// <summary>
        /// 运动停止
        /// </summary>
        /// <returns></returns>
        protected int MoveStop()
        {
            return xMove.MoveStop();
        }
        /// <summary>
        /// 判断运动是否完成，每次运动时必须调用
        /// </summary>
        /// <returns></returns>
        protected bool WaitMoveDone()
        {
            return xMove.WaitEvent(-1) == 0 ? true : false;
        }

        protected int SetAxisAccAndDec(int axisId, double acc, double dec)
        {
            return XDevice.Instance.FindAxisById(axisId).SetAxisAccAndDec(acc, dec);
        }
        protected int SetStopDec(int axisId, double dec)
        {
            return XDevice.Instance.FindAxisById(axisId).SetStopDec(dec);
        }
        protected int SetAxisJogAccAndDec(int axisId, int mode, int dir, double acc, double dec, int vel)
        {
            return XDevice.Instance.FindAxisById(axisId).APS_SetAxisJogParam(mode, dir, acc, dec, vel);
        }

        protected bool SetLimit(int actCardId, int axisId,int pos)
        {
            return XDevice.Instance.FindCardById(actCardId).SetLimit(axisId,pos);
        }
        #endregion

        #region Do操作

        /// <summary>
        /// 设置多个Do状态，不同状态
        /// </summary>
        /// <param name="doId"></param>
        /// <param name="doStsType"></param>
        /// <returns></returns>
        public int SetDo(int[] doId, DOSTSTYPE[] doStsType)
        {
            return xSetDo.SetDo(doId, doStsType, doId.Length);
        }
        /// <summary>
        /// 设置单个Do状态
        /// </summary>
        /// <param name="doId"></param>
        /// <param name="doStsType"></param>
        /// <returns></returns>
        public int SetDo(int doId, DOSTSTYPE doStsType)
        {
            return xSetDo.SetDo(new int[] { doId }, new DOSTSTYPE[] { doStsType }, 1);
        }
        /// <summary>
        /// 设置多个Do状态，相同状态
        /// </summary>
        /// <param name="doId"></param>
        /// <param name="doStsType"></param>
        /// <returns></returns>
        public int SetDo(int[] doId, DOSTSTYPE doStsType)
        {
            DOSTSTYPE[] types = new DOSTSTYPE[doId.Length];
            for (int i = 0; i < doId.Length; i++)
            {
                types[i] = doStsType;
            }
            return xSetDo.SetDo(doId, types, doId.Length);
        }

        #endregion

        #region Di操作

        public bool GetDi(int diId)
        {
            return XDevice.Instance.FindDiById(diId).STS;
        }

        /// <summary>
        /// 等待多个Di信号，不同状态，超时后暂停或停止
        /// </summary>
        /// <param name="diId"></param>
        /// <param name="diStsType"></param>
        /// <param name="timeout">超时时间，单位毫秒</param>
        /// <param name="timeoutToBeContinue">若为true，超时后停止，必须复位；若为false，超时后暂停，可继续</param>
        /// <returns></returns>
        public bool WaitDi(int[] diId, DISTSTYPE[] diStsType, int timeout, bool timeoutToBeContinue = false, string append = "")
        {
            return xWaitDi.WaitDi(diId, diStsType, diId.Length, timeout, timeoutToBeContinue, append) == 0 ? true : false;
        }
        /// <summary>
        /// 等待多个Di信号，超时后暂停或停止
        /// </summary>
        /// <param name="diId"></param>
        /// <param name="diStsType"></param>
        /// <param name="timeout">超时时间，单位毫秒</param>
        /// <param name="timeoutToBeContinue">若为false，超时后停止，必须复位；若为true，超时后暂停，可继续</param>
        /// <returns></returns>
        public bool WaitDi(int diId, DISTSTYPE diStsType, int timeout, bool timeoutToBeContinue = false, string append = "")
        {
            return xWaitDi.WaitDi(new int[] { diId }, new DISTSTYPE[] { diStsType }, 1, timeout, timeoutToBeContinue, append) == 0 ? true : false;
        }
        /// <summary>
        /// 等待多个Di信号，相同状态，超时后暂停或停止
        /// </summary>
        /// <param name="diId"></param>
        /// <param name="diStsType"></param>
        /// <param name="timeout">超时时间，单位毫秒</param>
        /// <param name="timeoutToBeContinue">若为true，超时后停止，必须复位；若为false，超时后暂停，可继续</param>
        /// <returns></returns>
        public bool WaitDi(int[] diId, DISTSTYPE diStsType, int timeout, bool timeoutToBeContinue = false, string append = "")
        {
            DISTSTYPE[] types = new DISTSTYPE[diId.Length];
            for (int i = 0; i < diId.Length; i++)
            {
                types[i] = diStsType;
            }
            return xWaitDi.WaitDi(diId, types, diId.Length, timeout, timeoutToBeContinue, append) == 0 ? true : false;
        }
        /// <summary>
        /// 等待多个Di信号，不同状态，超时后无动作
        /// </summary>
        /// <param name="diId"></param>
        /// <param name="diStsType"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool WaitDiSignal(int[] diId, DISTSTYPE[] diStsType, int timeout)
        {
            return xWaitDi.WaitDiSignal(diId, diStsType, diId.Length, timeout) == 0 ? true : false;
        }
        /// <summary>
        /// 等待单个Di信号，超时后无动作
        /// </summary>
        /// <param name="diId"></param>
        /// <param name="diStsType"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool WaitDiSignal(int diId, DISTSTYPE diStsType, int timeout)
        {
            return xWaitDi.WaitDiSignal(new int[] { diId }, new DISTSTYPE[] { diStsType }, 1, timeout) == 0 ? true : false;
        }
        /// <summary>
        /// 等待多个Di信号，相同状态，超时后无动作
        /// </summary>
        /// <param name="diId"></param>
        /// <param name="diStsType"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool WaitDiSignal(int[] diId, DISTSTYPE diStsType, int timeout)
        {
            DISTSTYPE[] types = new DISTSTYPE[diId.Length];
            for (int i = 0; i < diId.Length; i++)
            {
                types[i] = diStsType;
            }
            return xWaitDi.WaitDiSignal(diId, types, diId.Length, timeout) == 0 ? true : false;
        }

        #endregion

        #region Robort操作
        public void RegisterRobort(XCommandRobot command)
        {
            XRobortStyle = command;
        }
        public int RobortConnect()
        {
            if (XRobortStyle != null)
            {
                RobortSatrtByIO();
                Thread.Sleep(200);
                RobortResetByIO();
                Thread.Sleep(200);
                return XRobortStyle.InitialConnect();
            }
            return -1;
        }
        public int RobortDisConnect()
        {
            if (XRobortStyle != null)
            {
                XRobortStyle.ServoOffByIO();
                return XRobortStyle.DisConnect();
            }
            return -1;
        }
        public bool RobotConnected
        {
            get { return XRobortStyle.RobotConnected; }
        }
        public bool RobotAlarm
        {
            get { return XRobortStyle.Alarm; }
        }
        public void RobortStopByIO()
        {
            if (XRobortStyle != null)
            {
                XRobortStyle.StopByIO();
            }
        }
        public void RobortResetByIO()
        {
            if (XRobortStyle != null)
            {
                XRobortStyle.ResetByIO();
            }
        }
        public void RobortSatrtByIO()
        {
            if (XRobortStyle != null)
            {
                XRobortStyle.SatrtByIO();
            }
        }
        public void RobortServoOffByIO()
        {
            XRobortStyle.ServoOffByIO();
        }
        public void RobortServoOnByIO()
        {
            XRobortStyle.ServoOnByIO();
        }
        public void RobortAuthorizationByIO(int value)
        {
            XRobortStyle.AuthorizationByIO(value);
        }
        public void RobortPauseOn()
        {
            if (XRobortStyle != null)
            {
                XRobortStyle.PauseOn();
            }
        }
        //继续
        public void RobortGoOn()
        {
            if (XRobortStyle != null)
            {
                XRobortStyle.GoOn();
            }
        }

        public void RestartControler()
        {
            if (XRobortStyle != null)
            {
                XRobortStyle.RestartContorlerByCommand();
            }
        }
        /// <summary>
        /// 获取机械手坐标
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool GetRobotPos(out double[] pos)
        {
            pos = new double[6];
            return XRobortStyle.GetRobotPos(out pos);

        }
        /// <summary>
        /// 单轴直线插补运动
        /// </summary>
        /// <param name="type"></param>
        /// <param name="rel">juli</param>
        /// <param name="vel"></param>
        /// <param name="waitMotionDone"></param>
        /// <returns></returns>
        /// 
        public bool RobortMovRel(AxisType type, double rel, double vel, bool waitMotionDone = true)
        {
            return XRobortStyle.MovRel(type, rel, vel, waitMotionDone);
        }
        /// <summary>
        /// 以直线插补动作从现在位置到移动目的位置为止 。(直线)
        /// </summary>
        /// <param name="tarPostion">目标位置</param>
        /// <param name="vel">速度</param>
        /// <param name="timeout"></param>
        /// <param name="waitMotionDone"></param>
        /// <returns></returns>
        public bool RobortMovL(double[] tarPostion, int vel, int timeout, bool waitMotionDone = true)
        {
            return XRobortStyle.MovL(tarPostion, vel, timeout, waitMotionDone);
        }
        /// <summary>
        /// 从现在位置到移动目的位置 执行关节插补位置。(S形)
        /// </summary>
        /// <param name="tarPostion"></param>
        /// <param name="vel"></param>
        /// <param name="timeout"></param>
        /// <param name="waitMotionDone"></param>
        /// <returns></returns>
        public bool RobortMovS(double[] tarPostion, int vel, int timeout, bool waitMotionDone = true)
        {
            return XRobortStyle.MovS(tarPostion, vel, timeout, waitMotionDone);
        }
        /// <summary>
        /// 走门型直线运动
        /// </summary>
        /// <param name="tarPostion"></param>
        /// <param name="vel"></param>
        /// <param name="timeout"></param>
        /// <param name="waitMotionDone"></param>
        /// <param name="limtZ"></param>
        /// <returns></returns>
        public bool RobortJumpL(double[] tarPostion, int vel, int timeout, bool waitMotionDone = true, double limtZ = -70)
        {
            return XRobortStyle.JumpL(tarPostion, vel, timeout, waitMotionDone, limtZ);
        }
        /// <summary>
        /// 走门型弧线运动
        /// </summary>
        /// <param name="tarPostion"></param>
        /// <param name="vel"></param>
        /// <param name="timeout"></param>
        /// <param name="waitMotionDone"></param>
        /// <param name="limtZ"></param>
        /// <returns></returns>
        public bool RobortJumpS(double[] tarPostion, int vel, int timeout, bool waitMotionDone = true, double limtZ = 200)
        {
            return XRobortStyle.JumpS(tarPostion, vel, timeout, waitMotionDone, limtZ);
        }

        public void RobortSetDOStata(int Doid, DOSTSTYPE stata)
        {
            if (XRobortStyle != null)
            {
                XRobortStyle.SetDOSts(Doid, (DOSTSTYPE)stata);
            }
        }

        public void RobortGetDiStata(int Diid, ref DOSTSTYPE stata)
        {
            if (XRobortStyle != null)
            {
                //XRobortStyle.SetDOSts(Doid, (DOSTSTYPE)stata);
            }
        }

        public bool RobortWaitDISts(int timeout, int Diid, DISTSTYPE diSts)
        {
            return XRobortStyle.WaitDISts(timeout, Diid, diSts);
        }
        public void RobortSetDOAndWaitDI(int timeout, int doId, DOSTSTYPE doSts, int diId, DISTSTYPE diSts)
        {
            if (XRobortStyle != null)
            {
                XRobortStyle.SetDoAndWaitDi(timeout, doId, doSts, diId, diSts);
            }
        }
        #endregion
        protected int APS_pt_start(int[] axisId, int axisCount, int ptbId, int ptbCount)
        {
            return xMove.APS_pt_start(axisId, axisCount, ptbId, ptbCount);
        }


    }

}
