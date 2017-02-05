using MessageHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamHandler.Abstract;
using StreamHandler;
using MessageHandler.DataUploading;
using System.Timers;
using System.Diagnostics;
using MessageHandler.Processors;

namespace MessageHandler
{

    /// <summary>
    /// Processor for handling write operations (in this case write fitmware to module)
    /// </summary>
    public class FirmwareProcessor : IFrameProccesor, IDisposable
    {
        static private readonly Int32 BLOCK_SIZE = 1000;

        private BlockDriver blockDriver = new BlockDriver();

        private Piper piper;

        private IDataUploader dataUploader;

        private bool m_request_sent = false;

        private Boolean m_data_active = false;

        private Timer sendTimer = new Timer();

        Byte[] m_payload = new byte[BLOCK_SIZE];

        public FirmwareProcessor(Piper pipe, string source_path)
        {
            piper = pipe;
            //dataUploader = new CachedFileUploader(source_path);
            dataUploader = new DiskFileUploader(source_path);
            dataUploader.RefreshData();
            Debug.WriteLine($"Firmware processor initialized by file {dataUploader.ToString()} length = {dataUploader.Length}");

            sendTimer.Elapsed += SendTimer_Elapsed;
        }

        public void SendRequest()
        {
            piper.SendData(new WriteRequest(0x4003, dataUploader.Length));
            StartSending();
        }

        /// <summary>
        ///  will never return answer here
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="answer"></param>
        public override void Process(FramePacket packet, ref IStreamData answer)
        {
            lock (blockDriver)
            {
                State = ProcState.Idle;

                if (packet.Opc == OpCodes.ACK)
                {
                    State = ProcState.Data;
                    blockDriver.PassAckBlock(packet.Id);

                    if (blockDriver.IsLastAck)
                    {
                        State = ProcState.Finished;
                        StopSending();
                        Console.WriteLine("Stop sending action.");
                    }
                }

                /// reuest acknowledge handle here
                if (packet.Opc == OpCodes.WRQ)
                {
                    State = ProcState.CmdAck;
                    m_data_active = true;
                }
            }
        }

        public void ScheduleSendingData()
        {
            lock (blockDriver)
            {
                if (m_request_sent == false || m_data_active == false)
                {
                    return;
                }

                while (blockDriver.IsWindAllow())
                {
                    var readed = ReadDataChuck();

                    var fPacket = new FramePacket(OpCodes.DATA, blockDriver.BidSend, m_payload, readed);

                    piper.SendData(fPacket);

                    if (readed == 0)
                    {
                        // last block sent
                        blockDriver.BidLast = blockDriver.BidSend;
                    }
                    else
                        blockDriver.BidSend++;
                }
            }
        }

        public void Dispose()
        {
            sendTimer.Dispose();
        }

        private Int32 ReadDataChuck()
        {
            Int32 offset = (blockDriver.BidSend - 1) * BLOCK_SIZE;

            return dataUploader.ReadData(m_payload, offset, BLOCK_SIZE);
        }

        private void StartSending()
        {
            sendTimer.Interval = 50;
            m_request_sent = true;
            sendTimer.Start();
            blockDriver.Reset();
        }

        private void StopSending()
        {
            sendTimer.Stop();
            m_request_sent = false;
            m_data_active = false;
        }


        private void SendTimer_Elapsed(Object sender, ElapsedEventArgs e)
        {
            ScheduleSendingData();
        }
    }
}
