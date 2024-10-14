using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using APS168_W32;

namespace ZeroBase
{
    public class XMove : XCtrlEventHandler
    {
        private bool m_Busy;
        private bool m_Sevo;
        private bool m_Move;
        private bool m_Stop;
        private bool m_Pause;
        private bool m_AllowContinue;
        private int m_Status;
        private MoveMode m_MoveMode;
        private int m_Count;
        private int[] m_AxisId;
        private double[] m_Pos;
        private double[] m_Vel;
        private double[] m_Center;
        private int[] m_isStart;
        private int m_PtbId;
        private int m_PtbCount;
        private PTSTS m_PtSts;
        private bool m_ServoSts;
        private bool m_CheckLmt;
        private ManualResetEvent _mre;
        private XStation xStation;
      
        public XMove(XStation xStation)
        {
            m_Busy = false;
            m_Sevo = false;
            m_Move = false;
            m_Pause = false;
            m_AllowContinue = false;
            m_Status = 0;
            m_MoveMode = MoveMode.NONE;
            m_Count = 0;
            m_AxisId = new int[8];
            m_ServoSts = false;
            m_Pos = new double[8];
            m_Vel = new double[8];
            m_Center = new double[2];
            this._mre = new ManualResetEvent(false);
            this.xStation = xStation;
        }

        public override int HandleEvent(XEvent xEvent)
        {   
            switch (xEvent.EventID)
            {
                case XEventID.ALARM:
                    switch (xEvent.EventArgs.AlarmLevel)
                    { 
                        case (int)XAlarmLevel.PAUSE:
                            PrimOnPause();
                            break;
                        case (int) XAlarmLevel.STOP:
                            PrimOnStop();
                            break;
                    }
                    break;
                case XEventID.ESTOP:
                    PrimOnEStop();
                    break;
                case XEventID.SETSERVO:
                    PrimOnSetServo();
                    break;
                case XEventID.MOVE:
                    switch (m_MoveMode)
                    {
                        case MoveMode.MOVE_HOME:
                            PrimOnMoveHome();
                            break;
                        case MoveMode.MOVE_ABS:
                            PrimOnMoveAbs();
                            break;
                        case MoveMode.MOVE_REL:
                            PrimOnMoveRel();
                            break;
                        case  MoveMode.MOVE_JOG :
                            PrimOnMoveJog();
                            break;
                        case MoveMode.APS_PT_START:
                            PrimOnApsPtStart();
                            break;
                    }
                    break;
                case XEventID.PAUSE:
                    PrimOnPause();
                    break;
                case XEventID.CONTINUE:
                    PrimOnContinue();
                    break;
                case XEventID.RST:
                    PrimOnRST();
                    break;
                case XEventID.STOPMUSTRESET:
                    PrimOnStop();
                    break;
                case XEventID.MOVESTOP:
                    PrimOnMoveStop();
                    break;
                case XEventID.SIGNAL:
                    PrimOnSignal();
                    break;
            }
            return 0;
        }

        private int PrimOnSignal()
        {
            if (m_Sevo == true)
            {
                bool bDone = true;
                for (int i = 0; i < m_Count; i++)
                {
                    if (m_ServoSts == true)
                    {
                        if (XDevice.Instance.FindAxisById(m_AxisId[i]).IsSVON == false)
                        {
                            bDone = false;
                        }
                    }
                    else
                    {
                        if (XDevice.Instance.FindAxisById(m_AxisId[i]).IsSVON == true)
                        {
                            bDone = false;
                        }
                    }
                }
                if (bDone)
                {
                    PrimOnDone();
                }
            }

            if (m_Move == true)
            {
                bool bDone = true;
                bool bAlarm = false;
                for (int i = 0; i < m_Count; i++)
                {
                    if (m_MoveMode == MoveMode.MOVE_HOME)
                    {
                        if (XDevice.Instance.FindAxisById(m_AxisId[i]).IsHMV == true)
                        {
                            bDone = false;
                        }
                        if (XDevice.Instance.FindAxisById(m_AxisId[i]).IsALM == true)
                        {
                            bDone = false;
                            m_Status = -2;
                            bAlarm = true;
                            PostEvent(xStation, XAlarmLevel.STOP, XSysAlarmId.AXIS_ALM, m_AxisId[i]);
                        }
                        //------------------雷塞专用，对于其他卡有待验证----------------------------------------------
                        if (XDevice.Instance.FindAxisById(m_AxisId[i]).IsHomeD == false)
                        {
                            bDone = false;
                        }
                    }
                    else if (m_MoveMode == MoveMode.APS_PT_START)
                    {
                        XDevice.Instance.FindCardById(XDevice.Instance.FindAxisById(m_AxisId[0]).CardId).APS_get_pt_status(m_PtbId, ref m_PtSts);
                        if (XDevice.Instance.FindAxisById(m_AxisId[i]).IsALM == true)
                        {
                            bDone = false;
                            m_Status = -2;
                            bAlarm = true;
                            PostEvent(xStation, XAlarmLevel.STOP, XSysAlarmId.AXIS_ALM, m_AxisId[i]);
                        }
                        if (XDevice.Instance.FindAxisById(m_AxisId[i]).IsASTP == true)
                        {
                            bDone = false;
                            m_Status = -2;
                            bAlarm = true;
                            PostEvent(xStation, XAlarmLevel.STOP, XSysAlarmId.AXIS_ASTP, m_AxisId[i]);
                        }
                        if ((m_PtSts.BitSts & 0x01) == 1 && m_PtSts.RunningCnt < m_PtbCount)
                        {
                            bDone = false;
                        }
                    }
                    else
                    {
                        if (XDevice.Instance.FindAxisById(m_AxisId[i]).IsALM == true)
                        {
                            bDone = false;
                            m_Status = -2;
                            bAlarm = true;
                            PostEvent(xStation, XAlarmLevel.STOP, XSysAlarmId.AXIS_ALM, m_AxisId[i]);
                        }
                        if (XDevice.Instance.FindAxisById(m_AxisId[i]).IsASTP == true)
                        {
                            if (XDevice.Instance.FindAxisById(m_AxisId[i]).IsPEL == true)
                            {
                                if (m_CheckLmt)
                                {
                                    bDone = false;
                                    m_Status = -2;
                                    bAlarm = true;
                                    PostEvent(xStation, XAlarmLevel.STOP, XSysAlarmId.AXIS_PEL, m_AxisId[i]);
                                }
                            }
                            else if (XDevice.Instance.FindAxisById(m_AxisId[i]).IsMEL == true)
                            {
                                if (m_CheckLmt)
                                {
                                    bDone = false;
                                    m_Status = -2;
                                    bAlarm = true;
                                    PostEvent(xStation, XAlarmLevel.STOP, XSysAlarmId.AXIS_MEL, m_AxisId[i]);
                                }
                            }
                            else
                            {
                                bDone = false;
                                m_Status = -2;
                                bAlarm = true;
                                PostEvent(xStation, XAlarmLevel.STOP, XSysAlarmId.AXIS_ASTP, m_AxisId[i]);
                            }
                        }
                        if (XDevice.Instance.FindAxisById(m_AxisId[i]).IsMDN == false)
                        {
                            bDone = false;
                        }
                        if (XDevice.Instance.FindAxisById(m_AxisId[i]).IsFeedback)
                        {
                            if (m_CheckLmt)
                            {
                                var dev=Math.Abs(XDevice.Instance.FindAxisById(m_AxisId[i]).POS - m_Pos[i]);
                                if ( dev> 1)
                                {
                                    bDone = false;
                                }
                            }
                        }
                    }
                }
                if (bDone)
                {
                    if (m_MoveMode == MoveMode.MOVE_HOME)
                    {
                        bool IsHomeOk = true;
                        for (int i = 0; i < m_Count; i++)
                        {
                            bool aa = XDevice.Instance.FindAxisById(m_AxisId[i]).IsMDN;
                            //if (XDevice.Instance.FindAxisById(m_AxisId[i]).IsMDN)
                            if (XDevice.Instance.FindAxisById(m_AxisId[i]).IsALM != true && XDevice.Instance.FindAxisById(m_AxisId[i]).IsASTP != true)
                            {
                                XDevice.Instance.FindAxisById(m_AxisId[i]).SetHome(true);
                                XDevice.Instance.FindAxisById(m_AxisId[i]).HasEStoped = false;
                            }
                            else
                            {
                                IsHomeOk = false;
                                XDevice.Instance.FindAxisById(m_AxisId[i]).SetHome(false);
                            }
                        }
                        if (IsHomeOk)
                        {
                            PrimOnDone();
                        }
                        else
                        {
                            PrimOnExit();
                        }
                    }
                    else
                    {
                        PrimOnDone();
                    }
                }
                else if (bAlarm == true)
                {
                    if (m_MoveMode == MoveMode.MOVE_HOME)
                    {
                        for (int i = 0; i < m_Count; i++)
                        {
                            XDevice.Instance.FindAxisById(m_AxisId[i]).SetHome(false);
                        }
                    }
                    PrimOnExit();
                }
            }
            return 0;
        }

        private int PrimOnSetServo()
        {
            for (int i = 0; i < m_Count; i++)
            {
                XDevice.Instance.FindAxisById(m_AxisId[i]).SetServo(m_ServoSts);
            }
            m_Sevo = true;
            m_Move = false;
            m_Pause = false;
            return 0;
        }
        private int PrimOnMoveHome()
        {
            if (m_Stop)
            {
                PrimOnExit();
                return -1;
            }
            if (m_Pause)
            {
                return 1;
            }
            for (int i = 0; i < m_Count; i++)
            {
                if (XDevice.Instance.FindAxisById(m_AxisId[i]).HasEStoped == true 
                    || XDevice.Instance.FindAxisById(m_AxisId[i]).HasSVONOFF == true)
                {
                    XDevice.Instance.FindAxisById(m_AxisId[i]).SetHome(false);
                    XDevice.Instance.FindAxisById(m_AxisId[i]).GoHome();
                    
                }
            }
            m_Move = true;
            m_MoveMode = MoveMode.MOVE_HOME;
            return 0;
        }
        private int PrimOnMoveAbs()
        {
            if (m_Stop)
            {
                PrimOnExit();
                return -1;
            }
            if (m_Pause)
            {
                return 1;
            }
            for (int i = 0; i < m_Count; i++)
            {
                XDevice.Instance.FindAxisById(m_AxisId[i]).MoveAbs(m_Pos[i], m_Vel[i]);
            }
            m_Move = true;
            m_MoveMode = MoveMode.MOVE_ABS;
            return 0;
        }
        private int PrimOnMoveJog()
        {
            if (m_Stop)
            {
                PrimOnExit();
                return -1;
            }
            for (int i = 0; i < m_Count; i++)
            {
                if (XDevice.Instance.FindAxisById(m_AxisId[i]).IsFeedback)
                {
                    m_Pos[i] += XDevice.Instance.FindAxisById(m_AxisId[i]).POS;
                }
                else
                {
                    m_Pos[i] += XDevice.Instance.FindAxisById(m_AxisId[i]).CommandPOS;
                }
            }
            if (m_Pause)
            {
                return 1;
            }
            for (int i = 0; i < m_Count; i++)
            {
                XDevice.Instance.FindAxisById(m_AxisId[i]).MoveJog(m_isStart[i]);
            }
            m_Move = true;
            m_MoveMode = MoveMode.MOVE_JOG;
            return 0;
        }
        private int PrimOnMoveRel()
        {
            if (m_Stop)
            {
                PrimOnExit();
                return -1;
            }
            for (int i = 0; i < m_Count; i++)
            {
                if (XDevice.Instance.FindAxisById(m_AxisId[i]).IsFeedback)
                {
                    m_Pos[i] += XDevice.Instance.FindAxisById(m_AxisId[i]).POS;
                }
                else
                {
                    m_Pos[i] += XDevice.Instance.FindAxisById(m_AxisId[i]).CommandPOS;
                }
            }
            if (m_Pause)
            {
                return 1;
            }
            for (int i = 0; i < m_Count; i++)
            {
                XDevice.Instance.FindAxisById(m_AxisId[i]).MoveAbs(m_Pos[i], m_Vel[i]);
            }
            m_Move = true;
            m_MoveMode = MoveMode.MOVE_REL;
            return 0;
        }
        private int PrimOnApsPtStart()
        {
            if (m_Stop)
            {
                PrimOnExit();
                return -1;
            }
            if (m_Pause)
            {
                return 1;
            }
            XDevice.Instance.FindCardById(XDevice.Instance.FindAxisById(m_AxisId[0]).CardId).APS_pt_start(m_PtbId);
            m_Move = true;
            m_MoveMode = MoveMode.APS_PT_START;
            return 0;
        }
        private int PrimOnPause()
        {
            m_Pause = true;
            if (m_MoveMode != MoveMode.APS_PT_START)
            {
                for (int i = 0; i < m_Count; i++)
                {
                    XDevice.Instance.FindAxisById(m_AxisId[i]).EStop();
                }
                m_Move = false;
            }
            m_Status = 1;
            return 0;
        }
        private int PrimOnContinue()
        {
            if (m_Pause)
            {
                if (m_AllowContinue == false)
                {
                    PrimOnDone();
                    return 0;
                }
                if (m_MoveMode == MoveMode.MOVE_HOME)
                {
                    for (int i = 0; i < m_Count; i++)
                    {
                        XDevice.Instance.FindAxisById(m_AxisId[i]).GoHome();
                    }
                }
                else if (m_MoveMode == MoveMode.MOVE_ABS || m_MoveMode == MoveMode.MOVE_REL)
                {
                    for (int i = 0; i < m_Count; i++)
                    {
                        XDevice.Instance.FindAxisById(m_AxisId[i]).MoveAbs(m_Pos[i], m_Vel[i]);
                    }
                }
                m_Move = true;
                m_Pause = false;
                m_Status = 0;
            }
            return 0;
        }
        private int PrimOnRST()
        {
            lock (this)
            {
                m_Busy = false;
            }
            m_Sevo = false;
            m_MoveMode = MoveMode.NONE;
            m_Stop = false;
            m_Move = false;
            m_Pause = false;
            m_AllowContinue = false;
            m_Status = 0;
            _mre.Reset();
            return 0;
        }
        private int PrimOnStop()
        {
            for (int i = 0; i < m_Count; i++)
            {
                XDevice.Instance.FindAxisById(m_AxisId[i]).EStop();
            }
            m_Stop = true;
            m_Status = -1;
            PrimOnExit();
            return 0;
        }
        private int PrimOnMoveStop()
        {
            for (int i = 0; i < m_Count; i++)
            {
                XDevice.Instance.FindAxisById(m_AxisId[i]).EStop();
            }
            PrimOnDone();
            return 0;
        }
        private int PrimOnEStop()
        {
            foreach (KeyValuePair<int, IAxis> kvp in XDevice.Instance.AxisMap)
            {
                kvp.Value.HasEStoped = true;
            }
            m_Stop = true;
            m_Status = -1;
            PrimOnExit();
            return 0;
        }
        private int PrimOnExit()
        {
            lock (this)
            {
                m_Busy = false;
            }
            m_Sevo = false;
            m_MoveMode = MoveMode.NONE;
            m_Move = false;
            m_Pause = false;
            m_AllowContinue = false;
            _mre.Set();
            return 0;
        }
        private int PrimOnDone()
        {
            lock (this)
            {
                m_Busy = false;
            }
            m_Sevo = false;
            m_MoveMode = MoveMode.NONE;
            m_Move = false;
            m_Pause = false;
            m_AllowContinue = false;
            m_Status = 0;
            _mre.Set();
            return 0;
        }


        public int SetServo(int[] axisId, int count, bool on)
        {
            lock (this)
            {
                if (m_Busy == true)
                {
                    return -1;
                }
                m_Busy = true;
                for (int i = 0; i < count; i++)
                {
                    m_AxisId[i] = axisId[i];
                }
                m_ServoSts = on;
                m_Count = count;
                m_Status = 0;
            }
            _mre.Reset();
            PostEvent(this, XEventID.SETSERVO);
            int ret = WaitEvent(3000);
            if (ret != 0)
            {
                if (m_ServoSts == true)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (XDevice.Instance.FindAxisById(m_AxisId[i]).IsSVON == false)
                        {
                            PostEvent(xStation, XAlarmLevel.STOP, XSysAlarmId.AXIS_SERVON_FAIL, m_AxisId[i]);
                        }
                    }
                }
            }
            return ret;
        }
        public int MoveHome(int[] axisId, int count)
        {
            SetServo(axisId, count, true);
            lock (this)
            {
                if (m_Busy == true)
                {
                    return -1;
                }
                m_Busy = true;
                for (int i = 0; i < count; i++)
                {
                    m_AxisId[i] = axisId[i];
                }
                m_Stop = false;
                m_Count = count;
                m_AllowContinue = true;
                m_Status = 0;
                m_MoveMode = MoveMode.MOVE_HOME;
            }
            _mre.Reset(); 
            PostEvent(this, XEventID.MOVE);
            return 0;
        }
        public int MoveAbs(int[] axisId, double[] pos, double[] vel, int count, bool checkLmt = true)
        {
            for (int i = 0; i < count; i++)
            {
                if (XDevice.Instance.FindAxisById(axisId[i]).IsHomeOk == false)
                {
                    return -2;
                }
            }
            lock (this)
            {
                if (m_Busy == true)
                {
                    return -1;
                }
                m_Busy = true;
                for (int i = 0; i < count; i++)
                {
                    m_AxisId[i] = axisId[i];
                    m_Pos[i] = pos[i];
                    m_Vel[i] = vel[i];
                }
                m_Count = count;
                m_AllowContinue = true;
                m_CheckLmt = checkLmt;
                m_Status = 0;
                m_MoveMode = MoveMode.MOVE_ABS;
            }
            _mre.Reset();
            PostEvent(this, XEventID.MOVE);
            return 0;

        }
        public int MoveJog(int[] axisID, int []isStart, int count,bool checkLmt = true)
        {
            lock (this)
            {

                m_isStart=new int [count];
                for (int i = 0; i < count; i++)
                {
                    m_AxisId[i] = axisID[i];
                    m_isStart[i] = isStart[i];
                }
                m_Count = count;
                m_AllowContinue = true;
                m_CheckLmt = checkLmt;
                m_Status = 0;
                m_MoveMode = MoveMode.MOVE_JOG;
            }
            _mre.Reset();
            PostEvent(this, XEventID.MOVE);
            return 0;
        }
        public int MoveRel(int[] axisID, double[] pos, double[] vel, int count, bool checkLmt = true)
        {
            lock (this)
            {
                if (m_Busy)
                {
                    return -1;
                }
                m_Busy = true;
                for (int i = 0; i < count; i++)
                {
                    m_AxisId[i] = axisID[i];
                    m_Pos[i] = pos[i];
                    m_Vel[i] = vel[i];
                }
                m_Count = count;
                m_AllowContinue = true;
                m_CheckLmt = checkLmt;
                m_Status = 0;
                m_MoveMode = MoveMode.MOVE_REL;
            }
            _mre.Reset();
            PostEvent(this, XEventID.MOVE);
            return 0;
        }

        public int MoveStopMustReset()
        {
            PostEvent(this, XEventID.STOPMUSTRESET);
            return 0;
        }
        public int MoveStop()
        {
            PostEvent(this, XEventID.MOVESTOP);
            return 0;
        }
        public int APS_pt_start(int[] axisId, int axisCount, int ptbId, int ptbCount)
        {
            lock (this)
            {
                if (m_Busy == true)
                {
                    return -1;
                }
                m_Busy = true;
                for (int i = 0; i < axisCount; i++)
                {
                    m_AxisId[i] = axisId[i];
                }
                m_Count = axisCount;
                m_PtbId = ptbId;
                m_PtbCount = ptbCount;
                m_PtSts = new PTSTS();
                m_AllowContinue = true;
                m_Status = 0;
                m_MoveMode = MoveMode.APS_PT_START;
            }
            _mre.Reset();
            PostEvent(this, XEventID.MOVE);
            return 0;
        }
        
        /// <summary>
        /// wait for event
        /// </summary>
        /// <param name="timeout">timeout</param>
        /// <returns>running:0/stop:-1/astp:-2/pause:1/timeout:2</returns>
        public int WaitEvent(int timeout)
        {
            bool bRtn = _mre.WaitOne(timeout);
            _mre.Reset();
            if (bRtn == false)
            {
                lock (this)
                {
                    m_Busy = false;
                    m_Sevo = false;
                    m_MoveMode = MoveMode.NONE;
                    m_Move = false;
                    m_Pause = false;
                    m_Status = 2;
                }
            }
            return m_Status;
        }

        enum MoveMode
        {
            NONE,
            MOVE_HOME,
            MOVE_ABS,
            MOVE_REL,
            APS_PT_START,
            MOVE_JOG,
        }
    }
}
