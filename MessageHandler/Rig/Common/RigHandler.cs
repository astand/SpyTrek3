using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;

namespace MessageHandler.Rig
{
    public class RigHandler : IRigHandler
    {
        public IFrameProccesor<RigFrame> ProcHandler
        {
            get;
        }

        Func<RigFrame, bool> selector;

        public RigHandler(Func<RigFrame, bool> selector, IFrameProccesor<RigFrame> processor)
        {
            ProcHandler = processor;
            this.selector = selector;
        }

        public override HandleResult HandleFrame(RigFrame frame, ref IStreamData answer)
        {
            if (!selector(frame))
            {
                return HandleResult.Ignored;
            }

            ProcHandler.Process(frame, ref answer);
            return HandleResult.Handled;
        }
    }
}
