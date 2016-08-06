using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;

namespace MessageHandler
{
    public class ReadRequest : IStreamData
    {
        UInt16 m_file_id;
        Int32 m_id_length = sizeof(UInt16);
        public ReadRequest(UInt16 fileId)
        {
            m_file_id = fileId;
        }


        public Byte[] SerializeToByteArray()
        {
            Byte[] retval = BitConverter.GetBytes((UInt16)(1));
            Array.Resize(ref retval, retval.Length + m_id_length);
            Array.Copy(BitConverter.GetBytes(m_file_id), 0, retval, 2, m_id_length);
            return retval;
        }
    }
}
