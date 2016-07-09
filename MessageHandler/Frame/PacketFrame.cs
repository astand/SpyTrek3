using StreamHandler.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler
{
    public class FrameHead : IStreamData
    {
        protected const Int32 HeadSize = 4;

        public UInt16 Opc { get; protected set; }

        public UInt16 Id { get; protected set; }

        public virtual Byte[] SerializeToByteArray()
        {
            var retarray = new Byte[HeadSize];

            var opc = BitConverter.GetBytes(Opc);
            var id = BitConverter.GetBytes(Id);

            Array.Copy(opc, 0, retarray, 0, 2);
            Array.Copy(id, 0, retarray, 2, 2);

            return retarray;
        }

        public FrameHead(UInt16 opc, UInt16 id)
        {
            Opc = opc;
            Id = id;
        }

        public FrameHead(Byte[] array)
        {
            if (array == null || array.Length < HeadSize)
                throw new ArgumentException("Head size must have at least 4 bytes length");

            Opc = BitConverter.ToUInt16(array, 0);

            Id = BitConverter.ToUInt16(array, 2);
        }
    }


    public class FramePacket : FrameHead
    {
        public Byte[] Data { get; }

        public FramePacket(Byte[] array) : base(array)
        {
            /* Copy data body */
            Data = new Byte[array.Length - HeadSize];
            Array.Copy(array, HeadSize, Data, 0, array.Length - HeadSize);
        }


        public FramePacket(UInt16 opc, UInt16 id, Byte[] data) : base(opc, id)
        {
            if (data == null)
            {
                Data = new byte[0];
                return;
            }

            Data = new Byte[data.Length];
            Array.Copy(data, Data, data.Length);
        }

        public override sealed Byte[] SerializeToByteArray()
        {
            var head_array = base.SerializeToByteArray();
            var ret_array = new Byte[Data.Length + HeadSize];

            Array.Copy(head_array, 0, ret_array, 0, HeadSize);
            Array.Copy(Data, 0, ret_array, HeadSize, Data.Length);

            return ret_array;
        }
    }
}
