using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;
using System.Diagnostics;
using MessageHandler.DataFormats;

namespace MessageHandler.Processors
{
    public class TrekSaverProcessor : IFrameProccesor
    {
        Int32 block_id;

        Int32 trekNoteNum;

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



        //public bool IsNewTrek(MatrixItem item)
        //{

        //}

        private void SaveTrek(byte[] data, UInt16 block_num)
        {
            if (block_num == OpCodes.kFirstDataBlockNum)
            {
                trekNoteNum = 0;
            }

            Int32 current_offset = 0;
            bool parseOk;
            do
            {
                var trekNote = new NaviNote();
                parseOk = trekNote.TryParse(data, current_offset);
                if (parseOk)
                {
                    current_offset += NaviNote.Lenght;
                    trekNoteNum++;
                    Debug.WriteLine($"[{trekNoteNum}] Trek: {trekNote.ToString()}");
                }
            }
            while (parseOk);
        }
    }
}
