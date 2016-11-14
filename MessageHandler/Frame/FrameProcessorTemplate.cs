using StreamHandler.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler
{
    /// <summary>
    /// This abstract class uses in ReadMessageHandler data process as main
    /// Payload Handler. It contains common dispatcher logic for ReadProcess 
    /// action. Each file handler must implement self handlers
    /// </summary>
    public abstract class FrameProccesorTemplate : IFrameProccesor
    {
        public override void Process(FramePacket packet, ref IStreamData answer)
        {
            if (packet.Opc == OpCodes.RRQ)
                ProcessHead(packet);
            else if (packet.Opc == OpCodes.DATA)
                ProcessData(packet);
            else
                ProcessError(packet);

        }

        protected abstract void ProcessHead(FramePacket packet);

        protected abstract void ProcessData(FramePacket packet);

        protected abstract void ProcessError(FramePacket packet);
    }
}
