﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;
using MessageHandler;
using MessageHandler.DataFormats;
using System.Diagnostics;
using StreamHandler;

namespace MessageHandler.Processors
{
    public class TrekDescriptorProcessor : IFrameProccesor
    {
        public Action<List<TrekDescriptor>, bool> OnUpdated;

        private List<TrekDescriptor> list = new List<TrekDescriptor>();

        BidControl bidControl = new BidControl();

        StringBuilder stateStr = new StringBuilder(255);

        ByteRate byteRate = new ByteRate();

        Int32 recSize = 0;

        public override void Process(FramePacket packet, ref IStreamData answer)
        {
            stateStr.Clear();
            State = ProcState.Idle;
            if (packet.Opc == OpCodes.DATA)
            {
                if (bidControl.Next(packet.Id))
                {
                    State = ProcState.Data;
                    ProcessTrekDescriptors(packet.Data, packet.Id);
                    HandleByteRate(packet);
                    answer = new FramePacket(opc: OpCodes.ACK, id: packet.Id, data: null);
                    if (packet.Data.Length == 0)
                    {
                        State = ProcState.Finished;
                        stateStr.Append("Finished. ");
                    }
                }
                else
                {
                    /// wrong BlockID received
                }
            }
            else if (packet.Opc == OpCodes.RRQ)
            {
                stateStr.Append("Descriptors. RRQ ACK");
                State = ProcState.CmdAck;
                bidControl.Reset();
                recSize = 0;
                byteRate.MakeStartStamp();
            }
        }

        private void HandleByteRate(FramePacket packet)
        {
            if (packet.Data.Length == 0)
                return;

            recSize += packet.Data.Length;
            var tmp_kBps = byteRate.CalcKBperSec(recSize);
            stateStr.Append($"Speed: {tmp_kBps:F1} kBps");
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
            stateStr.Clear();

            if (block_num == 1)
                list.Clear();

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

            stateStr.Append($"Notes count: {list.Count}... ");

            OnUpdated?.Invoke(list, block_num == 1);
        }

        public override String ToString() => stateStr.ToString();
    }
}
