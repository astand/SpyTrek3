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
        public SpyTrekInfo Info { get; }

        private Int32 block_synchro = 0;

        public Action<SpyTrekInfo> OnUpdated;

        public override void Process(FramePacket packet, ref IStreamData answer)
        {
            if (packet.Opc == OpCodes.DATA)
            {
                if (CheckBlockSynchro(packet.Id))
                {
                    if (packet.Id == 1)
                    {
                        /// Pay load data placed in first data block
                        SpyTrekInfo Info = new SpyTrekInfo();
                        Info.TryParse(Encoding.UTF8.GetString(packet.Data));
                        OnUpdated?.Invoke(Info);
                    }
                    answer = new FramePacket(opc: OpCodes.ACK, id: packet.Id, data: null);
                }
            }
            else if (packet.Opc == OpCodes.RRQ)
                ResetBlockSynchro();


        }

        private bool CheckBlockSynchro(UInt16 id)
        {
            if (block_synchro + 1 == id)
            {
                block_synchro += 1;
                return true;
            }
            return false;
        }

        private void ResetBlockSynchro() => block_synchro = 0;
    }
}
