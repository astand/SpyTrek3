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

        System.Timers.Timer echoTimer;
        readonly double kEchoTimeout = 15000.0;

        SpyTrekInfo spyTrekInfo;

        InfoProcessor infoProcessor = new InfoProcessor();
        TrekDescriptorProcessor noteProcessor = new TrekDescriptorProcessor();
        TrekSaverProcessor saveProc = new TrekSaverProcessor();

        FirmwareProcessor firmProc;

        ErrorProcessor errProc = new ErrorProcessor();

        Action<String> notifyUI = null;

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

            echoTimer = new System.Timers.Timer(kEchoTimeout);
            echoTimer.Start();
            echoTimer.Elapsed += EchoTime2_Elapsed;
            EchoTime2_Elapsed(echoTimer, null);
        }

        private void EchoTime2_Elapsed(Object sender, ElapsedEventArgs e)
        {
            TimerCallback(null);
        }

        private void CreateChainOfResponsibility()
        {

            IHandler<FramePacket> infoHand = new ConcreteFileHandler<FramePacket>(null, infoProcessor, piper.SendData, ProcessorStateUpdated);
            IHandler<FramePacket> noteHand = new ConcreteFileHandler<FramePacket>(null, noteProcessor, piper.SendData, ProcessorStateUpdated);
            IHandler<FramePacket> trekHand = new ConcreteFileHandler<FramePacket>(null, saveProc, piper.SendData, ProcessorStateUpdated);

            infoHand.SetSpecification(fid => fid == FiledID.Info);
            noteHand.SetSpecification(fid => fid == FiledID.Filenotes);
            trekHand.SetSpecification(fid => fid == FiledID.Track);

            infoHand.SetSuccessor(noteHand);
            noteHand.SetSuccessor(trekHand);


            IHandler<FramePacket> errorHand = new ConcreteFileHandler<FramePacket>(null, errProc, null, ProcessorStateUpdated);
            // True specification for ERROR messages
            errorHand.SetSpecification(fid => true);

            firmProc = new FirmwareProcessor(piper, "st8.bin");
            IHandler<FramePacket> firmHand = new ConcreteFileHandler<FramePacket>(null, firmProc, null, ProcessorStateUpdated);
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
            //Debug.WriteLine($"Opc: {frame.Opc,04:X}. Id: {frame.Id,04:X}. Data length = {frame.Data.Length}");
            /// When the packets comes very fast and HandleRequest cannot process 
            /// data in time the packets are lost, so need process with locking
            lock (handleRead)
            {
                echoTimer.Reset();
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

        private void ProcessorStateUpdated(IFrameProccesor proc)
        {
            Debug.WriteLine($"State: {proc.State.ToString().PadRight(10, ' ')}| {proc.ToString()}");
            notifyUI?.Invoke(proc.ToString());
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

        public Int32 ReadTrekCmd(int trek_id)
        {
            var ret = noteProcessor.GetDescriptor(trek_id);
            if (ret == null)
                /// trek not found
                return -1;

            var isneed = saveProc.IsTrekNeed(ret);
            if (!isneed)
                /// no needness to downloading
                return -2;

            if (ret.IsBadTrek())
                return -3;

            var paydata = BitConverter.GetBytes(ret.Id);

            piper.SendData(new FramePacket(opc: OpCodes.RRQ, id: FiledID.Track, data: paydata, length: 2));

            return ret.Id;
        }

        public void StartFirmwareUpdating()
        {
            firmProc.SendRequest();
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
            /// Set UI status string notifier
            notifyUI += updater;
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

                echoTimer?.Dispose();
                echoTimer = null;

                piper?.Dispose();
                piper = null;

                networkPipe?.Dispose();
                networkPipe = null;
            }
        }

        private string InstanceName() => $"NODE[{timeConnected.ToString("mmssfff")}]";
    }
}
