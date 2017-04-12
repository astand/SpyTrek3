using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.Rig.Common
{
    public class RigRrqTrekFrame : RigRrqFrame
    {
        public RigRrqTrekFrame(UInt16 trekId) : base(OpID.SoleTrek)
        {
            Data = BitConverter.GetBytes(trekId);
        }
    }
}
