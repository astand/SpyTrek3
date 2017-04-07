using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;
using StreamHandler;

namespace MessageHandler.Rig
{
    public class RigHandler : IRigHandler
    {
        public static Func<RigFrame, Int32> RigSender;

        public IFrameProccesor<RigFrame> ProcHandler {
            get;
        }

        public RigHandler(IFrameProccesor<RigFrame> processor)
        {
            ProcHandler = processor;
            ProcHandler.SendAnswer = new Action<RigFrame>(target => {
                RigSender?.Invoke(target);
            });
        }

        public override HandleResult HandleFrame(RigFrame frame)
        {
            if (!ProcHandler.FrameAccepted(frame))
            {
                return HandleResult.Ignored;
            }

            ProcHandler.Process(frame);
            return HandleResult.Handled;
        }
    }
}
