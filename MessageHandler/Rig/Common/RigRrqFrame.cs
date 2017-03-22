using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.Rig.Common
{
    public class RigRrqFrame : RigFrame
    {
        public RigRrqFrame(OpID rigId)
        {
            Opc = OpCode.RRQ;
            RigId = rigId;
            BlockNum = 0;
            Data = new byte[0];
        }
    }
}
