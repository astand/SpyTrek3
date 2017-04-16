using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace TrekTreeService.MessageContracts
{   
    [KnownType(typeof(RouteTree))]
    [MessageContract(IsWrapped = true, WrapperName = "TrekTreeInstanseObject", WrapperNamespace = "http://localhost/TrekTree")]
    public class TrekTreeInstance
    {
        [MessageBodyMember(Order = 1)]
        public string NameLink { get; set; }

        [MessageBodyMember(Order = 2)]
        public int Count { get; set; }

        [MessageBodyMember(Order = 3)]
        public TimeSpan Duration { get; set; }

        [MessageBodyMember(Order = 4)]
        public double AverageSpeed { get; set; }

        [MessageBodyMember(Order = 5)]
        public double LocalMile { get; set; }

        [MessageBodyMember(Order = 6)]
        public double FullMile { get; set; }

        [MessageBodyMember(Order = 7)]
        public RouteTree Route { get; set; }
    }
}
