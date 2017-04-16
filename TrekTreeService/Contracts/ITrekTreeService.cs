using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TrekTreeService.MessageContracts;

namespace TrekTreeService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITrekTreeService" in both code and config file together.
    [ServiceContract]
    public interface ITrekTreeService
    {
        [OperationContract]
        TrekTreeCollection GetTrekTreeCollection(TrekTreeRequest request);

        [OperationContract]
        TrekFile GetTrekFile(TrekTreeRequest request);

        [OperationContract]
        TrekNodePoint GetTrekNodePoint(TrekTreeRequest request);

    }
}
