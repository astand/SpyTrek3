using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;

namespace StreamHandler
{
    interface IStreamHandler
    {
        /// <summary>
        /// wraps clean buff to proper for stream packet and return it
        /// </summary>
        /// <param name="cleanbuf"></param>
        /// <returns></returns>
        byte[] PackPacket(byte[] cleanbuf);

        /// <summary>
        /// Read available data form reader and try strip valid packet form it
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        bool ExtractPacket(IPipeReader reader);

        byte[] Data();
    }
}
