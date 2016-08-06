using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.DataUploading
{
    public class FileUploader : IDataUploader
    {
        private string m_fpath;

        private long m_fsize;

        MemoryStream m_firmstream = new MemoryStream();

        public FileUploader(string rootpath)
        {
            m_fpath = rootpath;
        }
        public Int32 Length => (Int32)(m_fsize);

        /// <summary>
        /// Read data from source to dst array
        /// </summary>
        /// <param name="dst">Destination array</param>
        /// <param name="offset">Data source offset</param>
        /// <param name="length">Max data count</param>
        /// <returns>Count of successful read bytes</returns>
        public Int32 ReadData(Byte[] dst, Int32 offset, Int32 length)
        {
            Int32 ret_array_length = FixLength(offset, length);

            m_firmstream.Position = offset;
            ret_array_length = m_firmstream.Read(dst, 0, ret_array_length);

            return ret_array_length;
        }

        public bool RefreshData()
        {
            /* If uses caching to memory this command will actuate memory reinit process */
            try
            {
                m_fsize = new FileInfo(m_fpath).Length;

                using (var fs = new FileStream(m_fpath, FileMode.Open, FileAccess.Read))
                {
                    fs.Position = 0;
                    fs.CopyTo(m_firmstream);
                }
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException || e is ArgumentException)
                {
                    Debug.WriteLine($"Refreshing fault. {e.ToString()}");
                    m_fsize = 0;
                    return false;
                }
                else
                    throw;
            }
       
            return true;

        }

        /// <summary>
        /// It will block attempts to read data out from valid stram range
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private Int32 FixLength(Int32 offset, Int32 length)
        {
            if (offset > Length)
                return 0;
            else if (offset + length > Length)
                return Length - offset;
            return length;
        }
    }
}
