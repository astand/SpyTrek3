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

        public bool IsTrekNeed(TrekDescriptor desc) => trekWr.TrekCanBeWrite(ImeiPath, desc);


        public SoleTrekHandler() : base("SoleTrek", Common.OpID.SoleTrek)
        {
        }


        protected override Boolean ProcessHead(RigFrame packet)
        {
            bid.Size = BitConverter.ToInt32(packet.Data, 0);
            return true;
        }
        protected override void ProcessData(RigFrame packet)
        {
            noteCount = SaveTrek(packet.Data, packet.DataSize, packet.BlockNum);
        }

        private Int32 SaveTrek(byte[] data, int size, UInt16 block_num)
        {
            int current_offset = 0;
            var notes = new List<NaviNote>();

            while (current_offset + NaviNote.Lenght <= size)
            {
                var trekNote = new NaviNote();
                trekNote.TryParse(data, current_offset);
                current_offset += NaviNote.Lenght;
                notes.Add(trekNote);
            }

            /// This static class uses for save binary trek for using it in simulation
            //RawDataLogger.SaveArray(data, size);
            /// try write all available notes
            return trekWr.WriteNotes(notes, block_num == 1);
        }
    }
}
