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
using System.Threading;
using SpyTrekHost.UserUI;
using System.Windows.Forms;
using System.Diagnostics;

namespace SpyTrekHost
{
    class Program
    {

        static List<HandleInstance> nodes = new List<HandleInstance>();

        static TcpClient tcpClient;

        static ListNodesForm listNodes;

        static void Main(string[] args)
        {

            Console.Write($"Please input listening port number : ");
            var PortNumber = Console.ReadLine();
            Console.WriteLine("");
            UInt16 PortNumberNum;
            if (UInt16.TryParse(PortNumber, out PortNumberNum) == false)
                PortNumberNum = 20201;

            Console.WriteLine($"Tcp listener has started @ {DateTime.Now.ToShortTimeString()}.Port number is {PortNumberNum}");

            var tcpListener = new TcpListener(IPAddress.Any, PortNumberNum);
            tcpListener.Start();

            Thread td = new Thread(Dispatcher);

            td.Start();

            listNodes = new ListNodesForm();
            listNodes.SetListGetter(RefreshInstances);

            Thread ui = new Thread(UIThread);
            ui.Start();

            while (true)
            {
                try
                {
                    tcpClient = tcpListener.AcceptTcpClient();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Cannot accept client : {e.Message}");
                    continue;
                }

                nodes.Add(new HandleInstance(tcpClient.GetStream(), Deleter));
                string info = String.Format($"{DateTime.Now.ToString("HH:mm:ss.ff")}> Client connected! Current counts = {nodes.Count}");
                listNodes.EnforceUpdating();
                
                Console.WriteLine(info);
            }
        }


        static void Dispatcher()
        {
            Console.WriteLine($"Dispatcher has started @ {DateTime.Now.ToShortTimeString()}");


            while (true)
            {
                var input = Console.ReadLine();

                if (nodes.Count != 0)
                {
                    var last_node_index = nodes.Count - 1;
                    var pipe = nodes[last_node_index].Pipe;

                    UInt16 commandNum;
                    if (UInt16.TryParse(input, out commandNum) == false)
                        commandNum = 4;

                    pipe.SendData(new ReadRequest(commandNum));
                }

                Thread.Sleep(200);
            }
        }



        static void UIThread()
        {
            Application.Run(listNodes);
        }

        static void Deleter(object sender, EventArgs args)
        {
            HandleInstance vic = sender as HandleInstance;
            Int32 index = nodes.IndexOf(vic);

            if (index == -1)
            {
                Debug.WriteLine($"Node was deleted previously.");
                return;
            }

            Debug.WriteLine($"Node with index [{index}] will be deleted. Stream was broken.");
            nodes.Remove(vic);
            
            vic.Dispose();
            listNodes.EnforceUpdating();
        }

        static List<HandleInstance> RefreshInstances() => nodes;
    }
}
