using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroBase
{
    public class XCommandCard7442 : XCommandCard
    {
        public override int Register(int actCardId)
        {
            var ret = DASK.Register_Card(DASK.PCI_7442, (ushort)actCardId);
            return ret;
        }

        public override int Update(int actCardId)
        {
            uint diValue0, doValue0;
            DASK.DI_ReadPort((ushort)actCardId, 0, out diValue0);
            DI_Data[0] = (int)diValue0;
            DASK.DO_ReadPort((ushort)actCardId, 0, out doValue0);
            DO_Data[0] = (int)doValue0;

            uint diValue1, doValue1;
            DASK.DI_ReadPort((ushort)actCardId, 1, out diValue1);
            DI_Data[1] = (int)diValue1;
            DASK.DO_ReadPort((ushort)actCardId, 1, out doValue1);
            DO_Data[1] = (int)doValue1;

            return 0;
        }

        public override int SetDo(int actCardId, int channel, int index, int sts)
        {
            if (index >= 32)
            {
                channel = 1;
                index = index - 32;
            }
            return DASK.DO_WriteLine((ushort)actCardId, (ushort)channel, (ushort)index, (ushort)sts);
        }
        public override int GetDo(int actCardId, int channel, int index, ref int sts)
        {
            //ushort _sts = 0;
            //short ret = DASK.DO_ReadLine((ushort)actCardId, (ushort)channel, (ushort)index, out _sts);
            //sts = _sts;
            //return ret;
            if (index >= 32)
            {
                channel = 1;
                index = index - 32;
            }
            sts = (DO_Data[channel] >> index) & 1;
            return 0;
        }
        public override int GetDi(int actCardId, int channel, int index, ref int sts)
        {
            //ushort _sts = 0;
            //short ret = DASK.DI_ReadLine((ushort)actCardId, (ushort)channel, (ushort)index, out _sts);
            //sts = _sts;
            //return ret;
            if (index >= 32)
            {
                channel = 1;
                index = index - 32;
            }
            sts = (DI_Data[channel] >> index) & 1;
            return 0;
        }

        public override int APS_DIO_SetCOSInterrupt32(int actCardId, byte Port, uint ctl, out uint hEvent, bool ManualReset)
        {
            return DASK.DIO_SetCOSInterrupt32((ushort)actCardId, Port, ctl, out hEvent, ManualReset);
        }
        public override int APS_DIO_INT1_EventMessage(int actCardId, int index, uint windowHandle, uint message, MulticastDelegate callbackAddr)
        {
            int ret = 0;

            switch(index)
            {
                case 0:
                    ret = DASK.DIO_INT1_EventMessage((ushort)actCardId, DASK.INT1_COS0, windowHandle, message, callbackAddr);
                    break;
                case 1:
                    ret = DASK.DIO_INT1_EventMessage((ushort)actCardId, DASK.INT1_COS1, windowHandle, message, callbackAddr);
                    break;
            }
            return ret;
        }
    }
}
