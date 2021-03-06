﻿using StreamHandler.Abstract;
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

        public event EventHandler<PiperEventArgs> OnData;

        public event EventHandler<PiperEventArgs> OnFail;

        public ByteRate UploadRate {
            get;
            set;
        } = new ByteRate();

        public ByteRate DownloadRate {
            get;
            set;
        } = new ByteRate();

        IPipeReader reader;

        IPipeWriter writer;

        System.Timers.Timer readtime;

        IStreamHandler packman = new SimpleHandler();

        public Piper(/*Stream stream, */IPipeReader reader, IPipeWriter writer)
        {
            /* need check for null ??? */
            if (reader == null || writer == null)
                throw new NullReferenceException($"Cannot assign null stream");

            this.reader = reader;
            this.writer = writer;
            TimerForPollingStreamInit(10);
        }

        public Int32 SendData(IStreamData streamdata)
        {
            var array_to_send = packman.PackPacket(streamdata.ConvertToBytes());
            UploadRate.PassData(array_to_send.Length);

            try
            {
                writer.Write(array_to_send, array_to_send.Length);
            }
            catch (NullReferenceException ex)
            {
            }
            catch (IOException ex)
            {
                OnFailCaller($"Write pipe is crashed. {ex.Message}");
            }

            return 0;
        }

        public Byte[] ReadUnpackedData() => packman.Data();


        private void TimerForPollingStreamInit(double ms)
        {
            readtime = new System.Timers.Timer(ms);
            readtime.Elapsed += Readtime_Elapsed;
            readtime.AutoReset = true;
            readtime.Start();
        }

        private void Readtime_Elapsed(Object sender, ElapsedEventArgs e)
        {
            if (packman.ExtractPacket(reader))
            {
                DownloadRate.PassData(packman.Data().Length);
                OnDataCaller(packman.Data(), "Packer received one packet");
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
        public void Dispose()
        {
            readtime?.Dispose();
            readtime = null;
            reader = null;
            writer = null;
            packman = null;
            OnData = null;
            OnFail = null;
        }

    }
}
