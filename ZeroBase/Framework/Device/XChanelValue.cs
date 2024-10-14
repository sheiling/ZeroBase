using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ZeroBase
{
    public class XChannelValue : XObject
    {
        private XCard card;
        private int channel;
        private string name;
        private double m_Value;
        private bool ischeck;
        public XChannelValue(XCard card, int channel, string name, bool ischeck = false)
        {
            this.card = card;
            this.channel = channel;
            this.name = name;
            this.ischeck = ischeck;
        }

        public int CardId { get; set; }

        public int ChannelId { get; set; }

        public bool IsCheck
        {
            get { return ischeck; }
        }

        public int Update()
        {
            return card.ReadChannel(channel, out m_Value);
        }

        public int ReadValue(out double value)
        {
            return card.ReadChannel(channel, out value);
        }

        public void SetVaule(double value)
        {
            card.WriteChannel(channel, value);
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public double Value
        {
            get
            {
                lock (this)
                {
                    return this.m_Value;
                }
            }
        }        
    }
}
