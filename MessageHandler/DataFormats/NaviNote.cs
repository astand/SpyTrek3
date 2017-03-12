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
        DateTime timePoint;
        Int16 altitude;
        Int32 lafull;
        Int32 lofull;
        UInt16 spd;
        UInt32 accum_dist;
        UInt16 spare;
        UInt16 adcsrc;
        UInt16 adc1;

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

            ret.Append($"{{ lat:{(lafull / KoorPrescaler):F6},");
            ret.Append($"lon:{(lofull / KoorPrescaler):F6},");
            ret.Append($"titl:\"{timePoint.ToString("yyyy-MM-ddTHH:mm:ss")}\",");
            ret.Append($"spd:{spd / 100},".PadRight(8, ' '));
            ret.Append($"dist:{accum_dist / 1000.0:F3} }},");
            return ret.ToString();
        }
    };
}
