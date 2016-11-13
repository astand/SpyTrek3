using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.DataFormats
{

    public static class DateTimeUtil
    {
        /// <summary>
        /// Create DateTime instance from packed to byte array
        /// format [year, month, day, hour, min, sec]
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public static DateTime GetDateTime(byte[] buff, int offset = 0)
        {
            DateTime ret = new DateTime(
                buff[offset] + 2000,
                buff[offset + 1],
                buff[offset + 2],
                buff[offset + 3],
                buff[offset + 4],
                buff[offset + 5]);

            return ret;
        }

        public static string ToDirectory(this DateTime dt) => $"{dt.Year:D4}/{dt.Month:D2}/{dt.Day:D2}";

        public static string ToJS(this DateTime dt) => dt.ToLocalTime().ToString(@"yyyy.MM.dd HH:mm");

        /// <summary>
        /// Method must implement reloading of timer counter.
        /// </summary>
        /// <param name="tim"></param>
        public static void Reset(this System.Timers.Timer tim)
        {
            tim.Stop();
            tim.Start();
        }
    }

    
}
