using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.DataFormats
{
    public class NaviNote
    {
        const Int32 kCoordinatePrescaler = 1000000;
        DateTime timePoint;
        Int16 altitude;
        Int32 lafull;
        Int32 lofull;
        UInt16 spd;
        UInt32 accum_dist;
        UInt16 spare;
        UInt16 adcsrc;
        UInt16 adc1;

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

        public static string PrintCoor(int crd)
        {
            int man = crd / kCoordinatePrescaler;
            int div = crd % kCoordinatePrescaler;
            return string.Format("{0}.{1:D6}", man, div);
        }



        public string PrintDistance()
        {
            string ret = string.Format("{0}.{1:D3}", accum_dist / 10000, (accum_dist % 10000) / 10);
            return ret;
        }

        public string GetStringNotify()
        {
            string outvic;

            string stdate = timePoint.ToString("yyyy-MM-ddTHH:mm:ss");
            outvic = "{ lat:" + PrintCoor(lafull) + ",lon:" + PrintCoor(lofull) + ",titl:";
            outvic += stdate + ",spd:" + spd / 100 + ",dist:" + PrintDistance() + " },";

            return outvic;
        }

        public override String ToString() => GetStringNotify();

    };
}
