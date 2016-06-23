using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamHandler
{
    interface IStreamHandler
    {
        byte[] PackPacket(byte[] cleanbuf);

        /// <summary>
        /// pass new chunk of data
        /// </summary>
        /// <param name="packedbuf"></param>
        /// <returns>return true if good packet recived</returns>
        bool UnpackPacket(byte[] packedbuf);

        bool UnpackPacket(Stream stream);

        bool UnpackPacket(Byte bt);

        byte[] GetUnpacked();
    }
}
