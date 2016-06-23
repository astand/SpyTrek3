using StreamHandler.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace StreamHandler
{
    public class Piper
    {
        private Stream stream;

        private Timer resend_time = new Timer();

        private IStreamHandler packer = new SimpleHandler();

        private bool IsBusy = false;

        private Int32 m_try_count;

        public event DataEventHandler OnData;

        public event ErrorEventHandler OnFail;

        public Piper(Stream stream)
        {
            /* need check for null ??? */
            if (stream == null)
                throw new NullReferenceException($"Cannot assign null stream");

            this.stream = stream;
        }


        public Int32 Send(IStreamData data)
        {
            if (IsBusy)
                return -1;
            IsBusy = true;
            SendData(data);
            return 0;
        }


        public Int32 SendData(IStreamData streamdata)
        {
            if (!stream.CanWrite)
            {
                OnFail(null, null);
                return -1;
            }

            var array_to_send = packer.PackPacket(streamdata.SerializeToByteArray());
            stream.Write(array_to_send, 0, array_to_send.Length);
            return 0;
        }



        public void OnDataCaller()
        {
            OnData(null, null);
        }

    }
}
