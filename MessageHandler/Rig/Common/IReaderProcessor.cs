using System;
using StreamHandler.Abstract;
using System.Diagnostics;
using MessageHandler.Rig.Common;

namespace MessageHandler.Rig
{
    public abstract class IReaderProcessor: IFrameProccesor<RigFrame>
    {
        protected RigBid bid = new RigBid();

        OpID RigId;

        protected IReaderProcessor(string name, OpID selfRig)
        {
            Name = name;
            RigId = selfRig;
        }

        protected virtual void SetName(string name)
        {
            Name = name;
        }

        public override void Process(RigFrame packet)
        {
            if (packet.Opc == OpCode.RRQ)
            {
                if (ProcessHead(packet))
                {
                    PState.State = ProcState.Data;
                    bid.BidAck = 0;
                    bid.Passed = 0;
                }

                PState.Message = Name + $": RRQ ack. Size {bid.Size}";
            }
            else if (packet.Opc == OpCode.DATA)
            {
                if ((bid.BidAck + 1) == packet.BlockNum && PState.State == ProcState.Data)
                {
                    bid.Passed += packet.Data.Length;
                    bid.BidAck += 1;
                    PState.Message = Name + $": DATA passed {bid.Passed} of {bid.Size}";

                    if (packet.Data.Length == 0)
                    {
                        PState.State = ProcState.Finished;
                        PState.Message += ". Finished.";
                    }

                    ProcessData(packet);
                }
                else
                {
                    Debug.WriteLine(Name + " : Warning: Non expecting block");
                }

                SendAnswer(new RigFrame()
                {
                    Opc = OpCode.ACK,
                    RigId = packet.RigId,
                    BlockNum = packet.BlockNum,
                    Data = new byte[0]
                });
            }
        }

        public override sealed bool FrameAccepted(RigFrame fr)
        {
            return fr.RigId == RigId;
        }

        protected abstract bool ProcessHead(RigFrame packet);

        protected abstract void ProcessData(RigFrame packet);
    }
}
