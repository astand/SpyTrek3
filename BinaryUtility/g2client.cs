using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;


namespace ProtSys
{
    /// <summary>
    /// 
    /// </summary>
    internal static class OpcodesNames
    {
        static string[] names = { "NULL","RRQ", "WRQ", "DATA", "ACK", "ERROR" };
        internal static string Print(UInt16 id)
        {
            if (id < names.Length)
                return names[id];
            return names[0];
        }
    }


    public enum TransState
    {
        WAIT_ACK, WAIT_DATA, IDLE, SEND_DATA, SEND_ACK
    };

    public static class TfBase
    {

        private static UInt16 DATA_OUT
        {
            get { return ((UInt16)1 << 14); }
        }

        /// <summary>
        /// Opcodes collection
        /// </summary>
        public const UInt16 OPC_RRQ =   (1);
        public const UInt16 OPC_WRQ =   (2);
        public const UInt16 OPC_DATA =  (3);
        public const UInt16 OPC_ACK =   (4);
        public const UInt16 OPC_ERR =   (5);


        //public static UInt16 OPC_DATA_ACK { get { return (10); } }


        /// <summary>
        /// ID collection for trek handle
        /// </summary>
        public const UInt16 ID_FILENOTES = (1);
        public const UInt16 ID_TRACK = (2);
        public static UInt16 ID_FIRMWARE { get { return (UInt16)(3 | DATA_OUT); } }

        /// <summary>
        /// ID collection for non DATA request
        /// </summary>
        public const UInt16 ID_INFO = (4);
        public const UInt16 ID_ECHO = (128);

        /// <summary>
        /// ID cmd that indicate main proccess that no data to send
        /// </summary>
        public const UInt16 ID_NONE = ((UInt16)0);


        private static bool IsDataOut(UInt16 id) { return ((id & DATA_OUT) == DATA_OUT); }


        internal static UInt16 getOpcByFileId(UInt16 id)
        {
            return (IsDataOut(id)) ? (OPC_WRQ) : (OPC_RRQ);
        }
    }








    /// <summary>
    /// class that contains two collections of state 
    /// 1 - for transfer state indication
    /// 2 - for file request state indication
    /// </summary>
    public class g2flags
    {
        internal TransState st_Transfer;
        internal UInt16 reqFile;

        internal g2flags()
        {
            ToIdle();
        }

        public void SetStFile(UInt16 v) { reqFile = v; }
        public UInt16 GetStFile() { return reqFile; }

        /// <summary>
        /// Drop flags to default state 
        /// </summary>
        internal void ToIdle()
        {
            reqFile = TfBase.ID_NONE;
            st_Transfer = TransState.IDLE;
        }

        internal bool IsNotIdle()
        {
            return (reqFile != TfBase.ID_NONE || st_Transfer != TransState.IDLE);
        }

    }











    /// <summary>
    /// handler for tcp stream
    /// </summary>
    public sealed class SocketGate : IDisposable
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
        public SocketGate(TcpClient cl)
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
        /// <param name="inb">Data buffer for withdraw data from network
        /// stream</param>
        /// <returns></returns>
        private int readInStream(byte[] inb)
        {
            int retval = 0;

            if (isDead()) return 0;

            // when trying read Property of disposed obj - > exception
            //if (m_cleint.Client.RemoteEndPoint == null)
            ////if (G2_Client.Client.Connected == false)
            //{
            //  debugcc.dbgWarn(" client not connected detected\n");
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool isDead() { return netstate == ENetState.DISPOSED; }



        /// <summary>
        /// If it can be write full payload writes to outstream
        /// </summary>
        /// <param name="obuf"></param>
        /// <returns></returns>
        public int handleOutStream(byte[] obuf)
        {
            return handleOutStream(obuf, obuf.Length);
        }

        /* asddsf */

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





    public class g2route
    {

        static UInt16 RESEND_COUNT { get { return (2); } }
        static UInt16 BIDWINDOWS { get { return (8); } }
        /// <summary>
        /// order number of current send block. Can't be more then 
        /// @bidack + @bidwindow
        /// </summary>
        public UInt16 bidsend;


        /*
         * @bidack - number of last acknowledged block
         * */
        public UInt16 bidack;


        /*
         * @bidlast - number of last block
         * */
        public UInt16 bidlast;


        /*
         * @resend_cnt - count of resending action 
         * */
        public UInt16 resend_cnt;


        /*
         * @bidwindow - size of gap beetwen @bidsend and @bidack  
         * expirience shows that value = 4 best
         * */
        public UInt16 bidwindow;




        internal bool ISLASTFREEZE
        {
            get { return (bidlast == bidsend); }
        }

        /* ------------------------------------------------------------------- */
        public g2route()
        {
            bidsend = 1;
            bidwindow = BIDWINDOWS;
            reloadResendCounter();
        }


        public g2route(UInt16 id) { bidack = id; }

        bool isBlockValid(UInt16 id, int windsize) { return (bidack == (id - windsize)); }

        public void reloadBlocks()
        {
            bidack = 0;
            bidsend = 1;
            bidlast = 0;
            reloadResendCounter();
        }

        internal void reloadResendCounter() { resend_cnt = RESEND_COUNT; }

        private void FixLastBid() { bidlast = bidsend; }

        internal void FixLastBid(Int32 val)
        {
            if (val > 0)
                bidsend++;
            else
                FixLastBid();
        }

        internal bool RollBackBidSend()
        {
            if (resend_cnt == 0) { return false; }
            else { resend_cnt--; }
            bidlast = 0;
            bidsend = (UInt16)(bidack + 1);
            return true;
        }


        //internal bool IsLastFreeze() { return (bidlast == bidsend);}

        public bool IsBlockSendPermitted()
        {
            return ((bidsend <= (bidack + bidwindow)) && !ISLASTFREEZE);
        }



        public bool blockCheck(UInt16 id)
        {
            if (isBlockValid(id, 1))
            {
                bidack += 1;
                return true;
            }
            return false;
        }


        public bool blockCheck(UInt16 id, UInt16 windsize)
        {
            if (isBlockValid(id, windsize))
            {
                bidack += windsize;
                return true;
            }
            return false;
        }

        internal bool IsLastBlock()
        {
            return bidack == bidlast;
        }



    };






    /* 
    * class provides working TFTP2 protocol
    * Full tftp2 packet consist of 2byte opc 2byte bid and array
    * of data.
    * 
    * 
    */
    public class GTftp2
    {
        private const int MAX_DATA_LEN = 1416;
        private const int MIN_BLOCK_LEN = 4;
        //private const int MAX_DATA_LEN_CRC = (MAX_DATA_LEN + 2);

        //public GTfpData stuff = new GTfpData();


        private UInt16 _body_opc;
        private UInt16 _body_id;
        public byte[] _body_data;
        /// <summary>
        /// 
        /// </summary>
        internal UInt16 OPC
        {
            get { return (UInt16)(_body_opc); }
            set { _body_opc = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal UInt16 ID
        {
            get { return (UInt16)(_body_id); }
            set { _body_id = value; }
        }

        internal byte[] DATA
        {
            get { return _body_data; }
            set { _body_data = value; }
        }

        /// <summary>
        /// 0 - in direction, 1 - out direction
        /// </summary>
        private Int32 g_dataDir;

        private Int32 dataDirection
        {
            get { return g_dataDir; }
            set { g_dataDir = value; }
        }


        internal bool IsDirectionIn() { return (dataDirection == 0); }

        //internal void DirectionIn() { dataDirection = 0; }
        //internal void DirectionOut() { dataDirection = 1; }


        public GTftp2() { DATA = new byte[0]; }

        private int CheckLen(int len = 0)
        {
            return ((len > MAX_DATA_LEN) ? (MAX_DATA_LEN) :
              (len < MIN_BLOCK_LEN ? (MIN_BLOCK_LEN) : (len)));
        }




        public GTftp2(int len)
        {
            len = CheckLen(len);
            DATA = new byte[len - 4];
        }


        private void setOpc(UInt16 o) { OPC = o; }
        private void setBlockId(UInt16 i) { ID = i; }

        private int setData(byte[] b) { return setData(b, b.Length); }
        private int setData(byte[] b, int blen)
        {
            int i;
            if (b == null || b.Length == 0) return 0;
            blen = CheckLen(blen);
            for (i = 0; i < blen; DATA[i] = b[i], i++) ;
            return i;
        }
        public byte[] loadBlock(UInt16 o = 0, UInt16 i = 0, byte[] b = null, int len = 0)
        {
            setOpc(o); setBlockId(i); setData(b, len);
            return GetByte(len);
        }

        public void headUp(GTftp2 src)
        {
            this.headUp(src.OPC, src.ID);
        }

        public void headUp(UInt16 id)
        {
            OPC = TfBase.getOpcByFileId(id);
            dataDirection = (OPC == TfBase.OPC_WRQ) ? (1) : (0);
            ID = id;
        }

        public void headUp(UInt16 opcode, UInt16 id)
        {
            OPC = opcode;
            ID = id;
        }

        public int dataUp(byte[] b)
        {
            int i;
            int len = CheckLen(b.Length);
            for (i = 0; i < len; DATA[i] = b[i], i++) ;
            return i;
        }


        public void headDataUp(UInt16 opcode, UInt16 id, byte[] b)
        {
            headUp(opcode, id);
            dataUp(b);
        }

        public void headAckUp(UInt16 id)
        {
            headUp(TfBase.OPC_ACK, id);
        }

        //public void headDataAckUp(UInt16 id)
        //{
        //  headUp(TfBase.OPC_DATA_ACK, id);
        //}

        //public void headDataUp(UInt16 opcode, UInt16 id, byte[] b)
        //{
        //  headUp(opcode, id);
        //  dataUp(b);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indx"></param>
        /// <returns></returns>
        internal Int32 LoadTrackIndex(Int32 indx)
        {
            headUp(TfBase.ID_TRACK);
            byte[] dat = BitConverter.GetBytes((UInt16)(indx));
            Array.Copy(dat, 0, DATA, 0, 2);
            return dat.Length;
        }


        internal int loadFSizeData(Int32 size)
        {

            byte[] temp = new byte[4];
            temp = BitConverter.GetBytes((UInt16)0x0401);
            Array.Copy(temp, 0, DATA, 0, 2);
            temp = BitConverter.GetBytes((UInt32)size);
            Array.Copy(temp, 0, DATA, 2, 4);
            return 0;
        }

        internal UInt32 getFSizeFromData()
        {
            return (DATA.Length > 5) ? (BitConverter.ToUInt32(DATA, 2)) : (0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        internal bool IsEqual(GTftp2 ob)
        {
            return ((ID == ob.ID) && (OPC == ob.OPC));
        }


        public byte[] GetByte(int size = -1)
        {
            dynByte dyn = new dynByte();

            dyn.AddByteArray(BitConverter.GetBytes(OPC));
            dyn.AddByteArray(BitConverter.GetBytes(ID));
            dyn.AddByteArray(DATA, size);
            return dyn.GetHonest();
        }
        public byte[] GetRawCrcByte(int size = -1)
        {
            dynByte dyn = new dynByte();
            /* get clear byte expression */
            dyn.AddByteArray(GetByte(size));
            dyn.SetCrc16();

            return dyn.GetHonest();
        }
        static public GTftp2 Gtftp2FromArray(byte[] ibuf, int blen = -1)
        {
            if (blen < 0) blen = ibuf.Length;

            if (blen < 4)
                return new GTftp2();

            GTftp2 outinst = new GTftp2(blen);
            outinst.ID = BitConverter.ToUInt16(ibuf, 2);
            outinst.OPC = BitConverter.ToUInt16(ibuf, 0);
            /* copy memory without 2 crc bytes */
            System.Array.Copy(ibuf, 4, outinst.DATA, 0, outinst.DATA.Length);
            return outinst;
        }

        /// <summary>
        /// Return byte[] data from dedicated GTftp2 block
        /// </summary>
        /// <param name="o">Opcode</param>
        /// <param name="i">id</param>
        /// <param name="b">data buffer</param>
        /// <param name="len">data lenght</param>
        /// <returns></returns>
        public byte[] getReadyBuff(UInt16 o = 0, UInt16 i = 0, byte[] b = null, int len = 0)
        {
            loadBlock(o, i, b, len);
            return ProtBinUtility.byteStuff(GetRawCrcByte(len));
        }

        public byte[] getBytes()
        {
            loadBlock(OPC, ID, DATA, DATA.Length);
            return ProtBinUtility.byteStuff(GetRawCrcByte(DATA.Length));
        }

        public byte[] getBytes(Int32 bodydatasize)
        {
            loadBlock(OPC, ID, DATA, bodydatasize);
            return ProtBinUtility.byteStuff(GetRawCrcByte(bodydatasize));
        }

        public override string ToString()
        {
            string s = String.Format("{0,6}:{1:X4}{3,8}[{2:D5}]",
        OpcodesNames.Print(OPC), ID, DATA.Length," ");
            return s;
        }
    }








    public class G2_Client : IDisposable
    {

        /// <summary>
        /// Time that determines period for main client thread sleep
        /// </summary>
        private Int32 TIME_FOR_TREAD_SLEEPING = 20;

        internal UInt32 G2_OUT_DATA_LEN { get { return 1000; } }


        /// <summary>
        /// static value indicate count of active sockets
        /// </summary>
        static int sockcount = 0;

        /// <summary>
        /// main net stream handler
        /// </summary>
        protected SocketGate m_netraw;

        /// <summary>
        /// buff for save molding full unpacking block
        /// and use it next
        /// </summary>
        byte[] unpackbuffer = new byte[1024];

        protected ResultOfReceiveProcess receivestatus = new ResultOfReceiveProcess();

        protected SpyTrekInfo spInfo = new SpyTrekInfo();

        protected Action<SpyTrekInfo> updateInfo = new Action<SpyTrekInfo>(obj => { });
        protected Action<string> updateStatus = new Action<string>(obj => { });
        protected Action<string> updateList = new Action<string>(obj => { });
        protected Action<MatrixItem> updateGrid = new Action<MatrixItem>(obj => { });

        //protected MatrixItem[] flist = new MatrixItem[MATRX_LENGHT];
        /// <summary>
        /// FullTrekCollection
        /// </summary>
        internal MICollection alltreks = new MICollection();

        /// <summary>
        /// instance for Transaction Control
        /// </summary>
        public g2flags SendMachine = new g2flags();

        /// <summary>
        /// 
        /// </summary>
        protected int curentindex;


        /// <summary>
        /// for keeping data in GTftp format
        /// </summary>
        public GTftp2 come0 = new GTftp2();


        internal GTftp2 from0 = new GTftp2(1020);
        /// <summary>
        /// string for save version and imei
        /// </summary>
        //string n_imei, n_ver;


        /// <summary>
        /// value of actual file size
        /// </summary>
        protected UInt32 commonFSize;

        /// <summary>
        /// collect of ID states for control transfering machine
        /// </summary>
        protected g2route bchief = new g2route();

        public void UpdateInfoDelegate(Action<SpyTrekInfo> d) { updateInfo = d; }
        public void UpdateStatusDelegate(Action<string> d) { updateStatus = d; }
        public void UpdateListDelegate(Action<string> d) { updateList = d; }
        public void UpdateGrid(Action<MatrixItem> d) { updateGrid = d; }

        /// <summary>
        /// IDisposable implementation
        /// not need implement Finalizer, but must be calls Dispose necessarily
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            debugcc.dbgInfo(" Dispose G2Client. Instance: [" + curentindex.ToString() + "] ");
        }

        /// <summary>
        /// Calls from Dispose() with @true, from Finalizer with @false
        /// when Dispose() -> release all unmanaged resources
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (m_netraw != null)
                {
                    m_netraw.Dispose();
                    m_netraw = null;
                }
            }
        }

        public G2_Client() { }

        public G2_Client(TcpClient cl, int i)
        {
            m_netraw = new SocketGate(cl);
            curentindex = i;
        }


        public virtual void startG2Client(TcpClient insocket, int id)
        {
            m_netraw = new SocketGate(insocket);
            curentindex = id;

            sockcount++;
            curentindex = sockcount;

            debugcc.dbgInfo("Income connection: " + insocket.ToString() + ". ID = " + curentindex.ToString());
            debugcc.dbgDebug("recbuffesize: " + insocket.ReceiveBufferSize.ToString());
            debugcc.dbgDebug("client describe: " + insocket.Client.RemoteEndPoint);
        }

        /// <summary>
        /// Check both transfer state and opc in incoming message
        /// </summary>
        /// <param name="opc">@opc in incoming message</param>
        /// <returns>true if wait ack state</returns>
        protected bool IsWaitAckTrue(UInt16 opc)
        {
            return (SendMachine.st_Transfer == TransState.WAIT_ACK) &&
              (opc == TfBase.OPC_RRQ);
        }


        protected bool IsSendDataTrue(UInt16 opc)
        {
            return (SendMachine.st_Transfer == TransState.SEND_DATA) &&
              (opc == TfBase.OPC_ACK);
        }

        /// <summary>
        /// Check both transfer state and opc in incoming message
        /// </summary>
        /// <param name="opc">@opc in incoming message</param>
        /// <returns>true if wait data state</returns>
        protected bool IsWaitDataTrue(UInt16 opc)
        {
            return (SendMachine.st_Transfer == TransState.WAIT_DATA) &&
                    (opc == TfBase.OPC_DATA);
        }


        /// <summary>
        /// 
        /// </summary>
        public void proccessMain()
        {
            int wasread = 0;
            while (true)
            {
                if (m_netraw.isDead())
                {
                    //sockcount--;
                    //if (sockcount > 100)
                    {
                        debugcc.dbgDebug(" mainproccess. Stream is dead close thread\n");
                        updateStatus("Потрачено");
                        return;
                    }
                }

                else
                {
                    wasread = UnpackBlockCut();
                    mainDataHandler(wasread);
                }

                Thread.Sleep(TIME_FOR_TREAD_SLEEPING);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int UnpackBlockCut()
        {
            int ret = 0;

            m_netraw.handleInStream();
            ret = m_netraw.stripFifoMsg(unpackbuffer);
            if (ret > 0) { debugcc.dbgTrace("strip success: " + ret.ToString() + "\n"); }
            return ret;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lenrx"></param>
        protected virtual void mainDataHandler(int lenrx) { }


        /// <summary>
        /// 
        /// </summary>
        protected virtual void ReqProcess() { }



        /// <summary>
        /// call only when UnpackCreateInstance return 0. It operates with
        /// inner tftp2 block and routine all data handling
        /// </summary>
        protected virtual void RecProcess() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blocklen"></param>
        /// <returns></returns>
        protected virtual int UnpackReceive(int blocklen)
        {
            if (blocklen < 4)
                return -1;

            come0 = GTftp2.Gtftp2FromArray(unpackbuffer, blocklen);
            return 0;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void mainTimerEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            SendMachine.SetStFile(TfBase.ID_ECHO);
        }

        //public void Dispose(bool v)
        //{
        //  Dispose();
        //}




        /// <summary>
        /// 
        /// </summary>
        //private void CloseClient()
        //{
        //  debugcc.dbgInfo("[" + curentindex.ToString() + "] close client\n");
        //}
    }




}
