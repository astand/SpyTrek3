using MessageHandler.DataFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.Notifiers
{
    public class NoteListEventArgs : EventArgs
    {
        public NoteListEventArgs(List<TrekDescriptor> list)
        {
            NoteList = list;
        }

        public List<TrekDescriptor> NoteList { get; }
    }
}
