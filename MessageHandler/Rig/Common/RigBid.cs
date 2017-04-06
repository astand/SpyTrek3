using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.Rig
{
    public class RigBid
    {
        public int Size { get; set; }

        public int Passed { get; set; }

        public int BidSend { get; set; }

        public int BidAck { get; set; }

        public int BidLast { get; set; }
    }
}
