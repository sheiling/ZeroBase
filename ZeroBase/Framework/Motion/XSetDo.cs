using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;


namespace ZeroBase
{
    public enum DOSTSTYPE
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
        PLF = 3,
    }

    public class XSetDo : XCtrlEventHandler
    {
        bool m_Busy;
        bool m_Stop;
        int m_Status;
        int m_Count;
        int m_Count1;
        DOSTSTYPE[] m_DoStsType; 
        int[] m_DoId;
        DOSTSTYPE[] m_DoStsType1;
        int[] m_DoId1;
        ManualResetEvent _mre;
        private Stopwatch sw;
        private XStation xStation;

        public XSetDo(XStation xStation)
        {
            m_Status = 0;
            m_Busy = false;
            m_Stop = false;
            m_DoStsType = new DOSTSTYPE[8];
            m_DoStsType1 = new DOSTSTYPE[8];
            sw = new Stopwatch();
            m_DoId = new int[8];
            m_DoId1 = new int[8];
            _mre = new ManualResetEvent(false);
            this.xStation = xStation;
        }

        public override int HandleEvent(XEvent xEvent)
        {
            switch (xEvent.EventID)
            {
                case XEventID.SIGNAL:
                    PrimOnSignal();
                    break;
                case XEventID.SETDO:
                    PrimOnSetDo();
                    break;
                case XEventID.RST:
                    PrimOnRST();
                    break;
                case XEventID.STOPMUSTRESET:
                    PrimOnStop();
                    break;
            }

            return 0;
        }

        private int PrimOnSignal()
        {
            if (sw.ElapsedMilliseconds > 200)
            {
                for (int i = 0; i < m_Count1; i++)
                {
                    switch (m_DoStsType1[i])
                    {
                        case DOSTSTYPE.PLS:
                            XDevice.Instance.FindDoById(m_DoId1[i]).SetDo(0);
                            sw.Reset();
                            break;
                        case DOSTSTYPE.PLF:
                            XDevice.Instance.FindDoById(m_DoId1[i]).SetDo(1);
                            sw.Reset();
                            break;
                    }
                }
            }
            return 0;
        }

        private int PrimOnSetDo()
        {
            if (m_Stop)
            {
                return -1;
            }
            for (int i = 0; i < m_Count; i++)
            {
                switch (m_DoStsType[i])
                {
                    case DOSTSTYPE.HIGH:
                        {
                            XDevice.Instance.FindDoById(m_DoId[i]).SetDo(1);
                        }
                        break;
                    case DOSTSTYPE.LOW:
                        {
                            XDevice.Instance.FindDoById(m_DoId[i]).SetDo(0);
                        }
                        break;
                    case DOSTSTYPE.PLS:
                        {
                            XDevice.Instance.FindDoById(m_DoId[i]).SetDo(1);
                            sw.Start();
                        }
                        break;
                    case DOSTSTYPE.PLF:
                        {
                            XDevice.Instance.FindDoById(m_DoId[i]).SetDo(0);
                            sw.Start();
                        }
                        break;
                }
            }
            PrimOnDone();
            return 0;
        }
        private int PrimOnRST()
        {
            lock (this)
            {
                m_Busy = false;
            }
            m_Status = 0;
            m_Stop = false;
            _mre.Reset();
            return 0;
        }
        private int PrimOnDone()
        {
            lock (this)
            {
                m_Busy = false;
            }
            m_Status = 0;
            m_Stop = false;
            _mre.Set();
            return 0;
        }
        private int PrimOnStop()
        {
            lock (this)
            {
                m_Busy = false;
            }
            m_Status = -1;
            m_Stop = true;
            return 0;
        }

        private int WaitDone()
        {
            _mre.WaitOne(-1);
            _mre.Reset();
            return m_Status;
        }

        /// <summary>
        /// SetDo
        /// </summary>
        /// <param name="doId"></param>
        /// <param name="DoStsType"></param>
        /// <param name="Count"></param>
        /// <returns>running:0/stop:-1/timeout:2</returns>
        public int SetDo(int[] doId, DOSTSTYPE[] DoStsType, int Count)
        {
            lock (this)
            {
                if (m_Busy)
                {
                    return -1;
                }
                m_Busy = true;
                for (int i = 0; i < Count; i++)
                {
                    m_DoId[i] = doId[i];
                    m_DoStsType[i] = DoStsType[i];
                }
                m_Count = Count;
                m_Count1 = 0;
                for (int i = 0; i < Count; i++)
                {
                    if (m_DoStsType[i] == DOSTSTYPE.PLS || m_DoStsType[i] == DOSTSTYPE.PLF)
                    {
                        m_DoId1[m_Count1] = m_DoId[i];
                        m_DoStsType1[m_Count1] = m_DoStsType[i];
                        m_Count1++;
                    }
                }
                m_Status = 0;
                sw.Reset();
            }
            _mre.Reset();
            PostEvent(this, XEventID.SETDO);
            return WaitDone();
        }
        
	
    }
}
