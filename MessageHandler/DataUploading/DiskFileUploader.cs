using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.DataUploading
{
    class DiskFileUploader : IDataUploader
    {

        private string m_fpath = String.Empty;

        private long m_fsize;

        public DiskFileUploader(string rootpath)
        {
            m_fpath = rootpath;
            RefreshData();
        }


        public Int32 Length => (Int32)m_fsize;

        public Int32 ReadData(Byte[] dst, Int32 offset, Int32 length)
        {
            try
            {
                using (var fs = new FileStream(m_fpath, FileMode.Open, FileAccess.Read))
                {
                    fs.Position = offset;
                    return fs.Read(dst, 0, length);
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException || ex is FileNotFoundException)
                {
                    return 0;
                }
                else
                    throw;
            }
        }

        public Boolean RefreshData()
        {
            /// may be here must be action as in constructor
            m_fsize = 0;

            if (File.Exists(m_fpath))
            {
                m_fsize = new FileInfo(m_fpath).Length;
            }

            return true;
        }
    }
}
