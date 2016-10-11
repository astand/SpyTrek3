using MessageHandler;
using MessageHandler.Abstract;
using MessageHandler.ConcreteHandlers;
using MessageHandler.DataFormats;
using MessageHandler.Notifiers;
using StreamHandler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace SpyTrekHost
{
    public class HandleInstance : IDisposable
    {
        private NetworkPipe networkPipe;

        private Piper piper;

        private OperationHandler<FramePacket,ReadOperationer> handleRead;

        private OperationHandler<FramePacket,ErrorOperationer> handleError;

        private OperationHandler<FramePacket,WriteOperationer> handleWrite;

        private readonly DateTime timeConnected;

        public event EventHandler SelfDeleter;

        public DateTime Connected => timeConnected;

        public Piper Pipe { get { return piper; } }

        public ISpyTrekInfoNotifier spyTrekNotifier = null;

        System.Threading.Timer timecallback;

        SpyTrekInfo spyTrekInfo;

        public SpyTrekInfo Info => spyTrekInfo;

        public HandleInstance(NetworkStream stream, EventHandler deleter = null)
        {
            timeConnected = DateTime.Now;
            SelfDeleter += deleter;
            networkPipe = new NetworkPipe(stream);

            piper = new Piper(networkPipe, networkPipe);

            object notif = ReadProcessorFactory.GetInfoProcessor();

            spyTrekNotifier = (ISpyTrekInfoNotifier)notif;

            spyTrekNotifier.Notify += SpyTrekNotifier_Notify;
            piper.OnFail += Piper_OnFail;

            piper.OnData += Piper_OnData;

            IHandler<FramePacket> infoHand = new ConcreteFileHandler<FramePacket>(null, (IFrameProccesor)notif, piper.SendData);
            IHandler<FramePacket> noteHand = new ConcreteFileHandler<FramePacket>(null, ReadProcessorFactory.GetNoteProcessor(), piper.SendData);
            IHandler<FramePacket> trekHand = new ConcreteFileHandler<FramePacket>(null, ReadProcessorFactory.GetTrekProcessor(), piper.SendData);

            infoHand.SetSpecification(fid => fid == FiledID.Info);
            noteHand.SetSpecification(fid => fid == FiledID.Filenotes);
            trekHand.SetSpecification(fid => fid == FiledID.Track);

            infoHand.SetSuccessor(noteHand);
            noteHand.SetSuccessor(trekHand);


            IHandler<FramePacket> errorHand = new ConcreteFileHandler<FramePacket>(null, ReadProcessorFactory.GetErrorProcessor(), null);
            // True specification for ERROR messages
            errorHand.SetSpecification(fid => true);


            IHandler<FramePacket> firmHand = new ConcreteFileHandler<FramePacket>(null, ReadProcessorFactory.GetFirmwareProcessor(piper, @"st8.bin"), null);
            // Set specification for WRQ files
            firmHand.SetSpecification(fid => true);

            handleRead = new OperationHandler<FramePacket, ReadOperationer>(infoHand);
            handleError = new OperationHandler<FramePacket, ErrorOperationer>(errorHand);
            handleWrite = new OperationHandler<FramePacket, WriteOperationer>(firmHand);

            handleRead.SetSuccessor(handleError);
            handleError.SetSuccessor(handleWrite);
            timecallback = new System.Threading.Timer(TimerCallback, null, 0, 15000);
        }

        private void Piper_OnFail(Object sender, PiperEventArgs e)
        {
            SelfDeleter(this, null);
        }

        private void TimerCallback(Object obj)
        {
            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss.fff")}. Timer scheduler elapsed");

            Pipe.SendData(new ReadRequest(4));
        }

        private void Piper_OnData(Object sender, PiperEventArgs e)
        {
            var frame = new FramePacket(e.Data);

            Console.WriteLine($"Opc: {frame.Opc,04:X}. Id: {frame.Id,04:X}. Data length = {frame.Data.Length}");
            /// When the packets comes very fast and HandleRequest cannot process 
            /// data in time the packets are lost, so need process with locking
            lock (handleRead)
            {
                handleRead.HandleRequest(new FramePacket(e.Data));
            }
        }

        private void SpyTrekNotifier_Notify(Object sender, InfoEventArgs e)
        {
            spyTrekInfo = e.spyTrekInfo;
        }

        public override String ToString()
        {
            String retval = base.ToString();

            if (spyTrekInfo != null)
            {
                retval = spyTrekInfo.ToString();
            }

            return retval;
        }

        public void Dispose()
        {
            Debug.WriteLine("Disposing action for HandleInstance");

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

                timecallback?.Dispose();
                timecallback = null;

                piper?.Dispose();
                piper = null;

                networkPipe?.Dispose();
                networkPipe = null;
            }
        }
    }
}
