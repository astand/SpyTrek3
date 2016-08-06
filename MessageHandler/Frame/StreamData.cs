using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;

namespace MessageHandler
{
    public class StreamData : IStreamData
    {
        private Byte[] _data;

        public StreamData(Int32 size)
        {
            _data = new Byte[size];
        }

        public StreamData(Byte[] data)
        {
            _data = data;
        }

        public Byte[] SerializeToByteArray() => _data;
    }
}
