using System;

namespace ZeroBase
{

    /// <summary>
    /// 机械手轴号命名,通常4轴机械手使用的是X,Y,Z,C;
    /// </summary>
    public enum AxisType
    {
        X,
        Y,
        Z,
        A,
        B,
        C,
        U
    }


    public abstract class XCommandRobot : XTask
    {

        public string ip { get; set; }
        public int Port { get; set; }

        public virtual bool Alarm { get; set; }

        public virtual bool Running { get; set; }
        public virtual int InitialConnect() { return -1; }

        public virtual bool RobotConnected { get; set; }
        public virtual int DisConnect() { return -1; }
        public RobotIo Robort_IO;
        public int RobortID;

        private double LimtZ = -30;

        #region IO 操作

        public virtual RobotIo RobortIO
        {
            get { return new RobotIo (); }
        }

        public virtual void StopByIO()
        {

        }
        public virtual void ResetByIO()
        {

        }
        public virtual void SatrtByIO()
        {

        }
        public virtual void ServoOffByIO()
        {

        }
        public virtual void ServoOnByIO()
        {

        }
        public virtual void AuthorizationByIO(int value)
        {

        }

        public virtual void PauseOn()
        {

        }
        public virtual void GoOn()
        {

        }


        #endregion

        public virtual void RestartContorlerByCommand()
        { }


        public virtual new bool GetRobotPos(out double[] pos)
        {
            pos = new double[6]; return false;
        }

        /// <summary>
        /// 单轴直线插补运动
        /// </summary>
        /// <param name="type"></param>
        /// <param name="rel"></param>
        /// <param name="vel"></param>
        /// <param name="waitMotionDone"></param>
        /// <returns></returns>
        public virtual bool MovRel(AxisType type, double rel, double vel, bool waitMotionDone = true)
        {
            return false;
        }

        /// <summary>
        /// 以直线插补动作从现在位置到移动目的位置为止 。(直线)
        /// </summary>
        /// <param name="tarPostion">目标位置</param>
        /// <param name="vel">速度</param>
        /// <param name="timeout"></param>
        /// <param name="waitMotionDone"></param>
        /// <returns></returns>
        public virtual bool MovL(double[] tarPostion, int vel, int timeout, bool waitMotionDone = true)
        {
            return false;
        }
        /// <summary>
        /// 从现在位置到移动目的位置 执行关节插补位置。(S形)
        /// </summary>
        /// <param name="tarPostion"></param>
        /// <param name="vel"></param>
        /// <param name="timeout"></param>
        /// <param name="waitMotionDone"></param>
        /// <returns></returns>
        public virtual bool MovS(double[] tarPostion, int vel, int timeout, bool waitMotionDone = true)
        {
            return false;
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
        public virtual bool JumpL(double[] tarPostion, int vel, int timeout, bool waitMotionDone = true, double limtZ = -50)
        {
            return false;
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
        public virtual bool JumpS(double[] tarPostion, int vel, int timeout, bool waitMotionDone = true, double limtZ = -50)
        {
            return false;
        }
        public virtual bool MoveTorque(double RelZ, int vel, double press, double time1, out double feedPress, int timeout, bool waitMotionDone = true)
        {
            feedPress = 0;
            return false;
        }

        public virtual bool MoveCp(double[] tmpPostion, double[] tarPostion, int vel, int timeout, bool waitMotionDone = true)
        {
            return false;
        }


        /// <summary>
        /// 发送 机械手要输出的DO,并且等待Di
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="doId"></param>
        /// <param name="doSts"></param>
        /// <param name="diId"></param>
        /// <param name="diSts"></param>
        /// <returns></returns>
        public virtual bool SetDoAndWaitDi(int timeout, int doId, DOSTSTYPE doSts, int diId, DISTSTYPE diSts)
        {
            return false;
        }

        /// <summary>
        /// 发送 机械手要输出的DO组,并且等待Di组
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="doId"></param>
        /// <param name="doSts"></param>
        /// <param name="diId"></param>
        /// <param name="diSts"></param>
        /// <returns></returns>
        public virtual bool SetDoAndWaitDi(int timeout, int[] doId, DOSTSTYPE[] doSts, int[] diId, DISTSTYPE[] diSts)
        {
            return false;
        }


        /// <summary>
        /// 获取机械手本体输出信号
        /// </summary>
        /// <param name="diId"></param>
        /// <param name="sts"></param>
        /// <returns></returns>
        public virtual bool GetDISts(int diId, out DISTSTYPE sts)
        {
            sts = DISTSTYPE.LOW;
            return false;
        }

        /// <summary>
        /// 获取机械手本体输出信号
        /// </summary>
        /// <param name="doId"></param>
        /// <param name="sts"></param>
        /// <returns></returns>
        public virtual bool GetDOSts(int doId, out DOSTSTYPE sts)
        {
            sts = DOSTSTYPE.LOW;
            return false;
        }
        /// <summary>
        /// 设置机械手本体输出信号
        /// </summary>
        /// <param name="doId"></param>
        /// <param name="sts"></param>
        /// <returns></returns>
        public virtual bool SetDOSts(int doId, DOSTSTYPE sts)
        {
            return false;
        }
        /// <summary>
        /// 设置机械手本体多个输出信号
        /// </summary>
        /// <param name="doId"></param>
        /// <param name="sts"></param>
        /// <returns></returns>
        public virtual bool SetDOSts(int [] doId, DOSTSTYPE [] sts)
        {
            return false;
        }
        /// <summary>
        /// 等待机械手本体多个输入信号
        /// </summary>
        /// <param name="doId"></param>
        /// <param name="sts"></param>
        /// <returns></returns>
        public virtual bool WaitDISts(int timeout ,int[] diId, DISTSTYPE[] sts)
        {

            return false;
        }
        /// <summary>
        /// 等待机械手本体单个输入信号
        /// </summary>
        /// <param name="doId"></param>
        /// <param name="sts"></param>
        /// <returns></returns>
        public virtual bool WaitDISts(int timeout, int diId, DISTSTYPE sts)
        {

            return false;
        }
        public virtual bool JudgePosEqual(double[] pos)
        {
            return false;
        }


        public virtual bool Trigger(string cmd)
        { return false; }

        public virtual bool GetData(out string data, int timeout)
        {
            data = ""; return true;
        }
        public virtual bool GetAlarmData(out string data)
        {
            data = ""; return true;
        }
        public virtual bool TriggerAndGetData(string cmd, bool waitMotionDone, int timeout = 30000)
        {
            return false;
        }

        public virtual bool TriggerAndGetData(string cmd, bool waitMotionDone, out string result,int timeout = 30000)
        {
            result = "";
            return false;
        }

        public virtual int Close() { return -1; }

        public virtual bool PartMode() { return false; }
    }
}
