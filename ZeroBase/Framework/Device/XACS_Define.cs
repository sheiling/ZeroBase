using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroBase
{
    public  class XACS_Define
    {
        public const int FAULT_RL = 1 << 0;
        public const int FAULT_LL = 1 << 1;
        public const int FAULT_NETWORK = 1 << 2;
        public const int FAULT_HOT = 1 << 4;
        public const int FAULT_SRL = 1 << 5;
        public const int FAULT_SLL = 1 << 6;
        public const int FAULT_ENCNC = 1 << 7;

        public const int FAULT_DRIVE = 1 << 9;
        public const int FAULT_ENC = 1 << 10;

        public const int FAULT_PE = 1 << 12;
        //public const int FAULT_ES = 0x10000000 << 0;

        public const int MST_ENABLE = 1 << 0;
        public const int MST_INPOS = 1 << 4;
        public const int MST_MOVE = 1 << 5;
        public const int MST_ACC = 1 << 6;

        public const int AMF_WAIT = 0x01 << 0;
        public const int AMF_RELATIVE = 0x01 << 1;
        public const int AMF_VELOCITY = 0x01 << 2;

        public const int IST_IND = 0x01 << 0;

        public const int PST_COMPILED = 0x01 << 0;
        public const int PST_RUN = 0x01 << 1;
        public const int PST_SUSPEND = 0x01 << 2;
    }
}
