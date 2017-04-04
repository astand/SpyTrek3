using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

namespace StreamHandler
{

    public class ByteRate
    {

        public int Rate
        {
            get {
                var ret = passedBytes;
                passedBytes = 0;
                return ret;
            }
        }

        int passedBytes = 0;

        public ByteRate()
        {
        }

        public void PassData(int size)
        {
            passedBytes += size;
        }
    }
}

