using MessageHandler;
using MessageHandler.Abstract;
using MessageHandler.ConcreteHandlers;
using MessageHandler.DataFormats;
using MessageHandler.Notifiers;
using MessageHandler.Processors;
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

        public Piper Pipe => piper;

        public ISpyTrekInfoNotifier spyTrekNotifier = null;

        System.Threading.Timer timecallback;

        SpyTrekInfo spyTrekInfo;

        InfoProcessor infoProcessor = new InfoProcessor();
        ReadProcessor readProcessor = new ReadProcessor("Trek");
        TrekDescriptorProcessor noteProcessor = new TrekDescriptorProcessor();
        TrekSaverProcessor saveProc = new TrekSaverProcessor();
        public SpyTrekInfo Info => spyTrekInfo;

        public HandleInstance(NetworkStream stream, EventHandler deleter = null)
        {
            timeConnected = DateTime.Now;
            SelfDeleter += deleter;
            networkPipe = new NetworkPipe(stream);

            piper = new Piper(networkPipe, networkPipe);
            piper.OnFail += Piper_OnFail;
            piper.OnData += Piper_OnData;

            infoProcessor.OnUpdated += WhenInfoUpdated;

            CreateChainOfResponsibility();

            timecallback = new System.Threading.Timer(TimerCallback, null, 500, 15000);
        }


        private void CreateChainOfResponsibility()
        {

            IHandler<FramePacket> infoHand = new ConcreteFileHandler<FramePacket>(null, infoProcessor, piper.SendData);
            IHandler<FramePacket> noteHand = new ConcreteFileHandler<FramePacket>(null, noteProcessor, piper.SendData);
            IHandler<FramePacket> trekHand = new ConcreteFileHandler<FramePacket>(null, saveProc, piper.SendData);

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
        }


        private void Piper_OnFail(Object sender, PiperEventArgs e)
        {
            SelfDeleter(this, null);
        }

        private void TimerCallback(Object obj)
        {
            Debug.WriteLine($"{InstanceName()}:{DateTime.Now.ToString("HH:mm:ss.fff")}. Timer scheduler elapsed");

            Pipe.SendData(new ReadRequest(4));
        }

        private void Piper_OnData(Object sender, PiperEventArgs e)
        {
            var frame = new FramePacket(e.Data);

            Debug.WriteLine($"Opc: {frame.Opc,04:X}. Id: {frame.Id,04:X}. Data length = {frame.Data.Length}");
            /// When the packets comes very fast and HandleRequest cannot process 
            /// data in time the packets are lost, so need process with locking
            lock (handleRead)
            {
                handleRead.HandleRequest(new FramePacket(e.Data));
            }
        }

        private void WhenInfoUpdated(SpyTrekInfo info)
        {
            if (spyTrekInfo == null)
            {
                spyTrekInfo = info;
                saveProc.SetImeiPath(spyTrekInfo.Imei);
                HICollection.RefreshList();
            }
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

        public Int32 ReadTrekCmd(int index_in_list)
        {
            var ret = noteProcessor.GetDescriptor(index_in_list);
            if (ret == null)
                /// trek not found
                return -1;

            var isneed = saveProc.IsTrekNeed(ret);
            if (!isneed)
                /// no needness to downloading
                return -2;
                
            var paydata = BitConverter.GetBytes(ret.Id);

            piper.SendData(new FramePacket(opc: OpCodes.RRQ, id: FiledID.Track, data: paydata, length: 2));

            return ret.Id;
        }

        public void SetListUpdater(Action<List<TrekDescriptor>, bool> updater)
        {
            noteProcessor.OnUpdated += updater;
        }

        public void SetInfoUpdater(Action<SpyTrekInfo> updater)
        {
            infoProcessor.OnUpdated += updater;
        }

        public void SetTrekUpdater(Action<string> updater)
        {
            saveProc.WriteStatus += updater;
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

        private string InstanceName() => $"NODE[{timeConnected.ToString("mmssfff")}]";
    }
}
