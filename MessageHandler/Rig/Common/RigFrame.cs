using MessageHandler.Rig.Common;
using StreamHandler.Abstract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.Rig
{
    public class RigFrame : IStreamData
    {
        public OpCode Opc
        {
            get;
            set;
        }

        public OpID RigId
        {
            get;
            set;
        }
        public UInt16 BlockNum
        {
            get;
            set;
        }

        public Byte[] Data
        {
            get
            {
                return data_;
            }
            set
            {
                ExtractDataFromArray(value);
            }
        }

        public int DataSize
        {
            get;
            set;
        } = 0;

        Byte[] data_ = new byte[0];

        public RigFrame(OpCode code = 0, OpID rigid = 0)
        {
            Opc = code;
            RigId = rigid;
        }

        public Byte[] ConvertToBytes()
        {
            var retarray = new Byte[6 + DataSize];
            MakeArray(retarray);
            return retarray;
        }

        public Int32 ConvertToBytes(Byte[] destination)
        {
            if (destination.Length < DataSize + 6)
                return -1;

            MakeArray(destination);
            return DataSize + 6;
        }

        public bool ConvertFromBytes(byte[] arr, int offset = 0)
        {
            if (arr.Length - offset < 6)
                /// Array too small
                return false;

            Opc = (OpCode)BitConverter.ToUInt16(arr, 0);
            RigId = (OpID)BitConverter.ToUInt16(arr, 2);
            BlockNum = BitConverter.ToUInt16(arr, 4);
            ExtractDataFromArray(arr, 6);
            return true;
        }

        void MakeArray(byte[] arr)
        {
            var cell = BitConverter.GetBytes((UInt16)Opc);
            Array.Copy(cell, 0, arr, 0, 2);
            cell = BitConverter.GetBytes((UInt16)RigId);
            Array.Copy(cell, 0, arr, 2, 2);
            cell = BitConverter.GetBytes(BlockNum);
            Array.Copy(cell, 0, arr, 4, 2);
            Array.Copy(Data, 0, arr, 6, DataSize);
        }

        void ExtractDataFromArray(byte[] source, int offset = 0, int len = -1)
        {
            var actual_copy_len = (len < 0) ? (source.Length - offset) : len;

            if (actual_copy_len > data_.Length)
            {
                /// expand array
                data_ = new byte[actual_copy_len];
            }

            Array.Copy(source, offset, data_, 0, actual_copy_len);
            DataSize = actual_copy_len;
        }


        public override String ToString()
        {
            return $"{Opc.ToString().PadRight(6, ' ')}" +
                   $" {RigId.ToString().PadRight(10, ' ')}" +
                   $"{BlockNum.ToString().PadRight(5, ' ')}" + $"{DataSize.ToString().PadRight(5, ' ')}";
        }
    }
}
