using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;

namespace MessageHandler.Processors
{
    public class ErrorProcessor : IFrameProccesor
    {
        public override void Process(FramePacket packet, ref IStreamData answer)
        {
            throw new NotImplementedException();
        }
    }
}
