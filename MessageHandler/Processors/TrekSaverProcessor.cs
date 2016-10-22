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
        public Action<String> onData;

        Int32 block_id;

        Int32 trekNoteNum;

        private StringBuilder statusString = new StringBuilder();

        private String imeiPath;

        public void Process(FramePacket packet, ref IStreamData answer)
        {
            statusString.Clear();

            if (packet.Opc == OpCodes.DATA)
            {
                // data
                if (block_id + 1 == packet.Id)
                {
                    block_id = packet.Id;
                    SaveTrek(packet.Data, packet.Id);
                    statusString.Append($"Downloading... {trekNoteNum}");
                    answer = new FramePacket(opc: OpCodes.ACK, id: packet.Id, data: null);
                }
                else return;
            }
            else if (packet.Opc == OpCodes.RRQ)
            {
                // Head confirmation
                statusString.Append($"RRQ Answer received");
                block_id = 0;
            }

            onData?.Invoke(statusString.ToString());
        }


        public bool IsTrekNeed(TrekDescriptor desc)
        {
            return true;
        }
     
        public void SetImeiPath(String imei)
        {
            imeiPath = imei;
        }

        private Int32 SaveTrek(byte[] data, UInt16 block_num)
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
                    Debug.WriteLine($"{imeiPath}:[{trekNoteNum}] Trek: {trekNote.ToString()}");
                }
            }
            while (parseOk);

            return trekNoteNum;
        }
    }
}
