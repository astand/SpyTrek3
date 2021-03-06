﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;
using SpyTrekHost.UserUI;

namespace SpyTrekHost
{
    partial class Program
    {
        public static string AppVersion;

        static TcpClient tcpClient;

        static ListNodesForm listNodes;

        static void Main(string[] args)
        {
            ReadVersion();
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
            Thread host = new Thread(TrekServiceHosting);
            host.Start();

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
            Console.WriteLine($"AppVersion : {AppVersion}");
            Console.WriteLine($"Dispatcher has started @ {DateTime.Now.ToShortTimeString()}");
        }


        static void UIThread()
        {
            Application.Run(listNodes);
        }

        static void ReadVersion()
        {
            var assemblyFullName = Assembly.GetExecutingAssembly().FullName;
            AppVersion = assemblyFullName.Split(',')[1].Split('=')[1];
        }
    }
}
