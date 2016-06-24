using StreamHandler.Abstract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;

namespace StreamHandler
{
    public class Piper : IDisposable
    {
        private MemoryStream stream;

        private System.Timers.Timer readtime;

        private IStreamHandler packer = new SimpleHandler();

        public event EventHandler<PiperEventArgs> OnData;

        public event EventHandler<PiperEventArgs> OnFail;
        
        public Piper(MemoryStream stream)
        {
            /* need check for null ??? */
            if (stream == null)
                throw new NullReferenceException($"Cannot assign null stream");

            this.stream = stream;
            TimerForPollingStreamInit(5);

        }

        public Int32 SendData(IStreamData streamdata)
        {
            if (!stream.CanWrite)
            {
                OnFailCaller("Data cannot be write");
                return -1;
            }

            var array_to_send = packer.PackPacket(streamdata.SerializeToByteArray());
            stream.Write(array_to_send, 0, array_to_send.Length);
            return 0;
        }

        public Byte[] ReadUnpackedData() => packer.GetUnpacked();

        public void Dispose()
        {
            readtime.Dispose();
        }

        private void TimerForPollingStreamInit(double ms)
        {
            readtime = new System.Timers.Timer(ms);
            readtime.Elapsed += Readtime_Elapsed;
            readtime.AutoReset = true;
            readtime.Start();
        }

        private void Readtime_Elapsed(Object sender, ElapsedEventArgs e)
        {
            var output_buff = stream.ToArray();
            
            if(output_buff.Length != 0)
            {
                foreach (var item in output_buff)
                {
                    var unpack_res = packer.UnpackPacket(item);
                    if (unpack_res)
                    {
                        OnDataCaller(packer.GetUnpacked(), "Packer received one packet");
                        return;
                    }
                }
            }
        }

        private void OnDataCaller(byte[] data, string message)
        {
            PiperEventArgs args = new PiperEventArgs(data, message);
            Volatile.Read(ref OnData)?.Invoke(this, args);
        }

        private void OnFailCaller(string message)
        {
            PiperEventArgs args = new PiperEventArgs(null, message);
            Volatile.Read(ref OnFail)?.Invoke(this, args);
        }

        public void TestOnDataInvoker()
        {
            OnDataCaller(new Byte[5], "Test invokation");
        }

    }
}
