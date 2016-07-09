using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler
{
    public interface IFrameSpecification
    {
        Boolean IsHead(UInt16 opc);

        Boolean IsData(UInt16 opc);
    }
}
