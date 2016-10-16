using MessageHandler.DataFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.Notifiers
{
    public class InfoEventArgs : EventArgs
    {
        public InfoEventArgs(SpyTrekInfo info)
        {
            spyTrekInfo = info;
        }

        public SpyTrekInfo spyTrekInfo { get; set; }
    }
}
