using System;
using StreamHandler.Abstract;
using System.Diagnostics;
using MessageHandler.Rig.Common;

namespace MessageHandler.Rig
{
    public abstract class IReaderProcessor: IFrameProccesor<RigFrame>
    {
        protected RigBid bid = new RigBid();

        protected virtual void SetName(string name)
        {
            Name = name;
        }

        public override void Process(RigFrame packet, ref IStreamData answer)
        {
            if (packet.Opc == OpCode.RRQ)
            {
                if (ProcessHead(packet, ref answer))
                {
                    PState.State = ProcState.Data;
                    bid.BidAck = 0;
                }
            }
            else if (packet.Opc == OpCode.DATA)
            {
                if ((bid.BidAck + 1) == packet.BlockNum && PState.State == ProcState.Data)
                {
                    ProcessData(packet, ref answer);
                    bid.BidAck += 1;

                    if (packet.Data.Length == 0)
                    {
                        PState.State = ProcState.Finished;
                    }
                }
                else
                {
                    Debug.WriteLine(Name + " : Warning: Non expecting block");
                }

                if (answer == null)
                {
                    answer = new RigFrame()
                    {
                        Opc = OpCode.ACK,
                        RigId = packet.RigId,
                        BlockNum = (UInt16)bid.BidAck,
                        Data = new byte[0]
                    };
                }
            }
        }

        protected abstract bool ProcessHead(RigFrame packet, ref IStreamData answer);

        protected abstract void ProcessData(RigFrame packet, ref IStreamData answer);
    }
}
