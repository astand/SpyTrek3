using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.DataUploading
{
    public interface IDataUploader
    {
        /// <summary>
        /// Copy data to destination buffer. As much as permits by the dst.Length
        /// </summary>
        /// <param name="dst">Destination buffer</param>
        /// <param name="offset">Offset in data source</param>
        /// <returns>Count of successful loaded bytes</returns>
        Int32 ReadData(byte[] dst, Int32 offset, Int32 length);
        
        /// <summary>
        /// Refresh data source (when source stream cached in memory)
        /// </summary>
        /// <returns>true if refreshed successful</returns>
        bool RefreshData();

        /// <summary>
        /// Get length of data source in bytes
        /// </summary>
        /// <returns>Length of data source</returns>
        Int32 Length { get; }
    }
}
