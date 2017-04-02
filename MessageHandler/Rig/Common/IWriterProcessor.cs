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
        protected Piper piper;

        protected RigBid bid = new RigBid();

        protected RigFrame rigFrame = new RigFrame();

        protected Int32 passWindow = 4;

        protected int FSize {
            get;
            set;
        }

        private Timer sendTimer = new Timer();

        protected virtual void SetName(string name)
        {
            Name = name;
            sendTimer.Elapsed += SendTimer_Elapsed;
        }

        private void SendTimer_Elapsed(Object sender, ElapsedEventArgs e)
        {
            if (PState.State != ProcState.Data)
            {
                return;
            }

            rigFrame.Opc = OpCode.DATA;
            rigFrame.RigId = OpID.Firmware;

            while ((bid.BidAck + passWindow) > bid.BidSend && (bid.BidSend != bid.BidLast))
            {
                rigFrame.BlockNum = (UInt16)bid.BidSend;
                var readed = ReadDataChunk();
                ++bid.BidSend;

                if (readed == 0)
                    // last block sent
                    bid.BidLast = bid.BidSend;

                Debug.WriteLine(Name + $": DATA SEND {rigFrame.BlockNum}. Length {rigFrame.Data.Length}");

                lock (piper)
                {
                    piper.SendData(rigFrame);
                }
            }
        }

        public void StartWriteAction()
        {
            PState.State = ProcState.CmdAck;

            if (OnWriteRequest() > 0)
                piper.SendData(rigFrame);
        }

        public override void Process(RigFrame packet, ref IStreamData answer)
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
            }
            else if (packet.Opc == OpCode.ACK)
            {
                // node acked data frame
                if (packet.BlockNum == bid.BidAck + 1)
                {
                    bid.BidAck += 1;
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
        }

        private void ScheduleSendingData(Object sender, ElapsedEventArgs e)
        {
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
