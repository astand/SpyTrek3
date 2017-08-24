using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TrekTreeService.MessageContracts
{
    [MessageContract]
    public class TrekNodePoints
    {
        [MessageBodyMember]
        public List<TrekNodePoint> PointsList = new List<TrekNodePoint>();
    }
}
