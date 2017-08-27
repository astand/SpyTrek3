using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;
using System.Diagnostics;
using MessageHandler.DataFormats;

namespace MessageHandler.Rig.Processors
{
    public class EchoHandler : IReaderProcessor
    {
        static readonly int CACH_CAPACITY = 10;

        NaviNote[] statNotes = new NaviNote[CACH_CAPACITY];

        List<NaviNote> poslist_ = new List<NaviNote>(CACH_CAPACITY);

        int loadedCnt = 0;

        public EchoHandler() : base("Echo", Common.OpID.Echo)
        {
            for (int i = 0; i < CACH_CAPACITY; i++)
            {
                statNotes[i] = new NaviNote();
            }
        }

        protected override Boolean ProcessHead(RigFrame packet)
        {
            loadedCnt = 0;
            var offset = 4;
            poslist_.Clear();

            // do parsing by received datasize
            while ((loadedCnt < CACH_CAPACITY) && (offset + NaviNote.Lenght) <= packet.DataSize)
            {
                if (!statNotes[loadedCnt].TryParse(packet.Data, offset))
                    break;

                poslist_.Add(statNotes[loadedCnt]);
                loadedCnt++;
                offset += NaviNote.Lenght;
            }

            Debug.WriteLine($"DataSize= {packet.DataSize}. {loadedCnt} points were gotten. 1st : " + statNotes[0].GetStringNotify());
            return true;
        }

        protected override void ProcessData(RigFrame packet)
        {
        }

        public List<NaviNote> GetCachedList()
        {
            if (loadedCnt == 0)
            {
                poslist_.Clear();
            }

            loadedCnt = 0;
            return poslist_;
        }
    }
}
