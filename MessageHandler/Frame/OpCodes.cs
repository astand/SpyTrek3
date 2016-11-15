using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler
{
    public static class OpCodes
    {
        public const UInt16 RRQ = 1;
        public const UInt16 DATA = 3;

        public const UInt16 WRQ = 2;
        public const UInt16 ACK = 4;

        public const UInt16 ERROR = 5;

        public const UInt16 kFirstDataBlockNum = 1;
    }

    public class ReadOperationer : IFrameSpecification
    {
        public Boolean IsData(UInt16 opc) => opc == OpCodes.DATA;

        public Boolean IsHead(UInt16 opc) => opc == OpCodes.RRQ; 
    }

    public class WriteOperationer : IFrameSpecification
    {
        public Boolean IsData(UInt16 opc) => opc == OpCodes.ACK;

        public Boolean IsHead(UInt16 opc) => opc == OpCodes.WRQ;
    }

    public class ErrorOperationer : IFrameSpecification
    {
        public Boolean IsData(UInt16 opc) => false;

        public Boolean IsHead(UInt16 opc) => opc == OpCodes.ERROR;
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


    public static class ErrorCode
    {
        public const UInt16 NoTrek = 6;
        public const UInt16 NonSupportedOpc = 5;
    }

}
