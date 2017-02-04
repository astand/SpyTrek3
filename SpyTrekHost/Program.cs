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
using System.Configuration;
using System.Diagnostics;

namespace SpyTrekHost
{
    class Program
    {

        static TcpClient tcpClient;

        static ListNodesForm listNodes;

        static void Main(string[] args)
        {
            var appSettings = ConfigurationManager.AppSettings;
            UInt16 portNum = 20201;
            UInt16.TryParse(appSettings["port"], out portNum);
            Console.WriteLine($"Tcp listener has started @ {DateTime.Now.ToShortTimeString()}.Port number is {portNum}");

            var tcpListener = new TcpListener(IPAddress.Any, portNum);
            tcpListener.Start();

            Thread td = new Thread(Dispatcher);

            td.Start();

            listNodes = new ListNodesForm();

            HICollection.AddListUpdater(listNodes.UpdateListNodes);

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

                HICollection.Add(new HandleInstance(tcpClient.GetStream()));
            }
        }


        static void Dispatcher()
        {
            Console.WriteLine($"Dispatcher has started @ {DateTime.Now.ToShortTimeString()}");
        }



        static void UIThread()
        {
            Application.Run(listNodes);
        }
    }
}
