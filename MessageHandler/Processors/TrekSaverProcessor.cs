using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;
using System.Diagnostics;

namespace MessageHandler.Processors
{
    public class TrekSaverProcessor : IFrameProccesor
    {
        Int32 block_id;

        public void Process(FramePacket packet, ref IStreamData answer)
        {
            if (packet.Opc == OpCodes.DATA)
            {
                // data
                if (block_id + 1 == packet.Id)
                {
                    block_id = packet.Id;
                    SaveTrek(packet.Data, packet.Id);
                    answer = new FramePacket(opc: OpCodes.ACK, id: packet.Id, data: null);
                }
                else return;
            }
            else if (packet.Opc == OpCodes.RRQ)
            {
                // Head confirmation
                block_id = 0;
            }
        }

        private void SaveTrek(byte[] data, UInt16 block_num)
        {
            Debug.WriteLine($"TrekSave action: date length = {data.Length}");
        }
    }
}
