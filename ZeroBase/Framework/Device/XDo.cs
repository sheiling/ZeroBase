using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ZeroBase
{
    /// <summary>
    /// 输出IO
    /// </summary>
    public class XDo : XObject
    {
        private XCard card;
        private int channel;
        private int actDoId;
        private string name;
        private int m_STS;
        private string cardname;
        private bool isInterference;
        private int interferenceThings;
        private double interferencepos;
        private DOSTSTYPE interferestatus;

        public XDo(XCard card, int channel, int actDoId, string name, string cardname, bool isInterference = false, int InterferenceThings = -1, double pos = 0, DOSTSTYPE status = DOSTSTYPE.HIGH)
        {
            this.card = card;
            this.channel = channel;
            this.actDoId = actDoId;
            this.name = name;
            this.cardname = cardname;
            this.isInterference = isInterference;
            this.interferenceThings = InterferenceThings;
            this.interferencepos = pos;
            this.interferestatus = status;
        }
        public DOSTSTYPE InterfereStatus
        {
            get { return this.interferestatus; }
            set { this.interferestatus = value; }
        }
        public bool IsInterference
        {
            get { return this.isInterference; }
            set { this.isInterference = value; }
        }
        public int IsInterferenceThing
        {
            get { return this.interferenceThings; }
            set { this.interferenceThings = value; }
        }
        public double InterferencePos
        {
            get { return this.interferencepos; }
            set { this.interferencepos = value; }
        }
        public int CardId { get; set; }

        public int TaskId { get; set; }

        public int Update()
        {
            int sts = 0;
            int ret;
            lock (this)
            {
                ret = GetDo(ref sts);
                m_STS = sts;
            }
            return ret;
        }

        public int SetDo(int sts)
        {
            return card.SetDo(channel, actDoId, sts);
        }

        private int GetDo(ref int sts)
        {
            return card.GetDo(channel, actDoId, ref sts);
        }

        public bool STS
        {
            get
            {
                lock (this)
                {
                    return m_STS > 0;
                }
            }
        }
        public int Channel
        {
            get
            {
                return channel;
            }
        }
        public int ActId
        {
            get
            {
                return actDoId;
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
    }
}
