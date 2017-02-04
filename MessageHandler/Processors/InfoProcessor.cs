﻿using MessageHandler.DataFormats;
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
        StringBuilder stateStr = new StringBuilder(255);
        public SpyTrekInfo Info { get; private set; }

        BidControl bCtrl = new BidControl();

        public Action<SpyTrekInfo> OnUpdated;

        public override void Process(FramePacket packet, ref IStreamData answer)
        {
            stateStr.Clear();
            State = ProcState.Idle;
            if (packet.Opc == OpCodes.DATA)
            {
                if (bCtrl.Next(packet.Id))
                {
                    if (packet.Id == 1)
                    {
                        /// Pay load data placed in first data block
                        Info = new SpyTrekInfo();
                        Info.TryParse(Encoding.UTF8.GetString(packet.Data));
                        OnUpdated?.Invoke(Info);
                        State = ProcState.Data;
                    }
                    if (packet.Data.Length == 0)
                    {
                        stateStr.Append($"Info updated. {Info.Imei}");
                        State = ProcState.Finished;
                    }
                   
                    answer = new FramePacket(opc: OpCodes.ACK, id: packet.Id, data: null);
                }
            }
            else if (packet.Opc == OpCodes.RRQ)
            {
                State = ProcState.CmdAck;
                stateStr.Append($"Info. RRQ ACK");
                bCtrl.Reset();
            }
        }

        public override String ToString() => stateStr.ToString();
    }
}
