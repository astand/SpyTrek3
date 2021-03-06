﻿using MessageHandler;
using MessageHandler.DataFormats;
using MessageHandler.Notifiers;
using MessageHandler.Rig;
using MessageHandler.Rig.Common;
using MessageHandler.Rig.Processors;
using StreamHandler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Timers;

namespace SpyTrekHost
{
    public partial class HandleInstance : IDisposable
    {
        private NetworkPipe networkPipe;

        private Piper piper;

        private readonly DateTime timeConnected;

        public event EventHandler SelfDeleter;

        public DateTime Connected => timeConnected;

        public Piper Pipe => piper;

        public ISpyTrekInfoNotifier spyTrekNotifier = null;
        public SpyTrekInfo Info => spyTrekInfo;


        Timer echoTimer;
        Timer roundTimer;

        readonly double kEchoTimeout = 10 * 1000;

        SpyTrekInfo spyTrekInfo;

        // New Rig prototol instances
        InfoHandler infoHandler = new InfoHandler();
        SoleTrekHandler saveHandler = new SoleTrekHandler();
        TrekListHandler listHandler = new TrekListHandler();
        EchoHandler echoHandler = new EchoHandler();
        FirmHandler firmHandler = new FirmHandler();
        RigRouter rigRouter;
        // end of rig

        Action<String> notifyUI = null;

        RigFrame rigFrame = new RigFrame();

        public HandleInstance(NetworkStream stream, EventHandler deleter = null)
        {
            timeConnected = DateTime.Now;
            SelfDeleter += deleter;
            networkPipe = new NetworkPipe(stream);
            piper = new Piper(networkPipe, networkPipe);
            piper.OnFail += Piper_OnFail;
            piper.OnData += Piper_OnData;
            infoHandler.OnUpdated += WhenInfoUpdated;
            CreateChainOfResponsibility();
            InitEchoTimer();
            InitRoundTimer();
        }


        private void EchoTime2_Elapsed(Object sender, ElapsedEventArgs e)
        {
            Debug.WriteLine($"{InstanceName()}:{DateTime.Now.ToString("HH: mm: ss.fff")}. Timer scheduler elapsed");

            if (spyTrekInfo == null)
                Pipe.SendData(new RigRrqFrame(OpID.Info));
            else
                Pipe.SendData(new RigRrqFrame(OpID.Echo));
        }

        private void CreateChainOfResponsibility()
        {
            var tempList = new List<RigHandler>();
            tempList.Add(new RigHandler(infoHandler, piper.SendData));
            tempList.Add(new RigHandler(listHandler, piper.SendData));
            tempList.Add(new RigHandler(saveHandler, piper.SendData));
            tempList.Add(new RigHandler(firmHandler, piper.SendData));
            tempList.Add(new RigHandler(echoHandler, piper.SendData));
            rigRouter = new RigRouter(tempList);
            rigRouter.ProcUpdateListener += ProcFullStateNotify;
        }
        private void Piper_OnFail(Object sender, PiperEventArgs e)
        {
            SelfDeleter(this, null);
        }

        private void Piper_OnData(Object sender, PiperEventArgs e)
        {
            lock (rigRouter)
            {
                rigFrame.ConvertFromBytes(e.Data);
                rigRouter.HandleFrame(rigFrame);
            }
        }
        private void WhenInfoUpdated(SpyTrekInfo info)
        {
            if (spyTrekInfo == null)
            {
                spyTrekInfo = info;
                saveHandler.ImeiPath = (spyTrekInfo.Imei);
                HICollection.RefreshList();
            }
        }
        private void ProcessorStateUpdated(IFrameProccesor<RigFrame> proc)
        {
            Debug.WriteLine($"State: {proc.PState.ToString().PadRight(10, ' ')}| {proc.ToString()}");
            notifyUI?.Invoke(proc.ToString());
        }

        private void ProcFullStateNotify(ProcFullState proc)
        {
            notifyUI?.Invoke(proc.Message);
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
            var ret = listHandler.Trek(trek_id);

            if (ret == null)
                /// trek not found
                return -1;

            var isneed = saveHandler.IsTrekNeed(ret);

            if (!isneed)
                /// no needness to downloading
                return -2;

            if (ret.IsBadTrek())
                return -3;

            saveHandler.SendReadRequest(new RigRrqTrekFrame(ret.Id));
            return ret.Id;
        }
        public void StartFirmwareUpdating()
        {
            //firmProc.SendRequest();
            firmHandler.StartWriteAction();
        }
        public void SetListUpdater(Action<List<TrekDescriptor>, bool> updater)
        {
            //noteProcessor.OnUpdated += updater;
            listHandler.OnUpdated += updater;
        }
        public void SetInfoUpdater(Action<SpyTrekInfo> updater)
        {
            //infoProcessor.OnUpdated += updater;
            infoHandler.OnUpdated += updater;
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
                roundTimer?.Dispose();
                roundTimer = null;
                echoTimer?.Dispose();
                echoTimer = null;
                piper?.Dispose();
                piper = null;
                networkPipe?.Dispose();
                networkPipe = null;
            }
        }
        private string InstanceName() => $"NODE[{timeConnected.ToString("mmssfff")}]";

        void InitRoundTimer()
        {
            roundTimer = new Timer();
            roundTimer.Interval = 5000;
            roundTimer.Elapsed += RoundTimer_Elapsed;
            roundTimer.Start();
        }

        private void InitEchoTimer()
        {
            echoTimer = new Timer(kEchoTimeout);
            echoTimer.Start();
            echoTimer.Elapsed += EchoTime2_Elapsed;
            EchoTime2_Elapsed(echoTimer, null);
        }
    }
}
