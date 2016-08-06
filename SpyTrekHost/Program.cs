﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using StreamHandler;
using MessageHandler.Abstract;
using MessageHandler.ConcreteHandlers;
using MessageHandler;

namespace SpyTrekHost
{
    class Program
    {
        static Piper channelPipe;

        static TcpClient tcpClient;

        static OperationHandler<FramePacket,ReadOperationer> handleRead;

        static OperationHandler<FramePacket,ErrorOperationer> handleError;


        static void Main(string[] args)
        {
            Console.WriteLine($"Spy Trek Host started @ {DateTime.Now}");
            while (true)
            {

                Console.Write($"Please input listening port number : ");


                var PortNumber = Console.ReadLine();
                Console.WriteLine("");
                UInt16 PortNumberNum = 0;
                try
                {
                    PortNumberNum = Convert.ToUInt16(PortNumber);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Something wrong  with port number : {e.Message}");
                    continue;
                }

                var tcpListener = new TcpListener(IPAddress.Any, PortNumberNum);
                tcpListener.Start();

                try
                {
                    tcpClient = tcpListener.AcceptTcpClient();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Cannot accept client : {e.Message}");
                    continue;
                }
                Console.WriteLine($"Client connected! Info : {tcpListener.Server}");


                var stream_for_pipe = new NetworkPipe(tcpClient.GetStream());

                channelPipe = new Piper(stream_for_pipe, stream_for_pipe);

                channelPipe.OnData += ChannelPipe_OnData;

                IHandler<FramePacket> infoHand = new ConcreteFileHandler<FramePacket>(null, ReadProcessorFactory.GetInfoProcessor(), channelPipe.SendData);
                IHandler<FramePacket> noteHand = new ConcreteFileHandler<FramePacket>(null, ReadProcessorFactory.GetNoteProcessor(), channelPipe.SendData);
                IHandler<FramePacket> trekHand = new ConcreteFileHandler<FramePacket>(null, ReadProcessorFactory.GetTrekProcessor(), channelPipe.SendData);

                infoHand.SetSuccessor(noteHand);
                noteHand.SetSuccessor(trekHand);

                infoHand.SetSpecification(fid => fid == FiledID.Info);
                noteHand.SetSpecification(fid => fid == FiledID.Filenotes);
                trekHand.SetSpecification(fid => fid == FiledID.Track);

                handleRead = new OperationHandler<FramePacket, ReadOperationer>(infoHand);

                IHandler<FramePacket> errorHand = new ConcreteFileHandler<FramePacket>(null, ReadProcessorFactory.GetErrorProcessor(), null);
                errorHand.SetSpecification(fid => true);

                handleError = new OperationHandler<FramePacket, ErrorOperationer>(errorHand);

                handleRead.SetSuccessor(handleError);

                while (true)
                {
                    Console.WriteLine($"Input command number (1 - Echo, 2 - Info)...");
                    var command = Console.ReadLine();
                    UInt16 CommandNum = Convert.ToUInt16(command);
                    channelPipe.SendData(new ReadRequest(CommandNum));
                }

            }
        }

        private static void ChannelPipe_OnData(Object sender, PiperEventArgs e)
        {
            var frame = new FramePacket(e.Data);

            /// When the packets comes very fast and HandleRequest cannot process 
            /// data in time the packets are lost, so need process with locking
            lock (handleRead)
            {
                handleRead.HandleRequest(new FramePacket(e.Data));
            }
        }
    }
}
