using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamHandler
{
    public class Crc16
    {
        private const Byte INIT_CRC_VALUE = 0xFF;

        public Byte Hi { get; private set; }

        public Byte Low { get; private set; }

        public Byte[] GetCheckedArray(Byte[] clean_array)
        {
            var crc_array = new Byte[clean_array.Length + 2];

            Array.Copy(clean_array, crc_array, clean_array.Length);

            InitStartCrc();
            CalculateForArray(clean_array, clean_array.Length);

            crc_array[crc_array.Length - 2] = Hi;
            crc_array[crc_array.Length - 1] = Low;

            return crc_array;
        }

        /// <summary>
        /// Check crc16 validity of array for @length
        /// </summary>
        /// <param name="array"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public Boolean CheckValidCRCInArray(Byte[] array, Int32 length)
        {
            if (PacketCannotBeUnchecked(array) || length < 2)
                return false;

            if (PacketCrcWrong(array, length))
                return false;

            return true;

        }

        private Boolean PacketCrcWrong(Byte[] arr, int length)
        {
            InitStartCrc();
            CalculateForArray(arr, length - 2);
            return (arr[length - 2] != Hi || arr[length - 1] != Low);
        }

        private Boolean PacketCannotBeUnchecked(Byte[] buff) => (buff.Length < 2);

        private void InitStartCrc()
        {
            Hi = INIT_CRC_VALUE;
            Low = INIT_CRC_VALUE;
        }

        private void CalculateForArray(Byte[] buff, int neededlength)
        {
            for (int i = 0; i < neededlength; i++)
                Update(buff[i]);
        }

        private void Update(Byte inbyte)
        {
            inbyte ^= (Hi);
            Hi = (Byte)(Low ^ Crc16Data.HiTableArray[inbyte]);
            Low = Crc16Data.LowTableArray[inbyte];
        }

    }
}
