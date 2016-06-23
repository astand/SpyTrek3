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

        public Byte[] GetCheckedArray(Byte[] arr)
        {
            var retarray = new Byte[arr.Length + 2];

            Array.Copy(arr, retarray, arr.Length);

            InitStartCrc();
            CalculateForArray(arr, arr.Length);

            retarray[retarray.Length - 2] = Hi;
            retarray[retarray.Length - 1] = Low;

            return retarray;
        }

        public Boolean GetUncheckedArray(Byte[] crcarray, out Byte[] outbuff)
        {
            outbuff = new Byte[0];

            if (PacketCannotBeUnchecked(crcarray))
                return false;

            outbuff = new Byte[crcarray.Length - 2];

            if (PacketCrcWrong(crcarray))
                return false;
            
            Array.Copy(crcarray, outbuff, outbuff.Length);
            return true;
        }

        private Boolean PacketCrcWrong(Byte[] arr)
        {
            InitStartCrc();
            CalculateForArray(arr, arr.Length - 2);
            return (arr[arr.Length - 2] != Hi || arr[arr.Length - 1] != Low);
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
