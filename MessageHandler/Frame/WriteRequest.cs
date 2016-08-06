using StreamHandler.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler
{
    public class WriteRequest : FramePacket
    {
        public WriteRequest(UInt16 fid, Int32 fsize) : base(OpCodes.WRQ, fid, BuildPayload(fsize))
        {

        }

        static private Byte[] BuildPayload(Int32 fsize)
        {
            var retarray = new Byte[6];

            Array.Copy(BitConverter.GetBytes(0x0401), 0, retarray, 0, 2);
            Array.Copy(BitConverter.GetBytes(fsize), 0, retarray, 2, 4);

            return retarray;
        }
    }
}
