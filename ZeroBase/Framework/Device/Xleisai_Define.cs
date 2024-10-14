using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroBase
{
    class Xleisai_Define : XObject
    {
        public const int MIO_ALM = 0x01 << 0;
        public const int MIO_PEL = 0x01 << 1;
        public const int MIO_MEL = 0x01 << 2;
        public const int MIO_EMG = 0x01 << 3;
        public const int MIO_ORG = 0x01 << 4;

        public const int MIO_SPEL = 0x01 << 6;
        public const int MIO_SMEL = 0x01 << 7;
        public const int MIO_INP = 0x01 << 8;
        public const int MIO_EZ = 0x01 << 9;
       
        public const int MIO_DSTP = 0x01 << 11;


        public const int MTS_MDN = 0x01 << 0;
        public const int MTS_ALM = 0x01 << 1;
        public const int MTS_OTHER = 0x01 << 15;

        public const int MTS_SVON = 0x01 << 10;
    }
}
