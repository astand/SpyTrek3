using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamHandler
{
    public class ByteRate
    {
        DateTime start;

        public void MakeStartStamp()
        {
            start = DateTime.Now;
        }
        public double CalcKBperSec(Int32 passedSize)
        {
            if (start == null)
                return 0;

            var secDiff = (Int32)(DateTime.Now - start).TotalSeconds;

            return (passedSize / (secDiff * 1000.0));
        }
    }
}
