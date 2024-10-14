using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using TCPLib;

namespace XCore
{
    //EpsonRobot机械手
    public class EpsonRobot : XCommandRobot
    {
        private RobotIo io;
        private SimpleTcpClient simpleTcp = new SimpleTcpClient();
        private ConcurrentQueue<string> queue1 = new ConcurrentQueue<string>();
        private ConcurrentQueue<string> alarmqueue = new ConcurrentQueue<string>();
        private object obj = new object();
        public bool IsPause = false;
        private string endStr = "\r";


        public EpsonRobot(RobotIo io)
        {
            this.io = io;
            Robort_IO = io;

        }
        public RobotIo Rrobort_IO
        {
            get { return this.io; }
        }
        public override int InitialConnect()
        {
            simpleTcp.Connect(ip, Port);
            simpleTcp.DataReceived += simpleTcpClient_DataReceived;

            if (RobotConnected)
            {
                return 0;
            }
            return -1;
        }

        public override bool RobotConnected
        {
            get { return simpleTcp.Connected; }
        }
        public override bool Alarm
        {
            get { return XDevice.Instance.FindDiById(io.DiAlarm).STS; }
        }
        public override bool Running
        {
            get { return XDevice.Instance.FindDiById(io.DiProgramRunnig).STS; }
        }
        #region IO 操作
        public override void StopByIO()
        {
            XDevice.Instance.FindDoById(io.DoStop).SetDo(0);
            Thread.Sleep(500);
            XDevice.Instance.FindDoById(io.DoStop).SetDo(1);
            Thread.Sleep(500);
            XDevice.Instance.FindDoById(io.DoStop).SetDo(0);
        }
        public override void ResetByIO()
        {
            XDevice.Instance.FindDoById(io.DoReset).SetDo(0);
            Thread.Sleep(500);
            XDevice.Instance.FindDoById(io.DoReset).SetDo(1);
            Thread.Sleep(500);
            XDevice.Instance.FindDoById(io.DoReset).SetDo(0);

        }
        public override void SatrtByIO()
        {
            XDevice.Instance.FindDoById(io.DoStart).SetDo(0);
            Thread.Sleep(500);
            XDevice.Instance.FindDoById(io.DoStart).SetDo(1);
            Thread.Sleep(500);
            XDevice.Instance.FindDoById(io.DoStart).SetDo(0);
        }
        public override void ServoOffByIO()
        {
            Thread.Sleep(1000);
            XDevice.Instance.FindDoById(io.DoServo).SetDo(0);

        }
        public override void ServoOnByIO()
        {
            XDevice.Instance.FindDoById(io.DoServo).SetDo(0);
            Thread.Sleep(500);
            XDevice.Instance.FindDoById(io.DoServo).SetDo(1);
            Thread.Sleep(500);
            XDevice.Instance.FindDoById(io.DoServo).SetDo(0);
        }
        public override void AuthorizationByIO(int value)
        {
            XDevice.Instance.FindDoById(io.DoAuthorization).SetDo(value);
        }
        //暂停
        public override void PauseOn()
        {
            if (XDevice.Instance.FindDoById(io.DoStop).STS == false)
            {
                XDevice.Instance.FindDoById(io.DoPause).SetDo(0);
                Thread.Sleep(500);
                XDevice.Instance.FindDoById(io.DoPause).SetDo(1);
                Thread.Sleep(500);
                XDevice.Instance.FindDoById(io.DoPause).SetDo(0);
            }
        }
        //继续
        public override void GoOn()
        {

            if (XDevice.Instance.FindDoById(io.DoStop).STS == false)
            {

                XDevice.Instance.FindDoById(io.DoContiue).SetDo(0);
                Thread.Sleep(500);
                XDevice.Instance.FindDoById(io.DoContiue).SetDo(1);
                Thread.Sleep(500);
                XDevice.Instance.FindDoById(io.DoContiue).SetDo(0);
            }
        }

        #endregion

        /// <summary>
        /// 获取机械手坐标
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public override bool GetRobotPos(out double[] pos)
        {

            pos = new double[6];
            try
            {
                lock (obj)
                {
                    var cmd = "GETPOS,";
                    Trigger(cmd);
                    string data;
                    if (GetData(out data, 1000))
                    {
                        var posT = data.Split(',');
                        if (data.Contains(cmd))
                        {
                            if (posT.Length >= 5)
                            {
                                for (int i = 1; i < posT.Length; i++)
                                {
                                    pos[i - 1] = Convert.ToDouble(posT[i]);
                                    pos[i - 1] = Math.Round(pos[i - 1], 3);
                                }
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;

        }

        /// <summary>
        /// 单轴直线插补运动
        /// </summary>
        /// <param name="type"></param>
        /// <param name="rel">juli</param>
        /// <param name="vel"></param>
        /// <param name="waitMotionDone"></param>
        /// <returns></returns>
        public override bool MovRel(AxisType type, double rel, double vel, bool waitMotionDone = true)
        {
            var cmd = "MVS";
            var name = Enum.GetName(typeof(AxisType), type);
            cmd = cmd + "," + name + "," + vel + "," + rel;
            return TriggerAndGetData(cmd, waitMotionDone);
        }

        /// <summary>
        /// 以直线插补动作从现在位置到移动目的位置为止 。(直线)
        /// </summary>
        /// <param name="tarPostion">目标位置</param>
        /// <param name="vel">速度</param>
        /// <param name="timeout"></param>
        /// <param name="waitMotionDone"></param>
        /// <returns></returns>
        public override bool MovL(double[] tarPostion, int vel, int timeout, bool waitMotionDone = true)
        {
            var type = "MOVL";
            var cmd = type + "," + vel + ",";

            for (int i = 0; i < tarPostion.Count(); i++)
            {
                cmd += tarPostion[i] + ",";
            }
            cmd = cmd.Substring(0, cmd.Length - 1);
            return TriggerAndGetData(cmd, waitMotionDone);
        }

        /// <summary>
        /// 从现在位置到移动目的位置 执行关节插补位置。(S形)
        /// </summary>
        /// <param name="tarPostion"></param>
        /// <param name="vel"></param>
        /// <param name="timeout"></param>
        /// <param name="waitMotionDone"></param>
        /// <returns></returns>
        public override bool MovS(double[] tarPostion, int vel, int timeout, bool waitMotionDone = true)
        {
            var type = "MOVS";
            var cmd = type + "," + vel + ",";

            for (int i = 0; i < tarPostion.Count(); i++)
            {
                cmd += tarPostion[i] + ",";
            }
            cmd = cmd.Substring(0, cmd.Length - 1);

            return TriggerAndGetData(cmd, waitMotionDone);
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
        public override bool JumpL(double[] tarPostion, int vel, int timeout, bool waitMotionDone = true, double limtZ = 500)
        {
            var type = "JUMPL";
            var cmd = type + "," + vel + ",";

            for (int i = 0; i < tarPostion.Count(); i++)
            {
                cmd += tarPostion[i] + ",";
            }
            cmd = cmd + limtZ;

            return TriggerAndGetData(cmd, waitMotionDone, timeout);
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
        public override bool JumpS(double[] tarPostion, int vel, int timeout, bool waitMotionDone = true, double limtZ = 500)
        {
            var type = "JUMPS";
            var cmd = type + "," + vel + ",";

            for (int i = 0; i < tarPostion.Count(); i++)
            {
                cmd += tarPostion[i] + ",";
            }
            cmd = cmd + limtZ;

            return TriggerAndGetData(cmd, waitMotionDone, timeout);
        }

        /// <summary>
        /// 使用力控模式（Z轴）
        /// </summary>
        /// <param name="tarPostion">目标位置</param>
        /// <param name="vel">速度</param>
        /// <param name="press">目标压力</param>
        /// <param name="feedPress">反馈压力</param>
        /// <param name="timeout"></param>
        /// <param name="waitMotionDone"></param>
        /// <returns></returns>
        public override bool MoveTorque(double relz, int vel, double press, double time, out double feedPress, int timeout, bool waitMotionDone = true)
        {
            feedPress = 0;
            var type = "MTORQUE";
            var cmd = type + "," + vel + "," + relz + "," + press + "," + time;


            lock (obj)
            {
                var ret = Trigger(cmd);
                if (!ret)
                {
                    return false;
                }
                if (waitMotionDone)
                {
                    string data;
                    if (GetData(out data, timeout))
                    {
                        if (data.Contains(cmd.Split(',')[0]))
                        {
                            try
                            {
                                feedPress = Convert.ToDouble(data.Split(',')[data.Split(',').Count()]);
                                return true;
                            }
                            catch (Exception)
                            {
                                return false;
                            }
                        }
                    }
                    return false;
                }
            }
            return true;
        }

        public override bool MoveCp(double[] tmpPostion, double[] tarPostion, int vel, int timeout, bool waitMotionDone = true)
        {
            var type = "MOVECP";
            var cmd = type + "," + vel + ",";
            for (int i = 0; i < tmpPostion.Count(); i++)
            {
                cmd += tmpPostion[i] + ",";
            }
            for (int i = 0; i < tarPostion.Count(); i++)
            {
                cmd += tarPostion[i] + ",";
            }
            cmd = cmd.Substring(0, cmd.Length - 1);

            return TriggerAndGetData(cmd, waitMotionDone);

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
        public override bool SetDoAndWaitDi(int timeout, int doId, DOSTSTYPE doSts, int diId, DISTSTYPE diSts)
        {
            string type = "SETDOANDWAITDI," + timeout;
            string result;
            var cmd = type + "," + doId + ",";

            cmd += doSts == DOSTSTYPE.HIGH ? 1 + "," : 0 + ",";
            cmd += diId + ",";
            cmd += diSts == DISTSTYPE.HIGH ? 1 + "" : 0 + "";

            if (TriggerAndGetData(cmd, true, out result))
            {
                if (result.Split(',')[1] == "0")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

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
        public override bool SetDoAndWaitDi(int timeout, int[] doId, DOSTSTYPE[] doSts, int[] diId, DISTSTYPE[] diSts)
        {
            string result = "";
            string type = "SETDOANDWAITDI," + timeout;

            var cmd = type + ",";

            for (int i = 0; i < doId.Length; i++)
            {
                cmd += doId[i] + ";";
            }

            cmd += ",";
            for (int i = 0; i < doSts.Length; i++)
            {
                if (doSts[i] == DOSTSTYPE.HIGH)
                {
                    cmd += 1 + ";";
                }
                else
                {
                    cmd += 0 + ";";
                }

            }

            cmd += ",";
            for (int i = 0; i < diId.Length; i++)
            {
                cmd += diId[i] + ";";
            }

            cmd += ",";
            for (int i = 0; i < diSts.Length; i++)
            {
                if (diSts[i] == DISTSTYPE.HIGH)
                {
                    cmd += 1 + ";";
                }
                else
                {
                    cmd += 0 + ";";
                }

            }
            cmd = cmd.Replace(";,", ",");
            cmd = cmd.Substring(0, cmd.Length - 1);

            if (TriggerAndGetData(cmd, true, out result))
            {
                if (result.Split(',')[1] == "0")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public override bool SetDOSts(int doId, DOSTSTYPE sts)
        {
            string type = "SETDO";
            var cmd = type + "," + doId + ",";

            cmd += sts == DOSTSTYPE.HIGH ? 1 + "" : 0 + "";
            return TriggerAndGetData(cmd, true);
        }
        public override bool SetDOSts(int[] doId, DOSTSTYPE[] sts)
        {
            string type = "SETDO";
            var cmd = type + ",";
            for (int i = 0; i < doId.Length; i++)
            {
                cmd += doId[i] + ";";
            }

            cmd += ",";
            for (int i = 0; i < sts.Length; i++)
            {
                if (sts[i] == DOSTSTYPE.HIGH)
                {
                    cmd += 1 + ";";
                }
                else
                {
                    cmd += 0 + ";";
                }

            }
            cmd = cmd.Replace(";,", ",");
            cmd = cmd.Substring(0, cmd.Length - 1);
            return TriggerAndGetData(cmd, true);
        }
        public override bool WaitDISts(int timeout, int diId, DISTSTYPE sts)
        {
            string result = "";
            string type = "WAITDI," + timeout;
            string cmd = type + ",";
            cmd += diId + ",";
            cmd += sts == DISTSTYPE.HIGH ? 1 + "" : 0 + "";
            if (TriggerAndGetData(cmd, true, out result))
            {
                if (result.Split(',')[1] == "0")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public override bool WaitDISts(int timeout, int[] diId, DISTSTYPE[] sts)
        {
            string result = "";
            string type = "WAITDI," + timeout;
            string cmd = type + ",";
            for (int i = 0; i < diId.Length; i++)
            {
                cmd += diId[i] + ";";
            }

            cmd += ",";
            for (int i = 0; i < sts.Length; i++)
            {
                if (sts[i] == DISTSTYPE.HIGH)
                {
                    cmd += 1 + ";";
                }
                else
                {
                    cmd += 0 + ";";
                }

            }
            cmd = cmd.Replace(";,", ",");
            cmd = cmd.Substring(0, cmd.Length - 1);
            if (TriggerAndGetData(cmd, true, out result))
            {
                if (result.Split(',')[1] == "0")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取机械手本体输出信号
        /// </summary>
        /// <param name="diId"></param>
        /// <param name="sts"></param>
        /// <returns></returns>
        public override bool GetDISts(int diId, out DISTSTYPE sts)
        {
            var cmd = "GETDI," + diId;
            sts = DISTSTYPE.LOW;
            Trigger(cmd);
            string data;
            var ret = GetData(out data, 3000);
            if (ret)
            {
                if (Convert.ToInt32(data) == 1)
                {
                    sts = DISTSTYPE.HIGH;
                }
                return true;
                // var DI_Data = Convert.ToInt32(data);
                //var  sts1 = (DI_Data>> diId) & 1;
                //if (sts1==1)
                //{
                //    sts = DISTSTYPE.HIGH;
                //}
            }

            return false;
        }

        /// <summary>
        /// 获取机械手本体输出信号
        /// </summary>
        /// <param name="doId"></param>
        /// <param name="sts"></param>
        /// <returns></returns>
        public override bool GetDOSts(int doId, out DOSTSTYPE sts)
        {
            var cmd = "GETDO," + doId;

            sts = DOSTSTYPE.LOW;
            Trigger(cmd);
            string data;
            var ret = GetData(out data, 3000);
            if (ret)
            {
                if (Convert.ToInt32(data) == 1)
                {
                    sts = DOSTSTYPE.HIGH;
                }
                return true;
            }

            //sts = DOSTSTYPE.LOW;
            //Trigger(cmd);
            //string data = "";
            //var ret = GetData(out data, 3000);
            //if (ret)
            //{
            //    var DI_Data = Convert.ToInt32(data);
            //    var sts1 = (DI_Data >> doId) & 1;
            //    if (sts1 == 1)
            //    {
            //        sts = DOSTSTYPE.HIGH;
            //    }
            //}

            return false;
        }

        /// <summary>
        /// 判断坐标与当前机械手坐标是否相等
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public override bool JudgePosEqual(double[] pos)
        {
            try
            {
                var cmd = "GETPOS";
                Trigger(cmd);
                string data;
                if (GetData(out data, 3000))
                {
                    var posT = data.Split(',');
                    if (posT.Length > 5)
                    {
                        for (int i = 1; i < posT.Length; i++)
                        {
                            if (Math.Abs(pos[i - 1] - Convert.ToDouble(posT[i])) > 0.05)
                            {
                                return false;
                            }
                        }
                        return true;
                    }

                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;

        }

        /// <summary>
        /// 快速门型指令
        /// </summary>
        /// <param name="tarpos">目标位置</param>
        /// <param name="limtUp">上升避障高度</param>
        /// <param name="limtTop">顶端高度</param>
        /// <param name="vel"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool JumpMvaToTargetPos(double[] tarpos, double limtUp, double limtTop, int vel, int timeout)
        {
            var type = "JUMPMVA";
            var cmd = type + "," + vel + "," + limtUp + "," + limtUp + "," + limtTop + "," + limtTop + "," + tarpos[0] + "," + tarpos[1] + "," + tarpos[2] + "," + tarpos[3];
            if (Trigger(cmd))
            {
                Thread.Sleep(30);
                string data;
                if (GetData(out data, 30000))
                {
                }
                while (true)
                {
                    Thread.Sleep(10);
                    break;
                }

                return true;
            }
            return false;
        }

        public override bool Trigger(string cmd)
        {
            while (queue1.Count > 0)
            {
                string left;
                queue1.TryDequeue(out left);
            }

            var ss = cmd;
            var count = 17 - ss.Split(',').Count();


            for (int i = 0; i < count; i++)
            {
                cmd += ",";
            }


            simpleTcp.Write(cmd + endStr);
            return true;

        }
        public override bool GetAlarmData(out string data)
        {
            data = "";

            try
            {

                if (alarmqueue.Count > 0)
                {
                    alarmqueue.TryDequeue(out data);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;

            }

        }
        public override bool GetData(out string data, int timeout)
        {
            data = "";
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            while (true)
            {
                try
                {
                    var alm = XDevice.Instance.FindDiById(io.DiAlarm).STS;
                    alm = false;
                    if (stopWatch.ElapsedMilliseconds > timeout || alm)
                    {
                        if (timeout != -1)
                        {
                            return false;
                        }
                    }
                    if (XDevice.Instance.FindDoById(io.DoStop).STS)
                    {
                        stopWatch.Restart();
                        Thread.Sleep(10);
                        continue;
                    }
                    if (queue1.Count > 0)
                    {
                        queue1.TryDequeue(out data);
                        break;
                    }
                    Thread.Sleep(1);
                }
                catch (Exception)
                {
                    return false;

                }
            }
            return true;
        }

        public override bool TriggerAndGetData(string cmd, bool waitMotionDone, int timeout = 30000)
        {
            lock (obj)
            {
                var ret = Trigger(cmd);
                if (!ret)
                {
                    return false;

                }
                if (waitMotionDone)
                {
                    string data;
                    if (GetData(out data, timeout))
                    {
                        if (data.Contains(cmd.Split(',')[0]))
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            return true;
        }
        public override bool TriggerAndGetData(string cmd, bool waitMotionDone, out string result, int timeout = 30000)
        {
            lock (obj)
            {
                result = "";
                var ret = Trigger(cmd);
                if (!ret)
                {

                    return false;

                }
                if (waitMotionDone)
                {
                    string data;
                    if (GetData(out data, timeout))
                    {
                        result = data;
                        if (data.Contains(cmd.Split(',')[0]))
                        {

                            return true;
                        }
                    }
                    return false;
                }
            }
            return true;
        }
        public void simpleTcpClient_DataReceived(object sender, Message message)
        {
            string result = message.MessageString;
            try
            {
                if (result != null)
                {
                    //int pos = result.IndexOf("\r");
                    //result = result.Substring(0, pos);
                }
                if (result.Contains("ERR"))
                {
                    alarmqueue.Enqueue(result);
                }
                else
                {

                    queue1.Enqueue(result);
                }
            }
            catch
            {

            }
        }

        public override bool PartMode()
        {
            var type = "PARTMODE";
            var cmd = type + ",";

            return TriggerAndGetData(cmd, true, -1);
        }

    }
}
