using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroBase
{
    public class XCommandCard9114 : XCommandCard
    {
        private ushort raw = 0;

        public override int Register(int actCardId)
        {
            return DASK.Register_Card(DASK.PCI_9114DG, (ushort)(actCardId));
        }

        public override int ReadChannel(int actCardId, int channel, out double value)
        {
            value = 0;
            int ret = DASK.AI_ReadChannel((ushort)actCardId, (ushort)channel, DASK.AD_B_10_V, out raw);
            return DASK.AI_VoltScale((ushort)actCardId, DASK.AD_B_10_V, raw, out value);
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
