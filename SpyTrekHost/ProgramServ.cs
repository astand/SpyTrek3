using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;
using SpyTrekHost.UserUI;
using TrekTreeService;
using System.ServiceModel;
using MessageHandler.DataFormats;
using System.Collections;
using System.Collections.Generic;

namespace SpyTrekHost
{
    partial class Program
    {
        static void TrekServiceHosting()
        {
            TrekTreeService.TrekTreeService.NodePointsReader = ScanNoteForNode;

            using (ServiceHost host = new ServiceHost(typeof(TrekTreeService.TrekTreeService)))
            {
                host.Open();
                Console.WriteLine("TrekTreeService started @ " + DateTime.Now);
                Console.ReadKey();
            }
        }

        static List<NaviNote> ScanNoteForNode(string Imei)
        {
            var item = HICollection.List.Find(o => o.Info?.Imei == Imei);
            return item?.ReadCachedPoints();
        }
    }
}
