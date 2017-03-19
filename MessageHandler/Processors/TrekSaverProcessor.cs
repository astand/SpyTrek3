using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;
using System.Diagnostics;
using MessageHandler.DataFormats;
using MessageHandler.TrekWriter;
using StreamHandler;

namespace MessageHandler.Processors
{
    public class TrekSaverProcessor : IFrameProccesor
    {
        BidControl bidControl = new BidControl();

        private ITrekWriter trekWr = new FileTrekWriter();

        private String imeiPath;

        private Int32 noteCount;

        private Int32 trekSize = 0;

        private StringBuilder stateStr = new StringBuilder(255);

        ByteRate byteRate = new ByteRate();

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
                    State = ProcState.Data;
                    noteCount = SaveTrek(packet.Data, packet.Id);
                    var acked_size = noteCount * NaviNote.Lenght;
                    HandleSizePassed(acked_size);
                    HandleByteRate(acked_size);
                    answer = new FramePacket(opc: OpCodes.ACK, id: packet.Id, data: null);

                    if (packet.Data.Length == 0)
                    {
                        State = ProcState.Finished;
                        stateStr.Append(". Finished");
                    }
                }
                else
                {
                    stateStr.Append($"Unexpected packet ID: {packet.Id}. Exp: {bidControl.Expected}.");
                    return;
                }
            }
            else if (packet.Opc == OpCodes.RRQ)
            {
                State = ProcState.CmdAck;
                // Head confirmation
                trekSize = (packet.Data.Length >= 4) ? BitConverter.ToUInt16(packet.Data, 2) : (-1);
                stateStr.Append($"Trek. RRQ ACK. File size: {trekSize} Bytes");
                bidControl.Reset();
                byteRate.MakeStartStamp();
            }
        }

        private void HandleSizePassed(Int32 acked_size)
        {
            var percent = ((acked_size * 100.0) / trekSize).ToString("F2").PadRight(6, ' ');
            var bytes = $"{acked_size} from {trekSize}".PadRight(18, ' ');
            stateStr.Append($"({percent} %)  {bytes}. ");
        }

        private void HandleByteRate(Int32 received_size)
        {
            var tmp_kBps = byteRate.CalcKBperSec(received_size);
            stateStr.Append($"Speed: {tmp_kBps:F1} kBps");
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
