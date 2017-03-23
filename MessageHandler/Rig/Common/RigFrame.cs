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
            get;
            set;
        }

        public RigFrame() { }

        public RigFrame(byte[] arr)
        {
            Opc = (OpCode)BitConverter.ToUInt16(arr, 0);
            RigId = (OpID)BitConverter.ToUInt16(arr, 2);
            BlockNum = BitConverter.ToUInt16(arr, 4);
            Data = new byte[arr.Length - 6];
            Array.Copy(arr, 6, Data, 0, Data.Length);
        }


        public Byte[] SerializeToByteArray()
        {
            var retarray = new Byte[6 + Data.Length];
            MakeArray(retarray);
            return retarray;
        }

        public Boolean SerializeToByteArray(Byte[] destination)
        {
            try
            {
                MakeArray(destination);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"RigFrame creation error : ${ex.Message}");
                return false;
            }

            return true;
        }

        public bool DeserializeFromArray(byte[] srcArray)
        {
            Opc = (OpCode)BitConverter.ToUInt16(srcArray, 0);
            RigId = (OpID)BitConverter.ToUInt16(srcArray, 2);
            BlockNum = BitConverter.ToUInt16(srcArray, 4);
            Data = new byte[srcArray.Length - 6];
            Array.Copy(Data, 0, srcArray, 6, Data.Length);
            return true;
        }

        private void MakeArray(byte[] arr)
        {
            var cell = BitConverter.GetBytes((UInt16)Opc);
            Array.Copy(cell, 0, arr, 0, 2);
            cell = BitConverter.GetBytes((UInt16)RigId);
            Array.Copy(cell, 0, arr, 2, 2);
            cell = BitConverter.GetBytes(BlockNum);
            Array.Copy(cell, 0, arr, 4, 2);
            Array.Copy(Data, 0, arr, 6, Data.Length);
        }

        public override String ToString()
        {
            return $"{Opc.ToString().PadRight(6, ' ')}" +
                   $" {RigId.ToString().PadRight(10, ' ')}" +
                   $"{BlockNum.ToString().PadRight(5, ' ')}" + $"{Data.Length.ToString().PadRight(5, ' ')}";
        }
    }
}
