using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace XCore
{
    public struct RobotIo //仅局限于机械手与板卡直接相互交互的信号，使用前要先把对应在板卡的ID赋给各个对应的
    {
        public int DoStart;
        public int DoPause;
        public int DoStop;
        public int DoReset;//出错复位
        public int DoResetProgram;//程序出错复位
        public int DoAuthorization;//操作权限
        public int DoServo;//
        public int DoContiue;
        public int DiRuning;//机械手运行中
        public int DiAlarm;//报警中
        public int DiAuthorization;//操作权限制
        public int DiProgramRunnig;//机械手程序运行中
    }
    public class RobortInformation
    {
        public int RobortId { get; set; }
        public int TaskId { get; set; }
        public int StationId { get; set; }
        public XDi Di_Alarm { get; set; }
        public XDi Di_ProgramRunnig { get; set; }
        public string RobortName { get; set; }
        public string ErrNo { get; set; }
        public AutoResetEvent RobortWaitErrorMessage = new AutoResetEvent(false);
        public XAlarmEventArgs _alarmEventArgs { get; set; }
    }

    //CCD控制器
    public class XRobotControler
    {
        private int taskId;
        private string name;
        private bool connected;
        private RobotIo io;

        private string ipAddress;
        private int portNo;
        private bool failShowDlg;
        private int retryTimes;

        private XCommandRobot command;
        private Dictionary<int, XRobortDi> robortdimap = new Dictionary<int, XRobortDi>();
        public Dictionary<int, XRobortDi> RobortDiMap
        {
            get { return robortdimap; }
        }
        private Dictionary<int, XRobortDo> robortdomap = new Dictionary<int, XRobortDo>();
        public Dictionary<int, XRobortDo> RobortDoMap
        {
            get { return robortdomap; }
        }
        public XRobotControler(int robortid,int taskid, XCommandRobot command, string name, string ipAddress, int portNo, RobotIo io, bool captureFailShowDlg = true, int retryTimes = 5)
        {
            this.taskId = taskid;
            this.command = command;
            this.name = name;
            this.ipAddress = ipAddress;
            this.portNo = portNo;
            command.ip = ipAddress;
            command.Port = portNo;
            command.RobortID = robortid;
            this.failShowDlg = captureFailShowDlg;
            this.retryTimes = retryTimes;
            this.failShowDlg = captureFailShowDlg;
            this.io = io;
        }


        #region 属性
        /// <summary>
        /// 保存点位文件的TaskID
        /// </summary>
        public int TaskId
        {
            get
            {
                return taskId;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public XCommandRobot Command
        {
            get
            {
                return command;
            }
        }


        public RobotIo RobotIO
        {
            get
            {
                return io;
            }
        }

        #endregion
        public void RegisterRobortDi(int setid)
        {
            if (robortdimap.ContainsKey(setid))
            {
                return;
            }
            else
            {
                robortdimap.Add(setid, RobotManger.Instance.FindRobotDiById(setid));
                RobotManger.Instance.FindRobotDiById(setid).TaskId = taskId;
            }
        }
        public void RegisterRobortDo(int setid)
        {
            if (robortdomap.ContainsKey(setid))
            {
                return;
            }
            else
            {
                robortdomap.Add(setid, RobotManger.Instance.FindRobotDoById(setid));
                RobotManger.Instance.FindRobotDoById(setid).TaskId = taskId;
            }
        }
        //#region IO 操作
        //public void StopByIO()
        //{
        //    command.StopByIO();
        //}
        //public void ResetByIO()
        //{
        //    command.ResetByIO();
        //}
        //public void SatrtByIO()
        //{
        //    command.SatrtByIO();
        //}
        ////断使能
        //public void ServoOffByIO()
        //{
        //    command.ServoOffByIO();
        //}
        ////操作权
        //public void AuthorizationByIO(int value)
        //{
        //    command.AuthorizationByIO(value);
        //}

        ////暂停
        //public void PauseOn()
        //{
        //    command.PauseOn();
        //}
        ////继续
        //public void GoOn()
        //{
        //    command.GoOn();
        //}


        //#endregion


        ///// <summary>
        ///// 初始化TCP连接
        ///// </summary>
        ///// <returns></returns>
        //public int InitialConnect()
        //{
        //    connected = false;
        //    int iRtn = command.InitialConnect(ipAddress, portNo);
        //    connected = true;
        //    return iRtn;
        //}

        ///// <summary>
        ///// 获取机械手直交坐标
        ///// </summary>
        ///// <param name="pos"></param>
        ///// <returns></returns>
        //public bool GetPos(out double[] pos)
        //{
        //    return command.GetRobotPos(out pos);
        //}

        ///// <summary>
        ///// 单轴直线插补运动
        ///// </summary>
        ///// <param name="type"></param>
        ///// <param name="rel"></param>
        ///// <param name="vel"></param>
        ///// <param name="waitMotionDone"></param>
        ///// <returns></returns>
        //public bool MovRel(AxisType type, double rel, double vel, bool waitMotionDone = true)
        //{
        //    return command.MovRel(type, rel, vel, waitMotionDone);
        //}



        ///// <summary>
        ///// 以直线插补动作从现在位置到移动目的位置为止 。(直线)
        ///// </summary>
        ///// <param name="tarPostion">目标位置</param>
        ///// <param name="vel">速度</param>
        ///// <param name="timeout"></param>
        ///// <param name="waitMotionDone"></param>
        ///// <returns></returns>
        //public bool MvsL(double[] tarPostion, int vel, int timeout=30000, bool waitMotionDone = true)
        //{
        //    return command.MvsL(tarPostion, vel, timeout, waitMotionDone);
        //}



        ///// <summary>
        ///// 从现在位置到移动目的位置 执行关节插补位置。(S形)
        ///// </summary>
        ///// <param name="tarPostion"></param>
        ///// <param name="vel"></param>
        ///// <param name="timeout"></param>
        ///// <param name="waitMotionDone"></param>
        ///// <returns></returns>
        //public bool MovS(double[] tarPostion, int vel, int timeout=30000, bool waitMotionDone = true)
        //{
        //    return command.MovS(tarPostion, vel, timeout, waitMotionDone);
        //}


        ///// <summary>
        ///// 走门型直线运动
        ///// </summary>
        ///// <param name="tarPostion"></param>
        ///// <param name="vel"></param>
        ///// <param name="timeout"></param>
        ///// <param name="waitMotionDone"></param>
        ///// <param name="limtZ"></param>
        ///// <returns></returns>
        //public bool JumpL(double[] tarPostion, int vel, int timeout=30000, bool waitMotionDone = true,double limtZ = 140)
        //{
        //    return command.JumpL(tarPostion, vel, timeout,waitMotionDone, limtZ);

        //}

        ///// <summary>
        ///// 走门型弧线运动
        ///// </summary>
        ///// <param name="tarPostion"></param>
        ///// <param name="vel"></param>
        ///// <param name="timeout"></param>
        ///// <param name="waitMotionDone"></param>
        ///// <param name="limtZ"></param>
        ///// <returns></returns>
        //public bool JumpS(double[] tarPostion, int vel, int timeout=30000, bool waitMotionDone = true,double limtZ = 140)
        //{
        //    return command.JumpS(tarPostion, vel, timeout, waitMotionDone,limtZ);
        //}


        ///// <summary>
        ///// 发送 机械手要输出的DO,并且等待Di
        ///// </summary>
        ///// <param name="timeout"></param>
        ///// <param name="doId"></param>
        ///// <param name="doSts"></param>
        ///// <param name="diId"></param>
        ///// <param name="diSts"></param>
        ///// <returns></returns>
        //public bool SetDoAndWaitDi(int timeout, int doId, DOSTSTYPE doSts, int diId, DISTSTYPE diSts)
        //{
        //    return command.SetDoAndWaitDi(timeout, doId, doSts, diId, diSts);
        //}

        ///// <summary>
        ///// 发送 机械手要输出的DO组,并且等待Di组
        ///// </summary>
        ///// <param name="timeout"></param>
        ///// <param name="doId"></param>
        ///// <param name="doSts"></param>
        ///// <param name="diId"></param>
        ///// <param name="diSts"></param>
        ///// <returns></returns>
        //public bool SetDoAndWaitDi(int timeout, int[] doId, DOSTSTYPE[] doSts, int[] diId, DISTSTYPE[] diSts)
        //{
        //    return command.SetDoAndWaitDi(timeout, doId, doSts, diId, diSts);
        //}


        ///// <summary>
        ///// 获取机械手本体输出信号
        ///// </summary>
        ///// <param name="diId"></param>
        ///// <param name="sts"></param>
        ///// <returns></returns>
        //public bool GetDISts(int diId, out DISTSTYPE sts)
        //{
        //    return command.GetDISts(diId, out sts);
        //}

        ///// <summary>
        ///// 获取机械手本体输出信号
        ///// </summary>
        ///// <param name="doId"></param>
        ///// <param name="sts"></param>
        ///// <returns></returns>
        //public bool GetDOSts(int doId, out DOSTSTYPE sts)
        //{
        //    return command.GetDOSts(doId, out sts);
        //}




        //public bool Trigger(string cmd)
        //{
        //    var iRtn = command.Trigger(cmd);
        //    return iRtn;
        //}

        //public bool GetData(out string data, int timeout)
        //{
        //    return command.GetData(out data, timeout);
        //}

        //public bool TriggerAndGetData(string cmd, int timeout, out string data)
        //{
        //    data = "";
        //    var iRtn = command.Trigger(cmd);
        //    if (!iRtn)
        //    {
        //        return false;
        //    }
        //    return command.GetData(out data, timeout);
        //}




    }
}
