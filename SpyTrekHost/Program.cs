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

namespace SpyTrekHost
{
    class Program
    {

        static List<HandleInstance> m_nodes = new List<HandleInstance>();

        static TcpClient tcpClient;

        static ListNodes listNodes;

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

            listNodes = new ListNodes();
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

                m_nodes.Add(new HandleInstance(tcpClient.GetStream()));
                string info = String.Format($"Client connected! Info : {tcpListener.Server}. Count in pool = {m_nodes.Count}");
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

                if (m_nodes.Count != 0)
                {
                    var last_node_index = m_nodes.Count - 1;
                    var pipe = m_nodes[last_node_index].Pipe;

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

        static List<HandleInstance> RefreshInstances() => m_nodes;
    }
}
