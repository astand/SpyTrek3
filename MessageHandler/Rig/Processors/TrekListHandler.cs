using System;
using System.Collections.Generic;
using System.Linq;
using StreamHandler.Abstract;
using MessageHandler.DataFormats;

namespace MessageHandler.Rig.Processors
{
    public class TrekListHandler : IReaderProcessor
    {
        public Action<List<TrekDescriptor>, bool> OnUpdated;

        RigFrame rigFrame = new RigFrame();

        List<TrekDescriptor> list = new List<TrekDescriptor>();

        public TrekDescriptor Trek(Int32 trek_id) => list.Where(o => o.Id == trek_id).FirstOrDefault();

        public TrekListHandler()
        {
            SetName("TrekList");
        }

        protected override Boolean ProcessHead(RigFrame packet, ref IStreamData answer)
        {
            bid.Size = BitConverter.ToInt32(packet.Data, 0);
            return true;
        }

        protected override void ProcessData(RigFrame packet, ref IStreamData answer)
        {
            ProcessTrekDescriptors(packet.Data, packet.BlockNum);
        }

        private void ProcessTrekDescriptors(Byte[] data, UInt16 block_num)
        {
            if (block_num == 1)
                list.Clear();

            bool parseOk;
            Int32 current_offset = 0;

            do
            {
                // must be declared new instance on each iteration for list fullfilling
                var oneTrek = new TrekDescriptor();
                parseOk = oneTrek.TryParse(data, current_offset);

                if (parseOk == true)
                {
                    list.Add(oneTrek);
                    current_offset += TrekDescriptor.Length;
                }
            } while (parseOk);

            OnUpdated?.Invoke(list, block_num == 1);
        }
    }
}
