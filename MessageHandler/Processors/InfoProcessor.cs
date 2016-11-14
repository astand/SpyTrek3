using MessageHandler.DataFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;
using System.Diagnostics;
using MessageHandler.Notifiers;
using System.Threading;

namespace MessageHandler.Processors
{
    public class InfoProcessor : IFrameProccesor
    {
        public SpyTrekInfo Info { get; }

        BidControl bCtrl = new BidControl();

        public Action<SpyTrekInfo> OnUpdated;

        public override void Process(FramePacket packet, ref IStreamData answer)
        {
            State = ProcState.Idle;
            if (packet.Opc == OpCodes.DATA)
            {
                if (bCtrl.Next(packet.Id))
                {
                    if (packet.Id == 1)
                    {
                        /// Pay load data placed in first data block
                        SpyTrekInfo Info = new SpyTrekInfo();
                        Info.TryParse(Encoding.UTF8.GetString(packet.Data));
                        OnUpdated?.Invoke(Info);
                        State = ProcState.Data;
                    }
                    if (packet.Data.Length == 0)
                    {
                        State = ProcState.Finished;
                    }
                    answer = new FramePacket(opc: OpCodes.ACK, id: packet.Id, data: null);
                    if (packet.Data.Length == 0)
                    {
                        State = ProcState.Finished;
                    }
                }
            }
            else if (packet.Opc == OpCodes.RRQ)
            {
                State = ProcState.CmdAck;
                bCtrl.Reset();
            }
        }
    }
}
