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

namespace SpyTrekHost
{
    partial class Program
    {
        static void TrekServiceHosting()
        {
            TrekTreeService.TrekTreeService.NoteReader = ScanNoteForNode;

            using (ServiceHost host = new ServiceHost(typeof(TrekTreeService.TrekTreeService)))
            {
                host.Open();
                Console.WriteLine("TrekTreeService started @ " + DateTime.Now);
                Console.ReadKey();
            }
        }

        static NaviNote ScanNoteForNode(string Imei)
        {
            var item = HICollection.List.Find(o => o.Info.Imei == Imei);
            return item?.GeoPoint;
        }
    }
}
