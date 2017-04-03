using System;
using System.Text;
using System.Diagnostics;
using StreamHandler.Abstract;
using MessageHandler.DataFormats;

namespace MessageHandler.Rig.Processors
{
    public class InfoHandler : IReaderProcessor
    {
        public Action<SpyTrekInfo> OnUpdated;

        public SpyTrekInfo Info {
            get;
            private set;
        }

        public InfoHandler()
        {
            SetName("INFO");
        }

        protected override Boolean ProcessHead(RigFrame packet, ref IStreamData answer)
        {
            bid.Size = BitConverter.ToUInt32(packet.Data, 0);

            if (Info == null)
            {
                Info = new SpyTrekInfo();
            }

            return true;
        }

        protected override void ProcessData(RigFrame packet, ref IStreamData answer)
        {
            if (packet.BlockNum == 1)
            {
                Info.TryParse(Encoding.UTF8.GetString(packet.Data));
                OnUpdated?.Invoke(Info);
            }
        }
    }
}
