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

        public List<NaviNote> PosList = new List<NaviNote>();

        public NaviNote Pos { get => PosList[0]; }

        public EchoHandler() : base("Echo", Common.OpID.Echo)
        {
            for (int i = 0; i < 20; i++)
            {
                PosList.Add(new NaviNote());
            }
        }

        protected override Boolean ProcessHead(RigFrame packet)
        {
            var listIndex = 0;
            var offset = 4;
            var parsingFinished = false;

            while (parsingFinished = PosList[listIndex].TryParse(packet.Data, offset) == true)
            {
                listIndex++;
                offset += NaviNote.Lenght;
            }

            Debug.WriteLine($"{listIndex} points were gotten. 1st : " + Pos.GetStringNotify());
            return true;
        }

        protected override void ProcessData(RigFrame packet)
        {
        }
    }
}
