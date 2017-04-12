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
        Func<RigFrame, Int32> rigSender;

        public IFrameProccesor<RigFrame> ProcHandler {
            get;
        }

        public RigHandler(IFrameProccesor<RigFrame> processor, Func<RigFrame, Int32> func)
        {
            rigSender = func;
            ProcHandler = processor;
            ProcHandler.SendAnswer = new Action<RigFrame>(target => {
                rigSender?.Invoke(target);
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
