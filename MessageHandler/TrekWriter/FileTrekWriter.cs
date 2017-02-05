using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageHandler.DataFormats;
using System.Diagnostics;

namespace MessageHandler.TrekWriter
{
    public class FileTrekWriter : ITrekWriter
    {
        Int32 currentOffset = 0;

        TrekFileFolder trekFolder = new TrekFileFolder("user/");

        public void ResetWriter()
        {
            throw new NotImplementedException();
        }

        public Boolean TrekCanBeWrite(String idPath, TrekDescriptor desc) => trekFolder.IsTrekNotPresented(desc, idPath);

        public Int32 WriteNotes(List<NaviNote> notes, Boolean start)
        {
            if (start)
            {
                currentOffset = 0;
            }

            trekFolder.AddNoteList(notes);
            currentOffset += notes.Count;

            return currentOffset;
        }
    }
}
