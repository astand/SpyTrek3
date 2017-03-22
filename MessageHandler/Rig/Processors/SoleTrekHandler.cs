using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;
using MessageHandler.DataFormats;
using MessageHandler.TrekWriter;

namespace MessageHandler.Rig.Processors
{
    public class SoleTrekHandler : IReaderProcessor
    {
        public String ImeiPath {
            get;
            set;
        }

        private ITrekWriter trekWr = new FileTrekWriter();


        private Int32 noteCount;

        private Int32 trekSize = 0;

        public bool IsTrekNeed(TrekDescriptor desc) => trekWr.TrekCanBeWrite(ImeiPath, desc);

        protected override Boolean ProcessHead(RigFrame packet, ref IStreamData answer)
        {
            trekSize = BitConverter.ToUInt16(packet.Data, 2);
            return true;
        }
        protected override void ProcessData(RigFrame packet, ref IStreamData answer)
        {
            noteCount = SaveTrek(packet.Data, packet.BlockNum);
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
