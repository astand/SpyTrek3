using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;
using System.IO;

namespace StreamHandler
{
    public class MemoryPipe : IPipeReadable, IPipeWritable
    {
        private readonly MemoryStream m_stream;
        public MemoryPipe(MemoryStream stream)
        {
            if (stream == null)
                throw new NullReferenceException($"Stream cannot be Null");

            m_stream = stream;
        }

        public long DataAvailable() => m_stream.Length;

        public Int32 Read(ref Byte[] array, Int32 count)
        {
            Int32 resultCount = 0;

            while (resultCount < array.Length || resultCount < count)
            {
                Int32 out_byte = m_stream.ReadByte();

                if (out_byte < 0)
                    break;

                array[resultCount] = Convert.ToByte(out_byte);
            }

            return resultCount;
        }

        public Int32 ReadByte() => m_stream.ReadByte();

        public Int32 Write(Byte bt)
        {
            m_stream.WriteByte(bt);
            return 1;
        }

        public Int32 Write(Byte[] array, Int32 length = -1)
        {
            int writable_count = (length > 0 && length < array.Length) ? (length) : (array.Length);

            for (int i = 0; i < writable_count; i++)
                Write(array[i]);

            return writable_count;
        }
    }
}
