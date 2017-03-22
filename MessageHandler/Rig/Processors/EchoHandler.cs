using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;
using System.Diagnostics;

namespace MessageHandler.Rig.Processors
{
    public class EchoHandler : IReaderProcessor
    {
        protected override Boolean ProcessHead(RigFrame packet, ref IStreamData answer)
        {
            return true;
        }

        protected override void ProcessData(RigFrame packet, ref IStreamData answer)
        {
            Debug.WriteLine($"Echo data frame handled. Block {packet.BlockNum}");
        }
    }
}
