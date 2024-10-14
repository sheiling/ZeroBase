using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZeroBase
{
    public class XCommandCard9112 : XCommandCard
    {
        private double zeroV = 65535 / 2;
        private double dataV = 32768 / 10;
        private ushort raw = 0;
        public short m_dev;
        public override int Register(int actCardId)
        {
            return m_dev = DASK.Register_Card(DASK.PCI_9112, (ushort)(actCardId));
        }
        
        public override int ReadChannel(int actCardId, int channel, out double value)
        {
            value = 0;
            DASK.AI_ReadChannel((ushort)actCardId, (ushort)channel, DASK.AD_B_10_V, out ushort rawData);
            DASK.AI_VoltScale((ushort)actCardId, DASK.AD_B_10_V, rawData, out double voltage);
            value = Math.Round((voltage * 5),3);
            return 0;
        }

        //public override int ReadChannel(int actCardId, int channel, out ushort value)
        //{
        //    return DASK.AI_ReadChannel((ushort)actCardId, (ushort)channel, DASK.AD_B_10_V, out value);
        //}

        public override int WriteChannel(int actCardId, int channel, double value)
        {
            return DASK.AO_WriteChannel((ushort)actCardId, (ushort)channel, (short)(value * (4096 / 10)));
        }
    }
}
