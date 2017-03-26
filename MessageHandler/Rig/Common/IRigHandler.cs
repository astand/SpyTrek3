using StreamHandler.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.Rig
{
    public enum HandleResult { Ignored, Handled }


    public abstract class IRigHandler
    {
        public abstract HandleResult HandleFrame(RigFrame frame, ref IStreamData answer);
    }
}
