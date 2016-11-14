using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.Processors
{
    public class BidControl
    {
        private Int32 bid;
        public void Reset() => bid = 0;

        public bool Next(Int32 nbid)
        {
            if (nbid == bid + 1)
            {
                bid += 1;
                return true;
            }

            return (false);
        }

    }
}
