using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroBase
{
    /// <summary>
    /// 输入IO
    /// </summary>
    public class XDi : XObject
    {
        private XCard card;
        private int channel;
        private int actDiId;
        private string name;
        private int m_STS;
        private bool m_PLS;
        private bool m_PLF;
        private int m_DiStsLast;
        private string cardname;

        private object obj = new object();

        public XDi(XCard card, int channel, int actDiId, string name, string cardname)
        {
            this.card = card;
            this.channel = channel;
            this.actDiId = actDiId;
            this.name = name;
            this.cardname = cardname;
        }
        
        public int CardId { get; set; }

        public int TaskId { get; set; }

        public int Update()
        {
            int sts = 0;
            int ret = GetDi(ref sts);
            lock(obj)
            {
                if ((sts > 0) && (m_DiStsLast <= 0))
                {
                    PLS = true;
                    PLF = false;
                }
                else if ((sts <= 0) && (m_DiStsLast > 0))
                {
                    PLF = true;
                    PLS = false;
                }
                else
                {
                    PLS = false;
                    PLF = false;
                }

                m_STS = sts;
                m_DiStsLast = m_STS;
            }
            return ret;
        }

        private int GetDi(ref int sts)
        {
            return card.GetDi(channel, actDiId, ref sts);
        }
        public int Channel
        {
            get
            {
                return channel ;
            }
        }
        public int ActId
        {
            get
            {
                return actDiId;
            }
        }

        public int SetId { get; set; }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string CardName
        {
            get
            {
                return cardname;
            }
        }
        public bool STS
        {
            set
            {
                lock (obj)
                {
                    m_STS = value ? 1 : 0;
                }
            }
            get
            {
                lock(obj)
                {
                    return m_STS > 0;
                }
            }
        }

        public bool PLS
        {
            set
            {
                lock (obj)
                {
                    m_PLS = value;
                }
            }
            get
            {
                lock(obj)
                {
                    return m_PLS;
                }
            }
        }

        public bool PLF
        {
            set
            {
                lock (obj)
                {
                    m_PLF = value;
                }
            }
            get
            {
                lock(obj)
                {
                    return m_PLF;
                }
            }
        }

        
    }
}
