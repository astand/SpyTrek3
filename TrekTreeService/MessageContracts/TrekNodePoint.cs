
using MessageHandler.DataFormats;
using System.ServiceModel;

namespace TrekTreeService.MessageContracts
{
    [MessageContract]
    public class TrekNodePoint
    {
        [MessageBodyMember]
        public string Date { get; set; }

        [MessageBodyMember]
        public double Lon { get; set; }

        [MessageBodyMember]
        public double Lat { get; set; }


        [MessageBodyMember]
        public double Spd { get; set; }

        [MessageBodyMember]
        public double Dist { get; set; }

        [MessageBodyMember]
        public double Kurs { get; set; }
    }
}