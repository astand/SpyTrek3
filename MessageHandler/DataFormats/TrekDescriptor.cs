using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.DataFormats
{
    class TrekDescriptor
    {
        private UInt16 id;

        private DateTime start;

        private DateTime stop;

        public UInt32 TrekSize { get; private set; }
        public UInt32 Dist { get; set; }
        public UInt32 Odometr { get; set; }



        private static readonly Int32 MINIMUM_DISTANCE_DETECT = (200 * 10);
    };

}

