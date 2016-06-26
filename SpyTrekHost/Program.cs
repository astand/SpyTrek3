using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using StreamHandler;


namespace SpyTrekHost
{
    class Program
    {
        static TcpClient tcpClient;
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

                var channelPipe = new Piper(stream_for_pipe, stream_for_pipe);

                channelPipe.OnData += ChannelPipe_OnData;


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
            Console.WriteLine($"Data presence in Pipe. {e.Message}. Data Length = {e.Data.Length}");
        }
    }
}
