using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

namespace StreamHandler
{

    public class ByteRate : IDisposable
    {

        public int Rate
        {
            get;
            set;
        }

        Timer tim = new Timer();

        int passedBytes = 0;

        public ByteRate()
        {
            tim.Interval = 1000;
            tim.Elapsed += Tim_Elapsed;
            tim.Start();
        }

        public void PassData(int size)
        {
            passedBytes += size;
        }

        private void Tim_Elapsed(Object sender, ElapsedEventArgs e)
        {
            Rate = passedBytes;
            passedBytes = 0;
        }

        public void Dispose()
        {
            tim.Dispose();
        }
    }
}

