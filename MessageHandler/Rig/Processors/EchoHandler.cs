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
        public NaviNote Pos { get; protected set; }

        public EchoHandler()
        {
            SetName("Echo");
        }

        protected override Boolean ProcessHead(RigFrame packet, ref IStreamData answer)
        {
            Pos.TryParse(packet.Data, 4);
            Debug.WriteLine(Pos.GetStringNotify());
            return true;
        }

        protected override void ProcessData(RigFrame packet, ref IStreamData answer)
        {
        }
    }
}
