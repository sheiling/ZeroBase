using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace ZeroBase
{
    public enum DISTSTYPE
    {
        /// <summary>
        /// 高电平
        /// </summary>
        HIGH = 0,
        /// <summary>
        /// 低电平
        /// </summary>
        LOW = 1,
        /// <summary>
        /// 上升沿
        /// </summary>
        PLS = 2,
        /// <summary>
        /// 下降沿
        /// </summary>
        PLF = 3
    }

    public class XWaitDi : XCtrlEventHandler
    {
        private bool _m_Wait;
        private bool m_Timeout;
        private int m_Status;
        private int m_Count;
        private DISTSTYPE[] m_DiStsType;
        private int[] m_DiId;
        private Stopwatch sw = new Stopwatch();
        private ManualResetEvent _mre;
        private ManualResetEvent _mreTimeout;
        private bool m_OnSignaled;
        private XStation xStation;

        private object obj = new object();

        public bool m_Wait
        {
            get
            {
                lock (obj)
                {
                    return _m_Wait;
                }
            }
            set
            {
                lock (obj)
                {
                    _m_Wait = value;
                }
            }
        }

        public XWaitDi(XStation xStation)
        {
            m_Status = 0;
            m_Timeout = false;
            m_Wait = false;
            m_OnSignaled = false;
            m_Count = 0;
            m_DiStsType = new DISTSTYPE[8];
            m_DiId = new int[8];
            _mre = new ManualResetEvent(false);
            _mreTimeout = new ManualResetEvent(false);
            this.xStation = xStation;
        }

        public string ErrorMessage { get; set; }

        public override int HandleEvent(XEvent xEvent)
        {
            switch (xEvent.EventID)
            {
                case XEventID.RST:
                case XEventID.RESET:
                    {
                        PrimOnRST();
                    }
                    break;
                case XEventID.WAITDI:
                    {
                        PrimOnWait();
                    }
                    break;
                case XEventID.STOPMUSTRESET:
                    {
                        PrimOnStop();
                    }
                    break;
                case XEventID.ESTOP:
                    {
                        PrimOnStop();
                    }
                    break;
                case XEventID.CONTINUE:
                    {
                        PrimOnContinue();
                    }
                    break;
                case XEventID.DICONTINUE:
                    {
                        PrimOnDiContinue();
                    }
                    break;
                case XEventID.SIGNAL:
                    {
                        PrimOnSignal();
                    }
                    break;
            }
            return 0;
        }

        private int CheckDiSts()
        {
            int sts = 1;
            for (int i = 0; i < m_Count; i++)
            {
                switch (m_DiStsType[i])
                {
                    case DISTSTYPE.HIGH:
                        {
                            if (XDevice.Instance.FindDiById(m_DiId[i]).STS == false)
                            {
                                sts = 0;
                            }
                            break;
                        }
                    case DISTSTYPE.LOW:
                        {
                            if (XDevice.Instance.FindDiById(m_DiId[i]).STS == true)
                            {
                                sts = 0;
                            }
                            break;
                        }
                    case DISTSTYPE.PLS:
                        {
                            if (XDevice.Instance.FindDiById(m_DiId[i]).PLS == false)
                            {
                                sts = 0;
                            }
                            break;
                        }
                    case DISTSTYPE.PLF:
                        {
                            if (XDevice.Instance.FindDiById(m_DiId[i]).PLF == false)
                            {
                                sts = 0;
                            }
                            break;
                        }
                }
            }
            return sts;
        }
        private int PrimOnSignal()
        {
            if (m_Wait == true)
            {
                if (CheckDiSts() != 0)
                {
                    m_Timeout = false;
                    m_Wait = false;
                    m_Status = 0;
                    m_OnSignaled = true;
                    _mre.Set();
                }
            }
            return 0;
        }
        private int PrimOnWait()
        {
            m_Wait = true;
            return 0;
        }
        private int PrimOnRST()
        {
            m_Wait = false;
            m_Timeout = false;
            m_Status = 0;
            sw.Reset();
            m_OnSignaled = false;
            _mre.Reset();
            _mreTimeout.Reset();
            return 0;
        }
        private int PrimOnStop()
        {
            m_Wait = false;
            m_Timeout = false;
            m_Status = -1;
            sw.Stop();
            return 0;
        }
        private int PrimOnDiContinue()
        {
            if (m_Timeout)
            {
                //_mreTimeout.Set();

                bool bDone = true;
                for (int i = 0; i < m_Count; i++)
                {
                    switch (m_DiStsType[i])
                    {
                        case DISTSTYPE.HIGH:
                            {
                                if (XDevice.Instance.FindDiById(m_DiId[i]).STS == false)
                                {
                                    PostEvent(xStation, XAlarmLevel.TIP, XSysAlarmId.WAITDI_TIMEOUT, m_DiId[i], true);
                                    ShowTimeoutDlg();
                                    bDone = false;
                                }
                                break;
                            }
                        case DISTSTYPE.LOW:
                            {
                                if (XDevice.Instance.FindDiById(m_DiId[i]).STS == true)
                                {
                                    PostEvent(xStation, XAlarmLevel.TIP, XSysAlarmId.WAITDI_TIMEOUT, m_DiId[i], false);
                                    ShowTimeoutDlg();
                                    bDone = false;
                                }
                                break;
                            }
                        case DISTSTYPE.PLS:
                            {
                                if (XDevice.Instance.FindDiById(m_DiId[i]).PLS == false)
                                {
                                    PostEvent(xStation, XAlarmLevel.TIP, XSysAlarmId.WAITDI_TIMEOUT, m_DiId[i], true);
                                    ShowTimeoutDlg();
                                    bDone = false;
                                }
                                break;
                            }
                        case DISTSTYPE.PLF:
                            {
                                if (XDevice.Instance.FindDiById(m_DiId[i]).PLF == false)
                                {
                                    PostEvent(xStation, XAlarmLevel.TIP, XSysAlarmId.WAITDI_TIMEOUT, m_DiId[i], false);
                                    ShowTimeoutDlg();
                                    bDone = false;
                                }
                                break;
                            }
                    }
                }
                if (bDone == true)
                {
                    m_Status = 0;
                    _mreTimeout.Set();
                }
            }
            return 0;
        }

        private int PrimOnContinue()
        {
            if (m_Timeout)
            {
                _mreTimeout.Set();
            }
            return 0;
        }

        public int Continue()
        {
            PostEvent(this, XEventID.DICONTINUE);
            return 0;
        }

        public int Stop()
        {
            PostEvent(xStation, XEventID.STOPMUSTRESET);
            return 0;
        }

        private int Waiting(int timeout)
        {
            m_Status = 0;
            sw.Restart();
            while (true)
            {
                if (m_OnSignaled)
                {
                    sw.Reset();
                    lock (obj)
                    {
                        m_Status = 0;
                    }
                    break;
                }
                if (sw.ElapsedMilliseconds > timeout)
                {
                    if (CheckDiSts() == 0)
                    {
                        sw.Reset();
                        lock (obj)
                        {
                            m_Wait = false;
                            m_Timeout = true;
                            m_Status = -2;
                        }
                    }
                    break;
                }
                Thread.Sleep(1);
            }
            return m_Status;
        }

        public int WaitDi(int[] DiNo, DISTSTYPE[] DiStsType, int Count, int timeout, bool timeoutToBeContinue, string alarmInfo = "")
        {
            lock (obj)
            {
                for (int i = 0; i < Count; i++)
                {
                    m_DiId[i] = DiNo[i];
                    m_DiStsType[i] = DiStsType[i];
                }
                m_Count = Count;
                m_Timeout = false;
                m_Status = 0;
                m_OnSignaled = false;
                m_Wait = false;
                ErrorMessage = "";
            }
            _mre.Reset();
            _mreTimeout.Reset();
            PostEvent(this, XEventID.WAITDI);
            if (timeout > 0)
            {
                if (Waiting(timeout) == -2)
                {
                    bool IsShowDlg = false;
                    for (int i = 0; i < m_Count; i++)
                    {
                        switch (m_DiStsType[i])
                        {
                            case DISTSTYPE.HIGH:
                                {
                                    if (XDevice.Instance.FindDiById(m_DiId[i]).STS == false)
                                    {
                                        ErrorMessage += "[" + XDevice.Instance.FindDiById(m_DiId[i]).Name + "]";
                                        if (timeoutToBeContinue == false)
                                        {
                                            PostEvent(xStation, XAlarmLevel.STOP, XSysAlarmId.WAITDI_TIMEOUT, m_DiId[i], true);
                                        }
                                        else
                                        {
                                            PostEvent(xStation, XAlarmLevel.TIP, XSysAlarmId.WAITDI_TIMEOUT, m_DiId[i], true);
                                            IsShowDlg = true;
                                        }
                                    }
                                    break;
                                }
                            case DISTSTYPE.LOW:
                                {
                                    if (XDevice.Instance.FindDiById(m_DiId[i]).STS == true)
                                    {
                                        ErrorMessage += "[" + XDevice.Instance.FindDiById(m_DiId[i]).Name + "]";
                                        if (timeoutToBeContinue == false)
                                        {
                                            PostEvent(xStation, XAlarmLevel.STOP, XSysAlarmId.WAITDI_TIMEOUT, m_DiId[i], false);
                                        }
                                        else
                                        {
                                            PostEvent(xStation, XAlarmLevel.TIP, XSysAlarmId.WAITDI_TIMEOUT, m_DiId[i], false);
                                            IsShowDlg = true;
                                        }
                                    }
                                    break;
                                }
                            case DISTSTYPE.PLS:
                                {
                                    ErrorMessage += "[" + XDevice.Instance.FindDiById(m_DiId[i]).Name + "]";
                                    if (XDevice.Instance.FindDiById(m_DiId[i]).PLS == false)
                                    {
                                        if (timeoutToBeContinue == false)
                                        {
                                            PostEvent(xStation, XAlarmLevel.STOP, XSysAlarmId.WAITDI_TIMEOUT, m_DiId[i], true);
                                        }
                                        else
                                        {
                                            PostEvent(xStation, XAlarmLevel.TIP, XSysAlarmId.WAITDI_TIMEOUT, m_DiId[i], true);
                                            IsShowDlg = true;
                                        }
                                    }
                                    break;
                                }
                            case DISTSTYPE.PLF:
                                {
                                    ErrorMessage += "[" + XDevice.Instance.FindDiById(m_DiId[i]).Name + "]";
                                    if (XDevice.Instance.FindDiById(m_DiId[i]).PLF == false)
                                    {
                                        if (timeoutToBeContinue == false)
                                        {
                                            PostEvent(xStation, XAlarmLevel.STOP, XSysAlarmId.WAITDI_TIMEOUT, m_DiId[i], false);
                                        }
                                        else
                                        {
                                            PostEvent(xStation, XAlarmLevel.TIP, XSysAlarmId.WAITDI_TIMEOUT, m_DiId[i], false);
                                            IsShowDlg = true;
                                        }
                                    }
                                    break;
                                }
                        }
                    }
                    if (IsShowDlg == true)
                    {
                        ErrorMessage += "等待信号超时";
                        ErrorMessage += alarmInfo;
                       
                         ShowTimeoutDlg();
                        _mreTimeout.WaitOne();
                    }
                }
            }
            else
            {
                _mre.WaitOne(-1);
            }
            return m_Status;
        }

        public int WaitDiSignal(int[] DiNo, DISTSTYPE[] DiStsType, int Count, int timeout)
        {
            lock (obj)
            {
                for (int i = 0; i < Count; i++)
                {
                    m_DiId[i] = DiNo[i];
                    m_DiStsType[i] = DiStsType[i];
                }
                m_Count = Count;
                m_Timeout = false;
                m_Status = 0;
                m_OnSignaled = false;
                m_Wait = false;
                ErrorMessage = "";
            }
            _mre.Reset();
            _mreTimeout.Reset();
            PostEvent(this, XEventID.WAITDI);
            if (timeout > 0)
            {
                Waiting(timeout);
            }
            return m_Status;
        }

        private void ShowTimeoutDlg()
          {
            string s = Convert.ToString(xStation.Name + ":\r\n" + ErrorMessage);
            //s = "是";
            XTimeoutTip xtt = new XTimeoutTip(s);
            //XTimeoutTip xtt = new XTimeoutTip(xStation.Name + ":\r\n" + ErrorMessage);
            xtt.ShowDialog();
            xtt.WaitOne();
            xtt.Reset();
            if (xtt.DialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                Continue();
            }
            else if (xtt.DialogResult == System.Windows.Forms.DialogResult.No)
            {
                Stop();
            }
        }
    }
}
