using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TrekTreeService.MessageContracts;
using TrekTreeService.Abstract;
using TrekTreeService.Concrete;
using MessageHandler.DataFormats;

namespace TrekTreeService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TrekTreeService" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class TrekTreeService : ITrekTreeService
    {
        public static Func<string, NaviNote> NoteReader;

        ITrekInfoProvider trekinfo = new TrekFileProvider();

        public TrekFile GetTrekFile(TrekTreeRequest request)
        {
            var ret = new TrekFile();
            ret.Name = "new file";
            ret.Content = trekinfo.GetContent(request.Imei, request.Year, request.Month, request.Day, request.FName);
            return ret;
        }

        public TrekTreeCollection GetTrekTreeCollection(TrekTreeRequest request)
        {
            Console.WriteLine("GetTrek request");
            var infoarray = trekinfo.GetInfo(request.Imei, request.Year, request.Month, request.Day);
            var model = new TrekTreeCollection();

            foreach (var info in infoarray)
            {
                var item = TrekDetails.ConvertToMessage(info);
                item.Route = RouteTree.GetRouteTree(request, info.NodeId);
                model.collection.Add(item);
            }

            return model;
        }
        public TrekNodePoint GetTrekNodePoint(TrekTreeRequest request)
        {
            if (request.Imei == null)
                return null;

            var nav_note = NoteReader(request.Imei);
            var ret = new TrekNodePoint()
            {
                Date = "Invalid"
            };

            if (nav_note != null)
            {
                ret.Date = nav_note.timePoint.ToString("yyyy-MM-ddTHH:mm:ss");
                ret.Lon = nav_note.lofull / 1000000.0;
                ret.Lat = nav_note.lafull / 1000000.0;
                ret.Dist = nav_note.accum_dist / 1000.0;
                ret.Kurs = 0;
                ret.Spd = nav_note.spd / 100.0;
            }

            return ret;
        }

    }
}
