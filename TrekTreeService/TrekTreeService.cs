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
        public static Func<string, List<NaviNote>> NodePointsReader;

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

            var nodePoint = NodePointsReader(request.Imei)[0];
            var ret = new TrekNodePoint()
            {
                Date = "Invalid"
            };

            if (nodePoint != null)
            {
                ret.Date = nodePoint.timePoint.ToString("yyyy-MM-ddTHH:mm:ss");
                ret.Lon = nodePoint.lofull / 1000000.0;
                ret.Lat = nodePoint.lafull / 1000000.0;
                ret.Dist = nodePoint.accum_dist / 1000.0;
                ret.Kurs = 0;
                ret.Spd = nodePoint.spd / 100.0;
            }

            return ret;
        }

        public TrekNodePoints GetTrekTreeNodePoints(TrekTreeRequest request)
        {
            var response = new TrekNodePoints() { Status = "Connected" };

            if (request.Imei == null)
            {
                response.Status = "BadRequest";
                return response;
            }

            var nodePoints = NodePointsReader(request.Imei);

            if (nodePoints == null)
            {
                response.Status = "NotConnected";
            }
            else
            {
                foreach (var onepoint in nodePoints)
                {
                    var ret = new TrekNodePoint
                    {
                        Date = onepoint.timePoint.ToString("yyyy-MM-ddTHH:mm:ss"),
                        Lon = onepoint.lofull / 1000000.0,
                        Lat = onepoint.lafull / 1000000.0,
                        Dist = onepoint.accum_dist / 1000.0,
                        Kurs = 0,
                        Spd = onepoint.spd / 100.0
                    };
                    response.PointsList.Add(ret);
                }
            }

            return response;
        }
    }
}
