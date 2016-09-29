﻿using MessageHandler;
using MessageHandler.Abstract;
using MessageHandler.ConcreteHandlers;
using StreamHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace SpyTrekHost
{
    public class HandleInstance
    {
        private NetworkPipe networkPipe;

        private Piper piper;

        private OperationHandler<FramePacket,ReadOperationer> handleRead;

        private OperationHandler<FramePacket,ErrorOperationer> handleError;

        private OperationHandler<FramePacket,WriteOperationer> handleWrite;

        public Piper Pipe { get { return piper; } }

        System.Threading.Timer timecallback;
        public HandleInstance(NetworkStream stream)
        {
            networkPipe = new NetworkPipe(stream);

            piper = new Piper(networkPipe, networkPipe);

            piper.OnData += Piper_OnData;

            IHandler<FramePacket> infoHand = new ConcreteFileHandler<FramePacket>(null, ReadProcessorFactory.GetInfoProcessor(), piper.SendData);
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
            timecallback = new System.Threading.Timer(TimerCallback, null, 0, 5000);
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
    }
}