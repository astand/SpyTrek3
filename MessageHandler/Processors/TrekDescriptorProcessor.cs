using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;
using MessageHandler;
using MessageHandler.DataFormats;
using System.Diagnostics;

namespace MessageHandler.Processors
{
    public class TrekDescriptorProcessor : IFrameProccesor
    {
        private List<TrekDescriptor> list = new List<TrekDescriptor>();

        public Action<List<TrekDescriptor>, bool> OnUpdated;

        public void Process(FramePacket packet, ref IStreamData answer, out State state)
        {
            state = State.Idle;

            if (packet.Opc == OpCodes.DATA)
            {
                ProcessTrekDescriptors(packet.Data, packet.Id);
                answer = new FramePacket(opc: OpCodes.ACK, id: packet.Id, data: null);
                if (packet.Data.Length == 0)
                {
                    state = State.Finished;
                }
            }
        }

        public Int32 GetTrekID(int index_in_list)
        {
            if (index_in_list >= list.Count)
            {
                return -1;
            }

            var item = list[index_in_list];

            return item.Id;
        }

        /// <summary>
        /// Return TrekDescriptor
        /// </summary>
        /// <param name="trek_id">Requested Trek ID</param>
        /// <returns>Instance in case of existing</returns>
        public TrekDescriptor GetDescriptor(Int32 trek_id) => list.Where(o => o.Id == trek_id).FirstOrDefault();

        private void ProcessTrekDescriptors(Byte[] data, UInt16 block_num)
        {
            Debug.WriteLine("Blocks num " + block_num);

            if (block_num == 1)
            {
                list.Clear();
            }

            Int32 current_offset = 0;
            bool parseOk;
            do
            {
                var dsc = new TrekDescriptor();
                parseOk = dsc.TryParse(data, current_offset);

                if (parseOk == true)
                {
                    list.Add(dsc);
                    current_offset += TrekDescriptor.Length;
                }

            } while (parseOk);

            OnUpdated?.Invoke(list, block_num == 1);
        }
    }
}
