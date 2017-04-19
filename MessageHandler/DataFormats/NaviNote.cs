using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.DataFormats
{
    public class NaviNote
    {
        const double KoorPrescaler = 1000000.0;
        public DateTime timePoint;
        public Int16 altitude;
        public Int32 lafull;
        public Int32 lofull;
        public UInt16 spd;
        public UInt32 accum_dist;
        public UInt16 spare;
        public UInt16 adcsrc;
        public UInt16 adc1;

        StringBuilder str = new StringBuilder(128);

        public static Int32 Lenght => (6 + (4 * 3) + (2 * 3) + (2 * 2));

        public NaviNote() { }

        public bool TryParse(Byte[] buff, Int32 offset)
        {
            try
            {
                Parse(buff, offset);
                return true;
            }
            catch (ArgumentOutOfRangeException) { }
            catch (IndexOutOfRangeException) { }
            catch (ArgumentException) { }

            return false;
        }

        private void Parse(byte[] b, int offset)
        {
            timePoint = DateTimeUtil.GetDateTime(b, offset);
            altitude = BitConverter.ToInt16(b, offset += 6);
            lafull = BitConverter.ToInt32(b, offset += 2);
            lofull = BitConverter.ToInt32(b, offset += 4);
            spd = BitConverter.ToUInt16(b, offset += 4);
            accum_dist = BitConverter.ToUInt32(b, offset += 2);
            spare = BitConverter.ToUInt16(b, offset += 4);
            adcsrc = BitConverter.ToUInt16(b, offset += 2);
            adc1 = BitConverter.ToUInt16(b, offset += 2);
        }

        public string GetStringNotify()
        {
            var ret = new StringBuilder(128);
            ret.Append($"{{ Lat:{(lafull / KoorPrescaler):F6},");
            ret.Append($"Lon:{(lofull / KoorPrescaler):F6},");
            ret.Append($"Date:\"{timePoint.ToString("yyyy-MM-ddTHH:mm:ss")}\",");
            ret.Append($"Spd:{spd / 100},".PadRight(8, ' '));
            ret.Append($"Dist:{accum_dist / 1000.0:F3} }},");
            return ret.ToString();
        }
    };
}
