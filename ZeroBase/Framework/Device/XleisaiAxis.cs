using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using APS_Define_W32;
using APS168_W32;

namespace ZeroBase
{
    public class XleisaiAxis : XObject,IAxis
    {
        private int actAxisId;
        private double lead;
        private XCard card;
        private string name;
        private int m_MotionIO;
        private int m_MotionSts;
        private int m_MotionPos;
        private int m_CommandPos;
        private bool isHomeOk;
        private bool hasEStoped = true;
        private bool m_HasServoOff = false;
        private bool m_ServoStsLast = false;
        private bool m_Feedback = true;
        private bool checkDone = true;
        private bool IsHomeDone = true;
        private double [] m_Value =new double[8];

        public XleisaiAxis(int actAxisId, double lead, XCard card, string name)
        {
            this.actAxisId = actAxisId;
            this.lead = lead;
            this.card = card;
            this.name = name;
        }

      


        public int ActId
        {
            get { return this.actAxisId; }
        }

        public int SetId { get; set; }

        public int CardId { get; set; }

        public int TaskId { get; set; }

        public XAxisDirection AxisDirection { get; set; }

        public AxisStyle CurrentAxis
        {
            get { return AxisStyle.Leisai_Axis; }
        }
        public bool IsFeedback
        {
            get { return this.m_Feedback; }
            set { this.m_Feedback = value; }
        }

        public int SetServo(bool on)
        {
            return card.SetServo(actAxisId, on);
        }
        public int GoHome()
        {
            return card.GoHome(actAxisId);
        }

        public int SetPosition(int position)
        {
            return card.SetPosition(actAxisId, position,lead);
        }

        public int CleanALM()
        {
            return card.ClearALM(actAxisId);
        }

        public int MoveAbs(double position, double vel)
        {
            return card.MoveAbs(actAxisId, XConvert.MM2PULS(position, lead), XConvert.MM2PULS(vel, lead));
        }
        public int MoveJog(double dir)
        {
            return card.MoveJog(actAxisId, (int)dir);
        }

        public int MoveRel(double distance,double vel)
        {
            return card.MoveRel(actAxisId, XConvert.MM2PULS(distance, lead), XConvert.MM2PULS(vel, lead));
        }
        public int Stop()
        {
            return card.Stop(actAxisId);
        }
        public int EStop()
        {
            return card.EStop(actAxisId);
        }
        public int Update()
        {
            lock (this)
            {
                int sts = 0;
                card.GetMotionIo(actAxisId, ref sts);
                m_MotionIO = sts;
                card.GetMotionSts(actAxisId, ref sts);
                m_MotionSts = sts;
                card.GetMotionPos(actAxisId, ref sts);
                m_MotionPos = sts;
                card.GetCommandPos(actAxisId, ref sts);
                m_CommandPos = sts;

                if (IsSVON == false && m_ServoStsLast == true)
                {
                    m_HasServoOff = true;
                }
                m_ServoStsLast = IsSVON;

                if (card.CheckMoveDone((ushort)actAxisId)==1)
                {
                    checkDone = true;
                }
                else
                {
                    checkDone = false;
                }
                if (card.CheckHomeDone((ushort)actAxisId)==1)
                {
                    IsHomeDone = true;
                }
                else
                {
                    IsHomeDone = false;
                }
                return 0;
            }
        }
        public int SetHome(bool b)
        {
            lock (this)
            {
                isHomeOk = b;
                m_HasServoOff = false;
                return 0;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public bool IsALM
        {
            get
            {
                lock (this)
                {
                    return XConvert.BitEnable(m_MotionIO, Xleisai_Define.MIO_ALM);
                }
            }
        }
        public bool IsPEL
        {
            get
            {
                lock (this)
                {
                    return XConvert.BitEnable(m_MotionIO, Xleisai_Define.MIO_PEL);
                }
            }
        }
        public bool IsMEL
        {
            get
            {
                lock (this)
                {
                    return XConvert.BitEnable(m_MotionIO, Xleisai_Define.MIO_MEL);
                }
            }
        }
        public bool IsORG
        {
            get
            {
                lock (this)
                {
                    return XConvert.BitEnable(m_MotionIO, Xleisai_Define.MIO_ORG);
                }
            }
        }
        public bool IsEMG
        {
            get
            {
                lock (this)
                {
                    return XConvert.BitEnable(m_MotionIO, Xleisai_Define.MIO_EMG);
                }
            }
        }
        public bool IsSVON
        {
            get
            {
                lock (this)
                {
                    return XConvert.BitEnable(m_MotionSts, Xleisai_Define.MTS_SVON);
                }
            }
        }
        public bool HasSVONOFF
        {
            get
            {
                lock (this)
                {
                    return m_HasServoOff;
                }
            }
        }

        public bool IsHomeD
        {
            get
            {
                lock (this)
                {
                    return IsHomeDone;
                  
                }
            }
        }

        public bool IsMDN
        {
            get
            {
                lock (this)
                {
                    return checkDone;
                    //return XConvert.BitEnable(m_MotionSts, Xleisai_Define.MTS_MDN);
                }
            }
        }
        public bool IsHMV
        {
            get
            {
                lock (this)
                {
                    return XConvert.BitEnable(m_MotionSts, Xleisai_Define.MTS_ALM);
                }
            }
        }
        public bool IsASTP
        {
            get
            {
                lock (this)
                {
                 
                    return XConvert.BitEnable(m_MotionSts, Xleisai_Define.MTS_OTHER);
                }
            }
        }
        public bool IsHomeOk
        {
            get
            {
                lock (this)
                {
                    return isHomeOk;
                }
            }
        }
        public bool HasEStoped
        {
            get
            {
                lock (this)
                {
                    return hasEStoped;
                }
            }
            set
            {
                lock (this)
                {
                    hasEStoped = value;
                }
            }
        }
        public double POS
        {
            get 
            {
                lock (this)
                {
                    return XConvert.PULS2MM(m_MotionPos, lead);
                }
            }
        }
        public double CommandPOS
        {
            get
            {
                lock (this)
                {
                    return XConvert.PULS2MM(m_CommandPos, lead);
                }
            }
        }
        public int PULS
        {
            get
            {
                lock (this)
                {
                    return m_MotionPos;
                }
            }
        }

        public int GetADinput(ushort channel, ref double Value)
        {
            return card.GetADinput(channel, ref Value);
        }

        public int SetAxisAccAndDec(double acc, double dec)
        {
            return card.SetAxisAccAndDec(actAxisId, lead, acc, dec);
        }
        public int SetJerk(double jerkvalue)
        {
            return 0;
        }
        public int SetStopDec( double dec)
        {
            return 0;
        }

        public int APS_SetAxisParam(APS_Define PRA, double value)
        {
            return card.APS_SetAxisParam(actAxisId, lead, PRA, value);
        }
        public int APS_SetAxisJogParam(int mode, int dir, double acc, double dec,int vel)
        {
            return card.APS_SetJogParam(actAxisId,mode,dir,lead,acc,dec,vel);
        }

        public int APS_SetBacklashEn(int on)
        {
            return card.APS_SetBacklashEnable(actAxisId, on);
        }

    }
    
}
