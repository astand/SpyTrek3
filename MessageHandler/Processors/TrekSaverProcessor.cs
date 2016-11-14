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
        //public Action<String> WriteStatus;

        BidControl bidControl = new BidControl();

        private ITrekWriter trekWr = new FileTrekWriter();

        private String imeiPath;

        private Int32 noteCount;

        private Int32 trekSize = 0;

        private StringBuilder stateStr = new StringBuilder(255);

        public override void Process(FramePacket packet, ref IStreamData answer)
        {
            State = ProcState.Idle;
            noteCount = 0;

            stateStr.Clear();

            if (packet.Opc == OpCodes.DATA)
            {
                // data
                if (bidControl.Next(packet.Id))
                {
                    noteCount = SaveTrek(packet.Data, packet.Id);

                    var by_size = noteCount * NaviNote.Lenght;
                    var completed = (by_size * 100.0) / trekSize;

                    stateStr.Append($"({completed:F1} %) {by_size} bytes downloaded.");

                    answer = new FramePacket(opc: OpCodes.ACK, id: packet.Id, data: null);
                    if (packet.Data.Length == 0)
                    {
                        stateStr.Append(". Finished");
                    }
                }
                else
                {
                    return;
                }
            }
            else if (packet.Opc == OpCodes.RRQ)
            {
                State = ProcState.CmdAck;
                // Head confirmation
                trekSize = (packet.Data.Length >= 4) ? BitConverter.ToUInt16(packet.Data, 2) : (-1);
                stateStr.Append($"Cmd Ack. Wait file: {trekSize} Bytes");
                bidControl.Reset();
            }
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

        public override String ToString() => stateStr.ToString();
    }
}
