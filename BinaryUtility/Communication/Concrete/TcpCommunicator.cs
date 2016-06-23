using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtSys.Communication.Abstract;
using System.Net.Sockets;

namespace ProtSys.Communication.Concrete
{
    class TcpCommunicator : ICommunication
    {
        TcpClient m_cleint;
        NetworkStream m_stream;


        /// <summary>
        /// state of SocketGate. Used for determination of whether connection alive or not
        /// </summary>
        enum ENetState { ACTIVE, DISPOSED };

        /// <summary>
        /// G2 client state
        /// </summary>
        ENetState netstate;


        /// <summary>
        /// Ring buffer for incoming data
        /// </summary>
        private fifoCommon g2fifo;


        private UnpackBlock unpack = new UnpackBlock();


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            debugcc.dbgInfo("Dispose SocketGate action");
        }


        ///
        /// <summary>
        ///
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (m_cleint != null)
                {
                    m_cleint.Close();
                    m_cleint = null;
                }
                if (m_stream != null)
                {
                    m_stream.Dispose();
                    m_stream = null;
                }
            }

        }

        /// <summary>
        /// Constructor for instance with parametrs
        /// </summary>
        /// <param name="fifsize">parametr for determine what size for
        /// input fifo object will be use (must be in config file)</param>
        public TcpCommunicator(TcpClient cl)
        {
            m_cleint = cl;
            m_stream = cl.GetStream();


            int size = 4096;
            g2fifo = new fifoCommon(size);
            netstate = ENetState.ACTIVE;
        }

        internal void MarkAsDead() { netstate = ENetState.DISPOSED; }

        /// <summary>
        /// Read stream. Load payload to buffer if data availible
        /// </summary>
        ///
        /// <param name="itonb">Data buffer for withdraw data from network stream</param>
        /// <returns></returns>
        /// 
        private int readInStream(byte[] inb)
        {
            int retval = 0;

            if (isDead()) return 0;

            /* when trying read Property of disposed obj - > exception */
            //if (m_cleint.Client.RemoteEndPoint == null)
            ////if (G2_Client.Client.Connected == false)
            //{
            //    debugcc.dbgWarn(" client not connected detected\n");
            //}


            try
            {
                if (m_stream.DataAvailable)
                    retval = m_stream.Read(inb, 0, inb.Length);
            }
            catch (Exception e)
            {
                netstate = ENetState.DISPOSED;
                debugcc.dbgWarn(" Exception during READ:\n" + e.ToString());
            }

            return retval;
        }


        /// <summary>
        /// put payload to FIFO
        /// </summary>
        /// <param name="inb">buffer with payload</param>
        /// <param name="size">size of payload data in incoming buffer</param>
        /// <returns>
        /// if @size > 0 then fifo try put alldata to inner buf and return count of
        /// located data
        /// </returns>
        private int putReadingToFifo(byte[] inb, int size)
        {
            if (size > 0)
                return g2fifo.item(inb, size);
            return size;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public int stripFifoMsg(byte[] b)
        {
            return unpack.stripMain(g2fifo, b);
        }
        /// <summary>
        /// Read stream, if data rceived put it in fifo
        /// </summary>
        /// <returns>count of putted data</returns>
        public int handleInStream()
        {
            byte[] transactionbuff = new byte[512];
            int bytescount = readInStream(transactionbuff);
            return putReadingToFifo(transactionbuff, bytescount);
        }

        public bool isDead() => (netstate == ENetState.DISPOSED);



        /// <summary>
        /// If it can be write full payload writes to outstream
        /// </summary>
        /// <param name="obuf"></param>
        /// <returns></returns>
        public int handleOutStream(byte[] obuf)
        {
            return handleOutStream(obuf, obuf.Length);
        }



        /// <summary>
        /// If it can be write payload writes to outstream
        /// </summary>
        /// <param name="obuf">contains paylod</param>
        /// <param name="size">lenght of pay load</param>
        /// <returns></returns>
        public int handleOutStream(byte[] obuf, int size)
        {
            if (isDead()) return 0;

            try
            {
                m_stream.Write(obuf, 0, size);
            }
            catch (Exception ex)
            {
                netstate = ENetState.DISPOSED;
                debugcc.dbgWarn(" Exception during WRITE: \n" + ex.Message);
                return -1;
            }

            return size;
        }

    };


}
