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
        public NaviNote Pos { get; protected set; } = new NaviNote();

        public EchoHandler() : base("Echo", Common.OpID.Echo)
        {
        }

        protected override Boolean ProcessHead(RigFrame packet)
        {
            Pos.TryParse(packet.Data, 4);
            Debug.WriteLine(Pos.GetStringNotify());
            return true;
        }

        protected override void ProcessData(RigFrame packet)
        {
        }
    }
}
