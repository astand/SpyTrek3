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
    public class TrekTreeCollection
    {
        [MessageBodyMember]
        public List<TrekTreeInstance> collection { get; set; }

        public TrekTreeCollection()
        {
            collection = new List<TrekTreeInstance>();
        }
    }
}
