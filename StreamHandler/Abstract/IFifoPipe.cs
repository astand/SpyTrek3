using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamHandler.Abstract
{
    public interface IPipeWritable
    {
        /// <summary>
        /// Write new chuck of data to Data Pipe
        /// </summary>
        /// <param name="array"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        Int32 Write(Byte[] array, Int32 length = -1);

        /// <summary>
        /// Write one byte to Data Pipe
        /// </summary>
        /// <param name="bt"></param>
        /// <returns></returns>
        Int32 Write(Byte bt);
    }

    public interface IPipeReadable
    {
        long DataAvailable();

        /// <summary>
        /// Read one byte from Data Pipe
        /// </summary>
        /// <returns></returns>
        Int32 ReadByte();

        /// <summary>
        /// Read @count data of array.Length or count of availible data in Pipe
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Int32 Read(ref Byte[] array, Int32 count);
    }
}

