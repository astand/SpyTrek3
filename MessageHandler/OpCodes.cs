using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler
{
    public static class OpCodes
    {
        public const UInt16 RRQ =   (1);
        public const UInt16 OPC_WRQ =   (2);
        public const UInt16 DATA =  (3);
        public const UInt16 ACK =   (4);
        public const UInt16 OPC_ERR =   (5);
    }

    public static class FiledID
    {
        public const UInt16 None = 0;
        public const UInt16 Info = 4;
        public const UInt16 Echo = 128;
        public const UInt16 Filenotes = 1;
        public const UInt16 Track = 2;
        public const UInt16 Firmware = 3 | 1 << 14; 
    }
}
