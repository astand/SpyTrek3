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

        List<TrekDescriptor> list = new List<TrekDescriptor>();

        public TrekDescriptor Trek(Int32 trek_id) => list.Where(o => o.Id == trek_id).FirstOrDefault();

        public TrekListHandler() : base("TrekList", Common.OpID.TrekList)
        {
        }

        protected override Boolean ProcessHead(RigFrame packet)
        {
            bid.Size = BitConverter.ToInt32(packet.Data, 0);
            return true;
        }

        protected override void ProcessData(RigFrame packet)
        {
            ProcessTrekDescriptors(packet.Data, packet.DataSize, packet.BlockNum);
        }

        private void ProcessTrekDescriptors(Byte[] data, int size, UInt16 block_num)
        {
            if (block_num == 1)
                list.Clear();

            Int32 current_offset = 0;

            while (current_offset + TrekDescriptor.Length <= size)
            {
                // must be declared new instance on each iteration for list fullfilling
                var oneTrek = new TrekDescriptor();
                oneTrek.TryParse(data, current_offset);
                list.Add(oneTrek);
                current_offset += TrekDescriptor.Length;
            }

            OnUpdated?.Invoke(list, block_num == 1);
        }
    }
}
