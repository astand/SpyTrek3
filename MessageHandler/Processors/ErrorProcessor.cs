using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;

namespace MessageHandler.Processors
{
    /// <summary>
    ///  Error processor handle all frames with OpCode.ERROR
    /// </summary>
    public class ErrorProcessor : IFrameProccesor
    {
        StringBuilder stateStr = new StringBuilder(255);

        public override void Process(FramePacket packet, ref IStreamData answer)
        {
            stateStr.Clear();
            State = ProcState.Idle;

            if (packet.Opc == OpCodes.ERROR)
            {
                State = ProcState.CmdAck;
                stateStr.Append($"Protocol Error.");

                if (packet.Id == ErrorCode.NoTrek)
                {
                    stateStr.Append($"Trek connot be found.");
                }
                else if (packet.Id == ErrorCode.NonSupportedOpc)
                {
                    stateStr.Append($"This opc not supported.");
                }
                else
                {
                    ///  Unknown Error code
                    stateStr.Append($"Unknown Error code.");
                }

                stateStr.Append(" Data: ");

                foreach (var item in packet.Data)
                {
                    stateStr.Append($"{item:X2} ");
                }
            }
            else
            {
                stateStr.Append("WTF. I cannot be here : error processor with non-ERROR code");
            }
        }

        public override String ToString() => stateStr.ToString();
    }
}
