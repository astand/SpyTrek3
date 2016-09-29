using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;

namespace StreamHandler
{
    public class SimpleHandler : IStreamHandler
    {
        private Byte[] stream_out_packet;

        private Crc16 crc = new Crc16();

        private ByteStuffer stuffer = new ByteStuffer();

        public SimpleHandler() { }

        public Byte[] Data() => stream_out_packet;

        public Byte[] PackPacket(Byte[] cleanbuf)
        {
            var ret_arr = crc.GetCheckedArray(cleanbuf);
            return stuffer.ArrayToStuff(ret_arr);
        }

        public Boolean UnpackPacket(Byte bt)
        {
            if (stuffer.TryStripDataFlow(bt) > 0)
            {
                stream_out_packet = stuffer.UnstuffedToArray();
                Boolean retval = crc.CheckValidCRCInArray(stream_out_packet, stream_out_packet.Length);

                if (retval)
                {
                    Array.Resize(ref stream_out_packet, stream_out_packet.Length - 2);
                }
                return retval;
            }
            return false;
        }

        public Boolean ExtractPacket(IPipeReader reader)
        {
            Int32 byteFromStream;
            reader.DataAvailable();

            while ((byteFromStream = reader.ReadByte()) >= 0)
            {
                if (UnpackPacket(Convert.ToByte(byteFromStream)))
                    return true;
            }
            return false;
        }
    }
}

