using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;

namespace StreamHandler
{
    public class DataAck : IStreamData
    {
        private UInt16 m_id;

        public DataAck(UInt16 id)
        {
            m_id = id;
        }

        public Byte[] SerializeToByteArray()
        {
            byte[] retval = new Byte[4];
            Array.Copy(BitConverter.GetBytes((UInt16)(4)), 0, retval, 0, sizeof(UInt16));
            Array.Copy(BitConverter.GetBytes(m_id), 0, retval, 2, sizeof(UInt16));

            return retval;
        }
    }
}
