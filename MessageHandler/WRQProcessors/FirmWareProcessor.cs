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

namespace MessageHandler
{

    /// <summary>
    /// Processor for handling write operations (in this case write fitmware to module)
    /// </summary>
    public class FirmwareProcessor : IFrameProccesor, IDisposable
    {
        static private readonly Int32 BLOCK_SIZE = 1000;

        private BlockDriver m_blockDriver = new BlockDriver();

        private Piper m_pipe;

        private IDataUploader dataUploader;

        private bool m_request_sent = false;

        private Boolean m_data_active = false;

        private Timer m_timer = new Timer();

        Byte[] m_payload = new byte[BLOCK_SIZE];

        public FirmwareProcessor(Piper pipe, string source_path)
        {
            m_pipe = pipe;
            //dataUploader = new CachedFileUploader(source_path);
            dataUploader = new DiskFileUploader(source_path);
            dataUploader.RefreshData();
            Debug.WriteLine($"Firmware processor initialized by file {dataUploader.ToString()} length = {dataUploader.Length}");

            m_timer.Elapsed += M_timer_Elapsed;
        }

        public void SendRequest()
        {
            m_pipe.SendData(new WriteRequest(0x4003, dataUploader.Length));
            StartSending();
        }

        /// <summary>
        ///  will never return answer here
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="answer"></param>
        public void Process(FramePacket packet, ref IStreamData answer, out Processors.ProcState state)
        {
            lock (m_blockDriver)
            {
                state = Processors.ProcState.Idle;

                if (packet.Opc == OpCodes.ACK)
                {
                    state = Processors.ProcState.Data;
                    m_blockDriver.PassAckBlock(packet.Id);

                    if (m_blockDriver.IsLastAck)
                    {
                        state = Processors.ProcState.Finished;
                        StopSending();
                        Console.WriteLine("Stop sending action.");
                    }
                }

                /// reuest acknowledge handle here
                if (packet.Opc == OpCodes.WRQ)
                {
                    state = Processors.ProcState.CmdAck;
                    m_data_active = true;
                }
            }
        }

        public void ScheduleSendingData()
        {
            lock (m_blockDriver)
            {
                if (m_request_sent == false || m_data_active == false)
                {
                    return;
                }

                while (m_blockDriver.IsWindAllow())
                {
                    var readed = ReadDataChuck();

                    var fPacket = new FramePacket(OpCodes.DATA, m_blockDriver.BidSend, m_payload, readed);

                    m_pipe.SendData(fPacket);

                    if (readed == 0)
                    {
                        // last block sent
                        m_blockDriver.BidLast = m_blockDriver.BidSend;
                    }
                    else
                        m_blockDriver.BidSend++;
                }
            }
        }

        public void Dispose()
        {
            m_timer.Dispose();
        }

        private Int32 ReadDataChuck()
        {
            Int32 offset = (m_blockDriver.BidSend - 1) * BLOCK_SIZE;

            return dataUploader.ReadData(m_payload, offset, BLOCK_SIZE);
        }

        private void StartSending()
        {
            m_timer.Interval = 50;
            m_request_sent = true;
            m_timer.Start();
            m_blockDriver.Reset();
        }

        private void StopSending()
        {
            m_timer.Stop();
            m_request_sent = false;
            m_data_active = false;
        }


        private void M_timer_Elapsed(Object sender, ElapsedEventArgs e)
        {
            ScheduleSendingData();
        }

    }
}
