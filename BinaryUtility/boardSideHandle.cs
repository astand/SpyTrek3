using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtSys
{
    public sealed class boardSideHandle : G2_Client
    {
        FileUpload oufile = new FileUpload(@"test_data");
        private string testInfoString = "IMEI=2323476283462834,VERSION=1.0.0,NAME=KuKU";
        /// <summary>
        /// 
        /// </summary>
        private System.Timers.Timer sendtimeout = new System.Timers.Timer();
        private System.Timers.Timer waittimeout = new System.Timers.Timer();


        //new public void Dispose(bool v)
        //{
        //  Dispose();
        //}

        protected override void Dispose(bool disposing)
        {
            sendtimeout.Dispose();
            waittimeout.Dispose();
            GC.SuppressFinalize(this);
            return;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="lenrx"></param>
        protected override void mainDataHandler(int lenrx)
        {
            ReqProcess();

            if (UnpackReceive(lenrx) != 0) { return; }

            RecProcess();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ReqProcess()
        {
            int ret = -1;

            if (SendMachine.st_Transfer == TransState.SEND_DATA)
            {
                if (SendMachine.reqFile == TfBase.ID_INFO)
                {
                    Encoding ec1 = Encoding.UTF8;
                    from0.DATA = ec1.GetBytes("IMEI=2323476283462834,VERSION=1.0.0,NAME=KuKU");
                    ret = from0.DATA.Length;
                    if (bchief.bidsend != 1)
                        ret = 0;
                }

                else if (SendMachine.reqFile == TfBase.ID_FILENOTES)
                {
                    if (!bchief.ISLASTFREEZE)
                    {
                        from0 = new GTftp2(650 + 4);
                        ret = oufile.TryReadFromFile((UInt16)(bchief.bidsend - 1), 650, from0.DATA);
                    }
                }
                //else if (SendMachine.reqFile == TfBase.ID_ECHO) { }
                //else if (SendMachine.reqFile == TfBase.ID_NONE) { }
                else { }

            }
            else { }


            if (ret < 0)
            { }
            else
            {
                if (!bchief.IsBlockSendPermitted())
                {
                    return;
                }
                sendtimeout.Start();
                from0.headUp(TfBase.OPC_DATA, bchief.bidsend);
                m_netraw.handleOutStream(from0.getBytes(ret));
                bchief.FixLastBid(ret);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blocklen"></param>
        /// <returns></returns>
        protected override int UnpackReceive(int blocklen) => base.UnpackReceive(blocklen);


        /// <summary>
        /// Client side ReceiveProccess handler
        /// </summary>
        protected override void RecProcess()
        {
            string preamb = "board side. RecProcess. ";
            UInt32 fsize = 0;
            if (come0.OPC == TfBase.OPC_RRQ)
            {
                from0.OPC = come0.OPC;
                from0.ID = come0.ID;

                SendMachine.reqFile = come0.ID;

                if (from0.ID == TfBase.ID_ECHO) { }
                else if (from0.ID == TfBase.ID_INFO) { fsize = (UInt32)testInfoString.Length; }
                else if (from0.ID == TfBase.ID_FILENOTES) { fsize = (UInt32)oufile.Lenght(); }
                else
                {
                    from0.OPC = TfBase.OPC_ERR;
                    from0.ID = 5;
                    SendMachine.reqFile = TfBase.ID_NONE;
                }

                from0.loadFSizeData((Int32)fsize);


                /* master side want read some info */
                if (fsize == 0)
                {
                    SendMachine.st_Transfer = TransState.IDLE;
                    SendMachine.reqFile = TfBase.ID_NONE;
                }
                else
                {
                    SendMachine.st_Transfer = TransState.SEND_DATA;
                    SendMachine.reqFile = come0.ID;
                }

                m_netraw.handleOutStream(from0.getBytes(6));

                bchief.reloadBlocks();
            }
            else if (come0.OPC == TfBase.OPC_WRQ)
            {
                SendMachine.st_Transfer = TransState.WAIT_DATA;
                bchief.reloadBlocks();
            }
            else if (come0.OPC == TfBase.OPC_ACK)
            {
                /* sending data acked */
                debugcc.dbgInfo(preamb + "OPC ACK. ID: " + come0.ID);
                bchief.blockCheck(come0.ID);
                if (bchief.IsLastBlock())
                {
                    debugcc.dbgInfo("Last ACK was got. Stop machine");
                    sendtimeout.Stop();
                    SendMachine.ToIdle();
                }
            }
            else if (come0.OPC == TfBase.OPC_DATA &&
              SendMachine.st_Transfer == TransState.WAIT_DATA)
            {
                int ret = come0.DATA.Length;

                if (!bchief.blockCheck(come0.ID))
                {
                    debugcc.dbgWarn("receive data: incorrect ID. ACK wrong = " + come0.ID + ". Wait = "
                      + (bchief.bidack + 1));
                }
                else
                {

                    debugcc.dbgInfo("receive data: Correct ID. ACK block: id="
                      + come0.ID + "; size=" + ret);

                    if (ret == 0)
                    {
                        debugcc.dbgInfo("receive data: Finish receiving");
                        SendMachine.ToIdle();
                        waittimeout.Stop();
                    }
                }
                come0.headAckUp(come0.ID);
                m_netraw.handleOutStream(come0.getBytes(0));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private int SendAcknowledge()
        {
            int ret = 0;
            //tfblock.headUp(TfBase.OPC_ACK, SendMachine.reqFile);
            SendMachine.st_Transfer = TransState.IDLE;


            if (SendMachine.reqFile == TfBase.ID_INFO)
            {
                //debugcc.dbgInfo("Send Info");
                //Encoding ec1 = Encoding.UTF8;
                //tfblock.body_data = ec1.GetBytes("IMEI=2323476283462834,VERSION=1.0.0,NAME=KuKU");
                //ret = tfblock.body_data.Length;
                come0.headUp(TfBase.OPC_ERR, 1);

            }

            else if (SendMachine.reqFile == TfBase.ID_ECHO)
            {
                debugcc.dbgInfo("Send Echo");
            }

            else if (SendMachine.reqFile == TfBase.ID_FILENOTES)
            {
                debugcc.dbgInfo("Send FileNotes ACK");

                Int32 size = oufile.Lenght();
                come0.DATA = BitConverter.GetBytes(size);
                SendMachine.st_Transfer = TransState.SEND_DATA;
                ret = come0.DATA.Length;
            }

            else { }

            return ret;
        }












        public override void startG2Client(System.Net.Sockets.TcpClient insocket, int id)
        {
            //echotimer.Start();
            //clThr.Start();
            sendtimeout.Interval = 5000;
            //waitdata_to.Start();

            sendtimeout.Elapsed += SendDataTimeOut;
            base.startG2Client(insocket, id);
        }

        private void SendDataTimeOut(Object source, System.Timers.ElapsedEventArgs e)
        {
            debugcc.dbgWarn(" Data sending timeout : left attempts = " + bchief.resend_cnt);
            bchief.RollBackBidSend();

            if (bchief.resend_cnt-- == 0)
            {
                SendMachine.ToIdle(); sendtimeout.Stop();
            }
            else
            {

            }

        }


        private void waittimeout_Elapsed(Object source, System.Timers.ElapsedEventArgs e)
        {
            debugcc.dbgWarn(" Data waiting timeout");
            SendMachine.ToIdle();
            waittimeout.Stop();
        }

        private void reloadWaitTimeout(UInt16 val = 9000)
        {
            waittimeout.Interval = val;
        }

    }
}
