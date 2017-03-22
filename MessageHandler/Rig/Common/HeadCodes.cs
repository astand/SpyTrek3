using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.Rig.Common
{
    public enum OpCode : UInt16
    {
        RRQ = 1,
        WRQ = 2,
        DATA = 3,
        ACK = 4,
        ERR = 5
    }

    public enum OpID : UInt16
    {
        None = 0,
        TrekList = 1,
        SoleTrek = 2,
        Firmware = 3,
        Info = 4,
        Echo = 128
    }
}
