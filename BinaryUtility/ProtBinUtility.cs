using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.IO;



namespace ProtSys
{
    struct OPCODE
    {
        public const byte RRQ = 1;
        public const byte WRQ = 2;
        public const byte DATA = 3;
        public const byte ACK = 4;
        public const byte ERROR = 5;
        public const byte OPACK = 6;
    };

    static public class  ProtBinUtility
    {
        enum ReceiveMachine 
        { 
            Begin,
            Esc,
            Msg, 
            Final 
        };

        const byte EDGE_B = 0xc0;
        const byte ESC = 0xDB;
        const byte ESC_C = 0xDC;
        const byte ESC_D = 0xDD;

        const int SIZE_RX_BUFF = 1500;

        static ReceiveMachine BinState = ReceiveMachine.Begin;
        
        static int byteCount = 0;
        
        //static byte stuffing = 0;

        public static byte[] gsmRxBuff = new byte[SIZE_RX_BUFF];
        static byte[] picRAM;
        
        static uint picSize = 0;
        static ushort blkSize = 0;
        static short blkId = 0;
        static uint calcPicSize = 0;

        public static string imageName;
        /* --------------------------------------- *
         * DataGramm in Data block
         * --------------------------------------- */
        public static DProt ProtMessageBuild(byte dstAddr, byte protCmd, byte[] dataGramm, int dataLen,short tCode = 1, short tID = 0)
        {
            DProt outB = new DProt(dataLen);

            outB.SetHead(dstAddr, protCmd);
            outB.SetTf(tCode, tID);
            outB.SetData(dataGramm, dataLen);
            outB.SetCrc();

            return outB;
        }
        /* --------------------------------------- *
         * empty data buffer (only CRC 2bytes 
         * --------------------------------------- */
        public static DProt ProtMessageBuild(byte dstAddr, byte protCmd, short  tfCode = 1, short tfSize = 0)
        {
            DProt victim = new DProt(0);

            victim.SetHead(dstAddr, protCmd);
            victim.SetTf(tfCode, tfSize);
            victim.SetCrc();

            return victim;
        }

        public static int ProtMessageSync(byte[] outBuff)
        {
            outBuff[0] = 0xC0;
            outBuff[1] = 0x00; 
            outBuff[2] = 0x80;
            outBuff[3] = 0x00;
            outBuff[4] = 0x10;
            outBuff[5] = 0xC0;
            return 6;
        }

        static public int ProtSendSync(NetworkStream tube)
        {
            byte[] sync = new byte[] {0xc0, 0x00, 0x80, 0x00, 0x10, 0xc0};
            tube.Write(sync, 0, sync.Length);
            return 0;
        }

        static int ProtByteMixAction(byte[] inBuff, byte[] outBuff, int inLen)
        {
            int indx, indx_out = 0;

            outBuff[indx_out++] = 0xC0;

            for (indx = 0; indx < inLen; indx++)
            {
                if (inBuff[indx] == 0xC0)
                {
                    outBuff[indx_out++] = 0xDB;
                    outBuff[indx_out++] = 0xDC;
                }
                else if (inBuff[indx] == 0xDB)
                {
                    outBuff[indx_out++] = 0xDB;
                    outBuff[indx_out++] = 0xDD;
                }
                else
                {
                    outBuff[indx_out++] = inBuff[indx];
                }
            }
            outBuff[indx_out++] = 0xC0;

            return indx_out;
        }

        static public void ProtSendBlock(DProt pureProt, NetworkStream rawstream)
        {
            byte[] rawProt = pureProt.GetRawByte();
            rawstream.Write(rawProt, 0, rawProt.Length);
        }

        static public void ProtSendBlock(OProt pureProt, NetworkStream rawstream)
        {
            byte[] rawProt = pureProt.GetRawByte();
            rawstream.Write(rawProt, 0, rawProt.Length);
        }

        static public int GTftpWrite(GTftp2 inst, NetworkStream rawstream)
        {
            byte[] rawbyte = inst.GetRawCrcByte();
            rawstream.Write(rawbyte, 0, rawbyte.Length);
            return 0;
        }

        static public void ProtSendBlock(DProt purePr)
        {
            byte[] rawProt = purePr.GetRawByte();
        }

        public static void ProtSendBlock(byte[] inBuff, NetworkStream rawstream)
        {
            int indxOut = 0;

            byte[] rawByff = new byte[3000];

            rawByff[indxOut++] = 0xC0;

            for (int indx = 0; indx < inBuff.Length; indx++)
            {
                if (inBuff[indx] == 0xC0)
                {
                    rawByff[indxOut++] = 0xDB;
                    rawByff[indxOut++] = 0xDC;
                }
                else if (inBuff[indx] == 0xDB)
                {
                    rawByff[indxOut++] = 0xDB;
                    rawByff[indxOut++] = 0xDD;
                }
                else
                {
                    rawByff[indxOut++] = inBuff[indx];
                }
            }

            rawByff[indxOut++] = 0xC0;
            try
            {
                rawstream.Write(rawByff, 0, indxOut);
            }
            catch (Exception eex)
            {
                Debug.WriteLine("[socket] error write: \n" + eex.Message);
            }
        }

        public static byte[] byteStuff(byte[] inBuff)
        {
          dynByte dst = new dynByte();
          dst.AddByte(0xc0);
          for (int indx = 0; indx < inBuff.Length; indx++)
          {
            if (inBuff[indx] == 0xC0)
            {
              dst.AddByte(0xDB);
              dst.AddByte(0xDC);
            }
            else if (inBuff[indx] == 0xDB)
            {
              dst.AddByte(0xDB);
              dst.AddByte(0xDD);
            }
            else
            {
              dst.AddByte(inBuff[indx]);
            }
          }
          dst.AddByte(0xc0);
          return dst.GetHonest();
        }

        
        /* --------------------------------------------------------------------------- */
        public static int ProtReceiveMessageWide()
        {
            byte cByte;
            int res;

            while ((res = ProtFifo.GetByte()) >= 0)
            {
                cByte = (byte)res;
                switch (BinState)
                {
                case (ReceiveMachine.Begin):
                    {
                        byteCount = 0;
                        if (cByte == 0xC0)
                        {
                            //Debug.Write("=[PROTBIN]::Start mark finding");
                            BinState = ReceiveMachine.Msg;
                        }
                        break;
                    }

                case (ReceiveMachine.Esc):
                    {
                        if (cByte == ESC_C)
                        {
                            gsmRxBuff[byteCount++] = EDGE_B;
                            BinState = ReceiveMachine.Msg;
                        }
                        else if (cByte == ESC_D)
                        {
                            gsmRxBuff[byteCount++] = ESC;
                            BinState = ReceiveMachine.Msg;
                        }
                        else
                        {
                            Debug.Write("=[PROT]:Error> Unexpecting byte: " + cByte);
                            BinState = ReceiveMachine.Begin;
                        }
                        break;
                    }

                case (ReceiveMachine.Msg):
                    {
                        if (cByte == ESC)
                        {
                            /* esc symbol */
                            BinState = ReceiveMachine.Esc;
                            break;
                        }
                        else if (cByte == EDGE_B)
                        {
                            /* Edge of packet */
                            if (byteCount > 2)
                            {
                                //Debug.Write("=[PROTBIN]::Stop message <" + byteCount + ">");
                                BinState = ReceiveMachine.Begin;

                                Crc16.CalcVal(gsmRxBuff, byteCount - 2, 1);
                                if (gsmRxBuff[byteCount - 2] == Crc16.GetHiVal() && gsmRxBuff[byteCount - 1] == Crc16.GetLoVal())
                                {
                                    return byteCount - 2;
                                }
                                Console.WriteLine("=[PROT]:CRC Eror<" + gsmRxBuff[byteCount - 2] + 
                                    "><" + gsmRxBuff[byteCount - 1] +"> calc <" + Crc16.GetHiVal() + 
                                    "><" + Crc16.GetLoVal() + ">");

                                Debug.Write("=[PROTBIN]::CRC Error Crc wait <" + gsmRxBuff[byteCount - 2] + 
                                    "><" + gsmRxBuff[byteCount - 1] +"> calc <" + Crc16.GetHiVal() + 
                                    "><" + Crc16.GetLoVal() + "> bC=" + byteCount);
                            }
                            else
                            {
                                Debug.Write("=[PROTBIN]::err Start byte double");
                            }
                        }
                        else
                        {
                            gsmRxBuff[byteCount++] = cByte;
                        }

                        break;
                    }

                case (ReceiveMachine.Final):
                    {
                        BinState = ReceiveMachine.Begin;
                        break;
                    }
                default:
                    break;
                } // switch

                if (byteCount >= (SIZE_RX_BUFF-1))
                {
                    BinState = ReceiveMachine.Begin;
                }
            } //while
            return 0;
        }

        //public static int ProtReceiveMessageWide(byte[] buff, int vic)
        //{
        //  byte cByte;
        //  int res;

        //  if (vic < 0)
        //    return 0;

          
        //  cByte = (byte)vic;


        //  cByte = (byte)res;
        //  switch (BinState)
        //  {
        //    case (ReceiveMachine.Begin):
        //      {
        //        byteCount = 0;
        //        if (cByte == 0xC0)
        //        {
        //          //Debug.Write("=[PROTBIN]::Start mark finding");
        //          BinState = ReceiveMachine.Msg;
        //        }
        //        break;
        //      }

        //    case (ReceiveMachine.Esc):
        //      {
        //        if (cByte == ESC_C)
        //        {
        //          gsmRxBuff[byteCount++] = EDGE_B;
        //          BinState = ReceiveMachine.Msg;
        //        }
        //        else if (cByte == ESC_D)
        //        {
        //          gsmRxBuff[byteCount++] = ESC;
        //          BinState = ReceiveMachine.Msg;
        //        }
        //        else
        //        {
        //          Debug.Write("=[PROT]:Error> Unexpecting byte: " + cByte);
        //          BinState = ReceiveMachine.Begin;
        //        }
        //        break;
        //      }

        //    case (ReceiveMachine.Msg):
        //      {
        //        if (cByte == ESC)
        //        {
        //          /* esc symbol */
        //          BinState = ReceiveMachine.Esc;
        //          break;
        //        }
        //        else if (cByte == EDGE_B)
        //        {
        //          /* Edge of packet */
        //          if (byteCount > 2)
        //          {
        //            //Debug.Write("=[PROTBIN]::Stop message <" + byteCount + ">");
        //            BinState = ReceiveMachine.Begin;

        //            Crc16.CalcVal(gsmRxBuff, byteCount - 2, 1);
        //            if (gsmRxBuff[byteCount - 2] == Crc16.GetHiVal() && gsmRxBuff[byteCount - 1] == Crc16.GetLoVal())
        //            {
        //              return byteCount - 2;
        //            }
        //            Console.WriteLine("=[PROT]:CRC Eror<" + gsmRxBuff[byteCount - 2] +
        //                "><" + gsmRxBuff[byteCount - 1] + "> calc <" + Crc16.GetHiVal() +
        //                "><" + Crc16.GetLoVal() + ">");

        //            Debug.Write("=[PROTBIN]::CRC Error Crc wait <" + gsmRxBuff[byteCount - 2] +
        //                "><" + gsmRxBuff[byteCount - 1] + "> calc <" + Crc16.GetHiVal() +
        //                "><" + Crc16.GetLoVal() + "> bC=" + byteCount);
        //          }
        //          else
        //          {
        //            Debug.Write("=[PROTBIN]::err Start byte double");
        //          }
        //        }
        //        else
        //        {
        //          gsmRxBuff[byteCount++] = cByte;
        //        }

        //        break;
        //      }

        //    case (ReceiveMachine.Final):
        //      {
        //        BinState = ReceiveMachine.Begin;
        //        break;
        //      }
        //    default:
        //      break;
        //  } // switch

        //  if (byteCount >= (SIZE_RX_BUFF - 1))
        //  {
        //    BinState = ReceiveMachine.Begin;
        //  }
        //  return 0;
        //}
        /* --------------------------------------------------------------------------- */
        

        static public DProt GetDProtFromBuff(int len)
        {
            return (DProt.GetProt(gsmRxBuff, len));
        }

        static public byte[] GetMsg(int len)
        {
            byte[] oou = new byte[len];

            for (int i = 0; i < len; oou[i] = gsmRxBuff[i], i++) ;
            return oou; ;
        }

        static public void GetAckOptions(byte[] options, int datLen, ref uint fSize, ref ushort blkSize)
        {
            byte[] ffSize;
            byte[] bbSize;

            int startFF, sizeFF;
            int startBB, sizeBB;

            int i;
            startFF = 6;
            sizeFF = sizeBB = 0;

            for (i = 6; i < datLen; i++)
            {
                if (options[i] == 0)
                {
                    sizeFF = i - startFF;
                    break;
                }
            }

            ffSize = new byte[sizeFF];
            startBB = i += 9;
            sizeBB = 0;
            for (; i < datLen; i++)
            {
                if (options[i] == 0)
                {
                    sizeBB = i - startBB;
                }
            }

            bbSize = new byte[sizeBB];

            for (i = 0; i < sizeFF; i++)
                ffSize[i] = options[startFF + i];

            for (i = 0; i < sizeBB; i++)
                bbSize[i] = options[startBB + i];


            fSize = Convert.ToUInt32(System.Text.UTF8Encoding.UTF8.GetString(ffSize));
            blkSize = Convert.ToUInt16(System.Text.UTF8Encoding.UTF8.GetString(bbSize));
            Debug.Write("[FILE]:Options> fSize: " + fSize + ", blkSize: " + blkSize);
        }


        static public int ParseWideProt(DProt victim)
        {
            int res = 0;

            switch (victim.Head.tCode)
            {
                case (OPCODE.OPACK):
                    {
                        
                        GetAckOptions(victim.Data, victim.DataLenght,ref picSize,ref  blkSize);
                        blkId = 0;
                        calcPicSize = 0;
                        Debug.WriteLine("[WTFTP]:Ack size{0}", picSize);
                        /* create RAM for jpeg */
                        if (picSize > 0)
                            picRAM = new byte[picSize];
                        else
                            return 1;
                                             
                        break;
                    }

                case (OPCODE.ERROR):
                    {
                        Debug.WriteLine("[WTFTP]:Error> type:{0}, code:{1}", OPCODE.ERROR, victim.Head.tID);
                        return 1;
                    }
                case (OPCODE.DATA):
                    {
                        if (victim.Head.tID > blkId)
                        {
                            /* skip some blocks */
                            int skipSize = (victim.Head.tID - blkId) * blkSize;
                            byte[] OxFF = new byte[skipSize];
                            for (int i = 0; i < skipSize; picRAM[calcPicSize++] = OxFF[i++]) ;
                            
                            Debug.WriteLine("[FILE]:Error> skip frames {0},lose size {1},ID {2}", (victim.Head.tID - blkId), skipSize,blkId);
                            Console.WriteLine("[FILE]:Error> skip frames {0},lose size {1},ID {2}", (victim.Head.tID - blkId), skipSize,blkId);
                            blkId = (short)(victim.Head.tID);
                        }
                        else if (victim.Head.tID < blkId)
                        {
                            Debug.Write("[FILE]:Error> WHAT THE FUCK");
                            Console.Write("[FILE]:Error> WHAT THE FUCK");
                        }

                        /* OK packet */
                        for (int ind = 0; ind < victim.DataLenght; picRAM[calcPicSize++] = victim.Data[ind++]) ; 
                        blkId++;
                        Debug.WriteLine("[FILE]:Data> block {0} OK,size{1} wait next", victim.Head.tID, calcPicSize);

                        if (victim.DataLenght < blkSize)
                        {
                            string path;
                            DateTime curr = DateTime.Now;
                            Debug.Write("[FILE]:Data> complete, wait fSize:" + picSize);
                            string dir_path = String.Format("{0:D4}-{1:D2}-{2:D2}/", curr.Year, curr.Month, curr.Day);
                            if (!Directory.Exists(dir_path))
                            {
                                Directory.CreateDirectory(dir_path);
                            }
                            
                                                        
                            int fileIndx = 0;
                            do{
                                path = dir_path + String.Format("IMG{0:D4}.JPG", fileIndx);
                                fileIndx++;
                            } while (File.Exists(path));

                            if (picRAM != null)
                            {
                                Debug.WriteLine("[FILE]:save> name:" + path);
                                imageName = path;
                                File.WriteAllBytes(path, picRAM);
                            }
                            else
                                Debug.WriteLine("[FILE]:Error> no data\n");
                            res = 1;
                        }
                        break;
                    }


            } // switch
            return res;
        }

        //public static int ProtReceiveMessageWide(byte[] inBuff, /*byte[] outBuff,*/ int inBuffSize)
        //{
        //    int indx;
        //    byte cByte;

        //    for (indx = 0; indx < inBuffSize; indx++)
        //    {
        //        cByte = inBuff[indx];
        //        switch (BinState)
        //        {
        //            case (ReceiveMachine.Begin):
        //                {
        //                    if (cByte == 0xC0)
        //                    {
        //                        Debug.Write("=[PROTBIN]::Start mark finding");
        //                        byteCount = 0;
        //                        BinState = ReceiveMachine.Msg;
        //                    }
        //                    break;
        //                }

        //            case (ReceiveMachine.Msg):
        //                {
        //                    if (cByte == 0xC0)
        //                        if (byteCount > 2)
        //                        {
        //                            Debug.Write("=[PROTBIN]::Stop message <" + byteCount + ">");
        //                            BinState = ReceiveMachine.Begin;

        //                            Crc16.CalcVal(gsmRxBuff, byteCount - 2, 1);
        //                            if (gsmRxBuff[byteCount - 2] == Crc16.GetHiVal() && gsmRxBuff[byteCount - 1] == Crc16.GetLoVal())
        //                            {
        //                                return byteCount - 2;
        //                            }
        //                            Debug.Write("=[PROTBIN]::err Crc wait <" + gsmRxBuff[byteCount - 2] + "><" + gsmRxBuff[byteCount - 1] +
        //                            "> calc <" + Crc16.GetHiVal() + "><" + Crc16.GetLoVal() + ">");
        //                        }
        //                        else
        //                        {
        //                            Debug.Write("=[PROTBIN]::err Start byte double");
        //                        }
        //                    else
        //                        byteCount = PutByteInRxFifo(cByte);
        //                    break;
        //                }

        //            case (ReceiveMachine.Final):
        //                {
        //                    BinState = ReceiveMachine.Begin;
        //                    break;
        //                }
        //            default:
        //                break;
        //        }
        //    }
        //    return 0;
        //}

        //static int PutByteInRxFifo(byte cChr)
        //{

        //    if (cChr == 0xDB)
        //    {
        //        stuffing = 1;
        //        return byteCount;
        //    }
        //    if (stuffing == 1)
        //    {
        //        if (cChr == 0xDC)
        //        {
        //            cChr = 0xC0;
        //        }
        //        if (cChr == 0xDD)
        //        {
        //            cChr = 0xDB;
        //        }
        //        stuffing = 0;
        //    }
        //    gsmRxBuff[byteCount++] = cChr;

        //    return byteCount;
        //}
    
    }


    static public class  ProtFifo       
    {
        const int GSM_FIFO_SIZE = 500000;
        static byte[] gsmFifo = new byte[GSM_FIFO_SIZE];
        
        static int gsmFifoHead = 0;
        static int gsmFifoTail = 0;



        static void RollHead()
        {
            gsmFifoHead = (gsmFifoHead + 1) % GSM_FIFO_SIZE;
        }

        static void RollTail()
        {
            gsmFifoTail = (gsmFifoTail + 1) % GSM_FIFO_SIZE;
        }

        static bool IsBufferFull()
        {
            return ((gsmFifoHead + 1) % GSM_FIFO_SIZE == gsmFifoTail);
        }

        static bool IsBufferEmpty()
        {
            return (gsmFifoTail == gsmFifoHead);
        }


        static public int InsertByte(int inByte)

        {
            if (!IsBufferFull())
            {
                gsmFifo[gsmFifoHead] = (byte)inByte;
                RollHead();
                return inByte;
            }
            Debug.Write("=[FIFO]:Error buffer is full(!)");
            return -1;
        }

        static public int GetByte()
        {
            int res = -1;
            if (!IsBufferEmpty())
            {
                res = (int)gsmFifo[gsmFifoTail];
                RollTail();
            }
            return res;
        }
        static public int InsertByteBuffer(byte[] dGramm, int dLen)
        {
            int indx;

            for (indx = 0; indx < dLen; indx++)
            {
                if (InsertByte((int)dGramm[indx]) < 0)
                {
                    Debug.Write("[FIFO]:error> Buffer is Full(!)");
                    break;
                }
            }
            return indx;
        }

        static public int FFlushBuf()
        {
            gsmFifoHead = gsmFifoTail = 0;
            return 0;
        }
    }


















    /* --------------------------------------------------------------------- */
    public class fifoCommon
    {
      /*
       * @param - _siz: value inicializate in constructor 
       * and mean size of buf (better will be power of two size
       * 1024, 2048, etc
       * */
      int _siz;
      

      /*
       * @param - bfif: buffer for data 
       * */
      byte[] bfifo;


      /* 
       * Head and Tail of circular 
       * */
      int btail;
      int bhead;


      public fifoCommon(int size)
      {
        _siz = size;
        bfifo = new byte[size];
        btail = bhead = 0;
      }


      public int item()
      {
        if (isempty()) return -1;
        return (int)bfifo[rolltail()];
      }


      public int item(byte inb)
      {
        if (isfull()) return -1;
        return (bfifo[rollhead()] = (inb));
      }

      public int item(byte[] inb, int len)
      {
        int count = 0, retval = -1;
        do 
        {
          retval = item(inb[count++]);
        } while (count < len && retval >= 0);

        return count;
      }


      public int freespace()
      {
        return (bhead > btail ? (_siz - (bhead - btail)) : (_siz - (btail - bhead)));
      }

      bool isempty() { return (bhead == btail); }
      bool isfull() { return (((bhead + 1) % _siz) == btail); }
      
      int rolltail() { int bres = btail; btail = roll(btail); return bres; }
      int rollhead() { int bres = bhead; bhead = roll(bhead); return bres; }

      private int roll(int index) { return ((index + 1) % _siz); }
    }

    internal class UnpackBlock
    {
      const byte EDGE_B = 0xc0;
      const byte ESC = 0xDB;
      const byte ESC_C = 0xDC;
      const byte ESC_D = 0xDD;

      const int SIZE_RX_BUFF = 1500;

      enum eStMach { Begin, Esc, Msg, Final };

      eStMach stMach = eStMach.Begin;

      int bcount;
      int retfifo;
      byte cbyte;

      internal int stripMain(fifoCommon ff, byte[] vic)
      {
        while (true)
        {
          retfifo = ff.item();

          if (retfifo < 0)
            break;

          cbyte = (byte)retfifo;

          switch (stMach)
          {
            case (eStMach.Begin):
              bcount = 0;
              if (cbyte == EDGE_B)
                stMach = eStMach.Msg;
              break;

            case (eStMach.Esc):
              
              stMach = eStMach.Msg;

              if (cbyte == ESC_C)
                vic[bcount++] = EDGE_B;

              else if (cbyte == ESC_D)
                vic[bcount++] = ESC;

              else
                stMach = eStMach.Begin;
                
              break;

            case (eStMach.Msg):
              if (cbyte == ESC)
              {
                stMach = eStMach.Esc;
                break;
              }

              else if (cbyte == EDGE_B)
              {
                int ret = checMessage(vic);
                if (ret > 0)
                  return ret;
              }
              else
                vic[bcount++] = cbyte;
              break;

            case (eStMach.Final):
              break;

            default:
              break;

              
          }// switch
          if (bcount > vic.Length - 1)
            stMach = eStMach.Begin;
        } // while 

        return 0;
      }

      private int checMessage(byte[] vic)
      {
        if (bcount < 6)
          return 0;

        stMach = eStMach.Begin;

        Crc16.CalcVal(vic, bcount - 2, 1);
        if (Crc16.CrcValid(vic[bcount - 1], vic[bcount - 2]))
          return (bcount - 2);
        return -1;

      }

    }
}


