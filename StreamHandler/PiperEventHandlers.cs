using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamHandler
{
    public class PiperEventArgs : EventArgs
    {
        public PiperEventArgs(Byte[] data, String message = "")
        {
            Message = message;
            Data = data;
        }

        public string Message { get; set; }

        public Byte[] Data { get; set; }

    }

    //public delegate void DataEventHandler(Object sender, EventArgs e);

    //public delegate void ErrorventHandler(Object sender, EventArgs e);
}
