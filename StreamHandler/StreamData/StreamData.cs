using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;

namespace StreamHandler
{
    public class StreamData : IStreamData
    {
        private Byte[] _data;

        public StreamData(Int32 size)
        {
            _data = new Byte[size];
        }

        public Byte[] SerializeToByteArray() => _data;
    }
}
