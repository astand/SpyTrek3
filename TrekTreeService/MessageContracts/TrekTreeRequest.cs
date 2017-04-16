using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace TrekTreeService.MessageContracts
{
    [MessageContract]
    public class TrekTreeRequest
    {
        [MessageBodyMember]
        public string License { get; set; }

        [MessageBodyMember]
        public string Nonusing { get; set; }

        [MessageBodyMember]
        public string Imei { get; set; }

        [MessageBodyMember]
        public int? Year { get; set; }

        [MessageBodyMember]
        public int? Month { get; set; }

        [MessageBodyMember]
        public int? Day { get; set; }

        [MessageBodyMember]
        public string FName { get; set; }

    }

}
