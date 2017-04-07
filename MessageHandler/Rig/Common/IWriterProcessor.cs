using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;
using System.Timers;
using StreamHandler;
using System.Diagnostics;

namespace MessageHandler.Rig.Common
{
    public abstract class IWriterProcessor : IFrameProccesor<RigFrame>, IDisposable
    {
        protected RigBid bid = new RigBid();

        protected RigFrame rigFrame = new RigFrame();

        protected Int32 passWindow = 4;

        protected int FSize
        {
            get;
            set;
        }

        Int32 resendCnt = 0;

        Timer sendTimer = new Timer();
        Timer resendTimer = new Timer();
        OpID RigId;

        protected IWriterProcessor(string name, OpID selfId)
        {
            Name = name;
            RigId = selfId;
            resendTimer.Interval = 6000;
            sendTimer.Elapsed += SendTimer_Elapsed;
            resendTimer.Elapsed += ResendTimer_Elapsed;
        }

        private void ResendTimer_Elapsed(Object sender, ElapsedEventArgs e)
        {
            if (PState.State != ProcState.Data)
            {
                resendTimer.Stop();
                return;
            }

            if (resendCnt > 2)
                PState.State = ProcState.Idle;

            bid.BidSend = bid.BidAck + 1;
            resendCnt++;
        }

        private void SendTimer_Elapsed(Object sender, ElapsedEventArgs e)
        {
            if (PState.State != ProcState.Data)
            {
                sendTimer.Stop();
                return;
            }

            rigFrame.Opc = OpCode.DATA;
            rigFrame.RigId = OpID.Firmware;

            while ((bid.BidAck + passWindow) > bid.BidSend && (bid.BidSend != bid.BidLast))
            {
                resendTimer.Start();
                rigFrame.BlockNum = (UInt16)bid.BidSend;
                var readed = ReadDataChunk();

                if (readed == 0)
                    bid.BidLast = bid.BidSend;
                else
                    bid.BidSend++;

                Debug.WriteLine(Name + $": DATA SEND {rigFrame.BlockNum}. Length {rigFrame.Data.Length}");

                lock (SendAnswer)
                {
                    SendAnswer(rigFrame);
                }
            }
        }

        public void StartWriteAction()
        {
            PState.State = ProcState.CmdAck;

            if (OnWriteRequest() > 0)
                SendAnswer(rigFrame);
        }

        public override void Process(RigFrame packet)
        {
            if (packet.Opc == OpCode.WRQ && PState.State == ProcState.CmdAck)
            {
                // node acked wrq
                bid.BidAck = 0;
                bid.BidLast = 0;
                bid.BidSend = 1;
                PState.State = ProcState.Data;
                sendTimer.Interval = 50;
                sendTimer.Start();
                resendTimer.Start();
            }
            else if (packet.Opc == OpCode.ACK)
            {
                // node acked data frame
                if (packet.BlockNum == bid.BidAck + 1)
                {
                    resendCnt = 0;
                    bid.BidAck += 1;

                    if (bid.BidAck == bid.BidLast)
                        PState.State = ProcState.Finished;

                    OnAckReceive();
                }
            }
            else if (packet.Opc == OpCode.ERR)
            {
                // node return ERR code
                OnErrorReceive();
            }
        }


        public void Dispose()
        {
            sendTimer.Dispose();
            resendTimer.Dispose();
        }

        public sealed override Boolean FrameAccepted(RigFrame o)
        {
            return o.RigId == RigId;
        }

        protected virtual Int32 OnWriteRequest()
        {
            throw new NotImplementedException();
        }

        protected virtual Int32 OnAckReceive()
        {
            throw new NotImplementedException();
        }

        protected virtual Int32 OnErrorReceive()
        {
            throw new NotImplementedException();
        }

        protected virtual Int32 ReadDataChunk()
        {
            throw new NotImplementedException();
        }
    }
}
