using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroBase
{
    class XAPS_Define : XObject
    {
        public const int MIO_ALM = 0x01 << 0;
        public const int MIO_PEL = 0x01 << 1;
        public const int MIO_MEL = 0x01 << 2;
        public const int MIO_ORG = 0x01 << 3;
        public const int MIO_EMG = 0x01 << 4;
        public const int MIO_SVON = 0x01 << 5;

        public const int MTS_HMV = 0x01 << 0;
        public const int MTS_MDN = 0x01 << 1;
        public const int MTS_ASTP = 0x01 << 2;
    }
}
