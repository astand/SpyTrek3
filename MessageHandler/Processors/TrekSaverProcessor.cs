using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;
using System.Diagnostics;
using MessageHandler.DataFormats;
using MessageHandler.TrekWriter;

namespace MessageHandler.Processors
{
    public class TrekSaverProcessor : IFrameProccesor
    {
        public Action<String> WriteStatus;

        Int32 block_id;

        private ITrekWriter trekWr = new FileTrekWriter();

        private StringBuilder statusString = new StringBuilder();

        private String imeiPath;

        public void Process(FramePacket packet, ref IStreamData answer, out ProcState state)
        {
            state = ProcState.Idle;
            statusString.Clear();
            Int32 note_counts = 0;

            if (packet.Opc == OpCodes.DATA)
            {
                // data
                if (block_id + 1 == packet.Id)
                {
                    block_id = packet.Id;
                    note_counts = SaveTrek(packet.Data, packet.Id);
                    statusString.Append($"Downloading... {note_counts * NaviNote.Lenght} Bytes.");
                    answer = new FramePacket(opc: OpCodes.ACK, id: packet.Id, data: null);
                    if (packet.Data.Length == 0)
                    {
                        state = ProcState.Finished;
                        statusString.Clear();
                        statusString.Append($"Finished {note_counts * NaviNote.Lenght} Bytes loaded.");
                    }
                }
                else return;
            }
            else if (packet.Opc == OpCodes.RRQ)
            {
                state = ProcState.CmdAck;
                // Head confirmation
                statusString.Append($"RRQ Answer received");
                block_id = 0;
            }

            WriteStatus?.Invoke(statusString.ToString());
        }


        public bool IsTrekNeed(TrekDescriptor desc) => trekWr.TrekCanBeWrite(imeiPath, desc);

        public void SetImeiPath(String imei)
        {
            imeiPath = imei;
        }

        private Int32 SaveTrek(byte[] data, UInt16 block_num)
        {
            var is_start_block = (block_num == OpCodes.kFirstDataBlockNum);

            Int32 current_offset = 0;
            bool parseOk;
            var notes = new List<NaviNote>();
            do
            {
                var trekNote = new NaviNote();
                parseOk = trekNote.TryParse(data, current_offset);
                if (parseOk)
                {
                    current_offset += NaviNote.Lenght;
                    notes.Add(trekNote);
                }
            }
            while (parseOk);

            /// try write all available notes
            return trekWr.WriteNotes(notes, is_start_block);
        }
    }
}
