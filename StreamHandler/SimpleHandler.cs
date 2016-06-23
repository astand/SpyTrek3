using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamHandler
{
    public class SimpleHandler : IStreamHandler
    {
        Byte[] m_out_packet;

        private Crc16 crc = new Crc16();

        private ByteStuffer stuffer = new ByteStuffer();

        public SimpleHandler()
        {

        }

        public Byte[] GetUnpacked() => m_out_packet;

        public Byte[] PackPacket(Byte[] cleanbuf)
        {
            var ret_arr = crc.GetCheckedArray(cleanbuf);
            return stuffer.GetStuffed(ret_arr);
        }

        public Boolean UnpackPacket(Byte[] packedbuf)
        {
            var ret_arr = stuffer.GetUnstuffed(packedbuf);
            return crc.GetUncheckedArray(ret_arr, out m_out_packet);
        }
        
        public Boolean UnpackPacket(Stream bt)
        {
            throw new NotImplementedException();
        }

        public Boolean UnpackPacket(Byte bt)
        {
            if (stuffer.GetUnstuffed(bt) > 0)
            {
                var crcArray = stuffer.GetInnerUnstuffed();
                Boolean retval = crc.GetUncheckedArray(crcArray, out m_out_packet);

                Debug.WriteLine($"Parsed array CRC valid ? = {retval}. Output array length = {m_out_packet.Length}");
                return retval;
            }
            return false;
        }

    }
}

