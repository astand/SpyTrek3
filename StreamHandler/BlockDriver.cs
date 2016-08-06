using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamHandler
{
    public class BlockDriver
    {
        public UInt16 BidSend { get; set; }

        public UInt16 BidAck { get; set; } = 0;

        public UInt16 BidLast { get; set; }

        private readonly int m_resend_count = 2;

        private readonly int m_bid_window = 8;

        public BlockDriver() { }

        public bool PassAckBlock(UInt16 ack_block_id)
        {
            if (ack_block_id == BidAck + 1)
            {
                BidAck += 1;
                return true;
            }

            return false;
        }

        public bool IsLast => BidLast == BidSend;

        public bool IsLastAck => BidAck == BidLast;

        public bool IsWindAllow() => ((BidAck + m_bid_window) > BidSend) && !IsLast;

        public void Reset()
        {
            BidLast = BidAck = 0;
            BidSend = 1;
        }
    }
}
