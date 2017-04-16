using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TrekTreeService.MessageContracts
{
    [MessageContract]
    public class TrekFile
    {
        [MessageBodyMember]
        public string Name { get; set; }

        [MessageBodyMember]
        public byte[] Content { get; set; }
    }
}
