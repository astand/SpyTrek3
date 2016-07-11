using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;
using System.Diagnostics;
using System.IO;

namespace StreamHandler
{
    /* NetworkPipe uses inner local buffer for caching data from source Stream
     * may be need more tests and it will possible to change this behaviour
     * */
    public class NetworkPipe : IPipeReader, IPipeWriter
    {
        private readonly Stream m_stream;

        private Byte[] m_inner_buff;

        private Int32 local_buffer_size;

        private Int32 m_index_of_output;

        private Int32 INNER_BUFF_SIZE = 10000;

        public NetworkPipe(Stream stream)
        {
            if (stream == null)
                throw new NullReferenceException($"Stream cannot be Null");

            m_stream = stream;
            m_inner_buff = new Byte[INNER_BUFF_SIZE];
        }
        public long DataAvailable() => 0;
        public Int32 Read(ref Byte[] array, Int32 count) => m_stream.Read(array, 0, count);

        //public Int32 ReadByte() => m_stream.ReadByte();
        public Int32 ReadByte()
        {
            if (local_buffer_size == 0)
            {
                /* Data presence in income pipe and local buffer empty */
                var retval = m_stream.Read(m_inner_buff, 0, INNER_BUFF_SIZE);
                local_buffer_size = retval;
                m_index_of_output = 0;
            }

            if (local_buffer_size > 0)
            {
                local_buffer_size--;
                return m_inner_buff[m_index_of_output++];
            }
            return -1;
        }

        public Int32 Write(Byte bt)
        {
            m_stream.WriteByte(bt);
            return 0;
        }

        public Int32 Write(Byte[] array, Int32 length = -1)
        {
            Int32 count_to_write = (length > 0 && length < array.Length) ? (length) : (array.Length);

            m_stream.Write(array, 0, count_to_write);

            return count_to_write;
        }
    }
}
