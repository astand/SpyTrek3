using System;
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

        static OperationHandler<FramePacket,WriteOperationer> handleWrite;

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
                // RRQ handlers
                IHandler<FramePacket> infoHand = new ConcreteFileHandler<FramePacket>(null, ReadProcessorFactory.GetInfoProcessor(), channelPipe.SendData);
                IHandler<FramePacket> noteHand = new ConcreteFileHandler<FramePacket>(null, ReadProcessorFactory.GetNoteProcessor(), channelPipe.SendData);
                IHandler<FramePacket> trekHand = new ConcreteFileHandler<FramePacket>(null, ReadProcessorFactory.GetTrekProcessor(), channelPipe.SendData);
                // ERROR handlers 
                IHandler<FramePacket> errorHand = new ConcreteFileHandler<FramePacket>(null, ReadProcessorFactory.GetErrorProcessor(), null);

                //var firmProcessor = new FirmwareProcessor(channelPipe, "");
                var firmProcessor = new FirmwareProcessor(channelPipe, @"d:\zShare\k_p_4.28.bin");
                // WRQ handlers
                IHandler<FramePacket> firmHand = new ConcreteFileHandler<FramePacket>(null, firmProcessor, null);

                // Set specification for RRQ files
                infoHand.SetSpecification(fid => fid == FiledID.Info);
                noteHand.SetSpecification(fid => fid == FiledID.Filenotes);
                trekHand.SetSpecification(fid => fid == FiledID.Track);

                // True specification for ERROR messages
                errorHand.SetSpecification(fid => true);

                // Set specification for WRQ files
                firmHand.SetSpecification(fid => true);

                infoHand.SetSuccessor(noteHand);
                noteHand.SetSuccessor(trekHand);

                handleRead = new OperationHandler<FramePacket, ReadOperationer>(infoHand);
                handleError = new OperationHandler<FramePacket, ErrorOperationer>(errorHand);
                handleWrite = new OperationHandler<FramePacket, WriteOperationer>(firmHand);

                handleRead.SetSuccessor(handleError);
                handleError.SetSuccessor(handleWrite);

                while (true)
                {
                    Console.WriteLine($"Input command number (1 - Echo, 2 - Info)...");
                    var command = Console.ReadLine();
                    Int32 commandNum = 0;

                    var isDigit = Int32.TryParse(command, out commandNum);

                    if (isDigit)
                        channelPipe.SendData(new ReadRequest((UInt16)commandNum));

                    if (command[0] == 'w')
                        firmProcessor.SendRequest();


                }

            }
        }

        private static void ChannelPipe_OnData(Object sender, PiperEventArgs e)
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
