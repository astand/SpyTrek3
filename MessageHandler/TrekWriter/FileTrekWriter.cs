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
        public void ResetWriter()
        {
            throw new NotImplementedException();
        }

        public Boolean TrekCanBeWrite(String idPath, TrekDescriptor desc)
        {
            return true;
        }

        public Int32 WriteNotes(List<NaviNote> notes, Boolean start)
        {
            if (start)
            {
                currentOffset = 0;
            }

            foreach (var item in notes)
            {
                Debug.WriteLine($"Note [{currentOffset}]: {item.ToString()}");
                currentOffset++;
            }

            return currentOffset;
        }
    }
}
