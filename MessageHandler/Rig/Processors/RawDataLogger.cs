using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.Rig.Processors
{
    public static class RawDataLogger
    {

        public static int count;

        public static string fName = DateTime.Now.ToString("HH-mm-ss") + ".txt";
        public static void SaveArray(byte[] arr, int size)
        {
            using (StreamWriter sw = File.AppendText(fName))
            {
                for (int i = 0; i < size; i++)
                {
                    sw.Write(PrintByte(arr[i]));
                }
            }
        }

        private static string PrintByte(Byte item)
        {
            var ret = $"0x{item:X2}, ";

            if (count++ > 15)
            {
                count = 0;
                ret += Environment.NewLine;
            }

            return ret;
        }
    }
}
