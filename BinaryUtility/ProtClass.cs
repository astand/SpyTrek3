using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using ProtSys.Abstract;
using ProtSys.Concrete;

namespace ProtSys
{

    public struct pHead
    {
        public byte Tind;
        public byte Rind;

        public byte Addr;
        public byte Func;
        public byte SrcAddr;
        public byte DstAddr;
        public byte Cmd;
        public byte PlushDataSize;

        public short tCode;
        public short tID;
    };


    public class DProt
    {

        const byte PROTWIDECMD = 0x6E;
        const byte PROTWIDESIZE = 0xFF;

        const byte DEFAULTSRCADDR = 0;
        const int PROTWIDEMAXSIZE = 1500;
        const int PROTWIDEDEFAULTSIZE = 1000 + 2;



        public pHead Head;

        static int HeadSize = Marshal.SizeOf(typeof(pHead));
        
        public byte[] Data;

        
        public int DataLenght 
        {
            get { return (Data.Length); }
        }

        public DProt(int size)
        {
            if (size > PROTWIDEMAXSIZE)
                size = PROTWIDEDEFAULTSIZE;

            Data = new byte[size+2];
        }

        public DProt()
        {
            Data = null;
        }

        public void SetTf(short tfCode, short tID)
        {
            Head.tID = tID;
            Head.tCode = tfCode;
        }
        public void SetTxRxIndex(byte tx, byte rx)
        {
            Head.Tind = tx;
            Head.Rind = rx;
        }

        public void SetAddrCmd(byte dstAddr, byte protCmd)
        {
            Head.DstAddr = Head.Addr = dstAddr;
            Head.Cmd = protCmd;
        }

        public void SetHead(byte sAddr, byte sCmd, byte tx = 0x81, byte rx = 0x00, 
                            byte sFunc = PROTWIDECMD, byte sSrcAddr = DEFAULTSRCADDR)
        {
            SetAddrCmd(sAddr, sCmd);
            Head.SrcAddr = sSrcAddr;
            Head.Func = sFunc;
            SetTxRxIndex(tx, rx);
            Head.PlushDataSize = 0xff;
            Debug.WriteLine("Set Head Action:Addr<{0}>|SrcAddr<{1}>|Code<{2}>|ID<{3}>", sAddr, sSrcAddr, Head.tCode, Head.tID);
        }

        public int SetData(byte[] dataGramm, int len)
        {
            if (len > 1500 || len <0)
                return -1;

            for (int indx = 0; indx < len; Data[indx] = dataGramm[indx], indx++);
            return 0;
        }

        public byte[] GetByte()
        {
            byte[] oBuffer = new byte[HeadSize + Data.Length];

            GCHandle gch = GCHandle.Alloc(Head, GCHandleType.Pinned);
            IntPtr ptr = gch.AddrOfPinnedObject();
            Marshal.Copy(ptr, oBuffer, 0, HeadSize);
            gch.Free();

            for (int indx = 0; indx < Data.Length; oBuffer[HeadSize + indx] = Data[indx], indx++) ;
            return oBuffer;
        }

        static public DProt GetProt(byte[] rawByte)
        {
            if (rawByte.Length < (HeadSize - 2))
                return null;
            int dataSize = (rawByte.Length - HeadSize - 2); // -2 CRC position 
            pHead locHead = new pHead();

            DProt ob = new DProt(dataSize);
            GCHandle gch = GCHandle.Alloc(locHead, GCHandleType.Pinned);
            IntPtr ptr = gch.AddrOfPinnedObject();
            Marshal.Copy(rawByte, 0, ptr, HeadSize);
            locHead = (pHead)Marshal.PtrToStructure(ptr, typeof(pHead));
            gch.Free();

            ob.Head = locHead;

            for (int indx = 0; indx < (dataSize + 2); ob.Data[indx] = rawByte[indx + HeadSize], indx++) ;

            return ob;

        }
        static public DProt GetProt(byte[] rawByte, int len)
        {
            if (len < (HeadSize - 2))
                return null;
            int dataSize = (len - HeadSize - 2);
            pHead locHead = new pHead();

            DProt ob = new DProt(dataSize);
            GCHandle gch = GCHandle.Alloc(locHead, GCHandleType.Pinned);
            IntPtr ptr = gch.AddrOfPinnedObject();
            Marshal.Copy(rawByte, 0, ptr, HeadSize);
            locHead = (pHead)Marshal.PtrToStructure(ptr, typeof(pHead));
            gch.Free();

            ob.Head = locHead;

            for (int indx = 0; indx < (dataSize + 2); ob.Data[indx] = rawByte[indx + HeadSize], indx++) ;

            return ob;

        }


        /*
         * Set CRC for DProt 
         */
        public int SetCrc()
        {
            byte[] protByteBuff = GetByte();

            Crc16.CalcVal(protByteBuff, (protByteBuff.Length - 2), 1);
            protByteBuff[protByteBuff.Length - 2] = Crc16.GetHiVal();
            protByteBuff[protByteBuff.Length - 1] = Crc16.GetLoVal();

            Data[Data.Length - 2] = protByteBuff[protByteBuff.Length - 2];
            Data[Data.Length - 1] = protByteBuff[protByteBuff.Length - 1];

            return 0;
        }

        public byte[] GetRawByte()
        {
            int indxOut = 0;

            byte[] pureByte = this.GetByte();
            dynByte bu = new dynByte();

            bu[indxOut++] = 0xC0;

            for (int indx = 0; indx < pureByte.Length; indx++)
            {
                if (pureByte[indx] == 0xC0)
                {
                    bu[indxOut++] = 0xDB;
                    bu[indxOut++] = 0xDC;
                }
                else if (pureByte[indx] == 0xDB)
                {
                    bu[indxOut++] = 0xDB;
                    bu[indxOut++] = 0xDD;
                }
                else
                {
                    bu[indxOut++] = pureByte[indx];
                }
            }

            bu[indxOut++] = 0xC0;

            return bu.GetHonest();
        }
    }

    class dynByte
    {
        byte[] mas;
        int len;
        static int defLen = 3000;

        public dynByte()
        {
            mas = new byte[defLen];
            len = 0;
        }
        public int Length => len;

        

        public byte this[int indx]
        {
            get 
            {
                if (isCorrect(indx))
                    return mas[indx];
                else
                    return 0;
            }
            set 
            {
                if (isCorrect(indx))
                {
                    mas[indx] = (byte)value;
                    len++;
                }
            }
        }

        public int AddByte(byte b)
        {
          this[len] = b;

          return len;
        }

        bool isCorrect(int indx) => (indx >= 0 && indx < defLen);

        private void AddByteToTail(byte b)
        {
            this[len] = b;
        }

        public byte[] GetHonest()
        {
            byte[] oout = new byte[this.Length];
            for (int indx = 0; indx < oout.Length; oout[indx] = this[indx], indx++) ;
            return oout;
        }

        public int AddByteArray(byte[] ccat)
        {
            int i = 0;
            if (ccat == null) return 0;
            while ((i < ccat.Length) && (isCorrect(len)))
            {
                mas[len++] = ccat[i++];
            }
            return 0;
        }

        public int AddByteArray(byte[] ccat, int len)
        {
          if (ccat == null) return 0;
          
          
          if (ccat.Length < len || len < 0 ) len = ccat.Length;

          for (int i = 0 ; i < len && (isCorrect(len)); mas[this.len++] = ccat[i++]); 
    
          return len;
        }

        public int SetCrc16()
        {
            byte[] clean_crc_buff = GetHonest();
            Crc16.CalcVal(clean_crc_buff, (clean_crc_buff.Length), 1);
            AddByteToTail(Crc16.GetHiVal());
            AddByteToTail(Crc16.GetLoVal());
            return 0;
        }
    }

    public class OProt
    {
        //typedef struct {
        //    unsigned char Addr;
        //    unsigned char Func;
        //    unsigned char SrcAddr;
        //    unsigned char DstAddr;
        //    unsigned char Cmd;
        //    unsigned char DataSize;
        //    unsigned char Data[MAX_DATA_SIZE_PROT];
        //} BUSSYSFRAME;
        const byte SelfAddr = 0;
        public const byte PROT_NG_STATE_SIZE = 116;
        public const byte PROT_PLC_STAT_SIZE = 0xF0;
        public const byte PROT_PLC_STAT_NUM = (0xF0 / 2); /* 240 / 2 -> 16 bit */
        public const byte PLCANALYZ_BLOCK_SIZE = 24;

        public const byte PROT_STATE_REQ = 0x03;
        public const byte PROT_STATE_ACK = 0x83;

        public const byte PROT_CMD_PLC_STAT = 0x6F;
        public const byte PROT_CMD_PLC_STAT_ACK = 0xEF;

        const int MAX_PROT_LEN = 250;
        public const int MIN_PROT_LEN = 8;
        const int HEAD_PROT_SIZE = 8;

        private byte[] ddgram;
        
        public OProt(int size)
        {
            if (size > MAX_PROT_LEN || size < MIN_PROT_LEN)
                size = MAX_PROT_LEN;
            ddgram = new byte[size + 2];
        }

        public OProt()
        {
            ddgram = null;
        }

        public int SetParam(
            byte txIndx,
            byte addr,
            byte cmd,
            byte dSize,
            byte[] data)
        {
            if (ddgram == null)
                return -1;
            if (ddgram.Length != (dSize + HEAD_PROT_SIZE) + 2)
                return -2;
            ddgram[0] = txIndx;
            ddgram[1] = 0;

            ddgram[2] = addr;
            ddgram[3] = 0x6e;
            ddgram[4] = 0;
            ddgram[5] = addr;

            ddgram[6] = cmd;
            ddgram[7] = dSize;

            for (int i = 0; i < dSize; ddgram[MIN_PROT_LEN + i] = data[i], i++) ;

            return 0;
        }

        public byte[] GetRawByte()
        {
            int indxOut = 0;

            dynByte bu = new dynByte();

            Crc16.CalcVal(ddgram, (ddgram.Length-2),1);
            ddgram[ddgram.Length - 2] = Crc16.GetHiVal();
            ddgram[ddgram.Length - 1] = Crc16.GetLoVal();

            bu[indxOut++] = 0xC0;
            for (int indx = 0; indx < ddgram.Length; indx++)
            {
                if (ddgram[indx] == 0xC0)
                {
                    bu[indxOut++] = 0xDB;
                    bu[indxOut++] = 0xDC;
                }
                else if (ddgram[indx] == 0xDB)
                {
                    bu[indxOut++] = 0xDB;
                    bu[indxOut++] = 0xDD;
                }
                else
                {
                    bu[indxOut++] = this.ddgram[indx];
                }
            }
            bu[indxOut++] = 0xC0;

            return bu.GetHonest();
        }
    }

   


    public class clnd0
    {
        DateTime wd;
        DateTime wdloc;
        byte[] timdat = new byte[6];
        const byte YYEAR = 0;
        const byte MMNTH = 1;
        const byte DDAY = 2;
        const byte HR = 3;
        const byte MIN = 4;
        const byte SEC = 5;
        /* - */
        public static int Lenght 
        {
            get
            {
                return 6 * 1;
            }
        }

        public int Set(byte[] buf, int startpos)
        {
            if ((buf.Length - 6) >= startpos)
            {
                for (int i = 0; i < 6; i++)
                    timdat[i] = buf[startpos + i];
                try
                {
                    wd = new DateTime(timdat[YYEAR] + 2000, timdat[MMNTH], timdat[DDAY], timdat[HR], timdat[MIN], timdat[SEC]);
                }
                catch (Exception ex1)
                {
                    Debug.WriteLine(":.:clnd0>Exception error: " + ex1.Message);
                    wd = new DateTime();
                }
                wdloc = wd.ToLocalTime();
                return 0;
            }
            return -1;
        }

        public string FitDirectory()
        {
          //string stout;
          //stout = String.Format("/{0:D4}/{1:D2}/{2:D2}/", wdloc.Year, wdloc.Month, wdloc.Day);
          //return stout;
          return String.Format("{0:D4}/{1:D2}/{2:D2}/", wdloc.Year, wdloc.Month, wdloc.Day);
        }


        public string Time() => wdloc.ToString("HH-mm");

        public string FullDatString()
        {
            return wdloc.ToString("yyyy.MM.dd HH:mm");
        }
        public string FullDatJSString()
        {
            //vic = string.Format("{0:MM/dd/yy H:mm:ss zzz}", datutc);
            return (String.Format("\"{0:yyyy-MM-ddTHH:mm:ss}\"", wd));
        }

        public DateTime GetDateTime()
        {
            return wdloc;
        }
        

    };

    
    public enum MatrixPositions : int
    {
      ID = 0, DAT0, DAT1, SIZE, SHORTMILE, LONGMILE
    }




  /* ------------------------------------------------------------------------------------------------ */
  /// <summary>
  /// Instanse of that class contains info about one Trek (Trek description)
  /// </summary>
  public sealed class MatrixItem
  {
    static private Int32[] itemoffsets = { 0, 
                                    2, 
                                    2+6, 
                                    2+6+6,
                                    2+6+6+4,
                                    2+6+6+4+4,
                                    2+6+6+4+4+4}; /* size */


    public UInt16 id;
    clnd0 dat0;
    clnd0 dat1;
    public UInt32 size {get; private set;}
    public UInt32 localmileage { get; set; }
    public UInt32 mileage { get; set; }
    private static ITrekNameFormatter frm = new FullFileNameBuilder();

    private static int[] _lenght_collection = { 2,clnd0.Lenght, clnd0.Lenght, 4, 4, 4 };

    private static int _lenght;
    /// <summary>
    /// Private lenght calculator
    /// </summary>
    /// <returns>lenght of payload one treklist item</returns>
    private static int LenghtCalcOneTime()
    {
      int sum = 0;
      foreach (var i in _lenght_collection)
        sum += i;
      return sum;
    }


    static MatrixItem()
    {
      _lenght = LenghtCalcOneTime();
    }
    /// <summary>
    /// Propertiy read access only for determine Lenght payload outside
    /// </summary>
    public static int Lenght
    {
      get
      {
        return _lenght;
        //return ((clnd0.Lenght * 2) + (3 * 4) + 2);
      }
    }

    /// <summary>
    /// Base constructor
    /// </summary>
    public MatrixItem()
    {
      dat0 = new clnd0();
      dat1 = new clnd0();
    }


    public static MatrixItem Factory(byte[] b, ref Int32 offset)
    {
        /* no space for full MI object */
        if ((b.Length - offset) < Lenght)
        {
            return null;
        }

        MatrixItem ret = new MatrixItem();
        ret.id = BitConverter.ToUInt16(b, offset + itemoffsets[0]);
        ret.dat0.Set(b, offset + itemoffsets[1]);
        ret.dat1.Set(b, offset + itemoffsets[2]);
        ret.size = BitConverter.ToUInt32(b, offset + itemoffsets[3]);
        ret.localmileage = BitConverter.ToUInt32(b, offset + itemoffsets[4]);
        ret.mileage = BitConverter.ToUInt32(b, offset + itemoffsets[5]);
        offset += itemoffsets[6];
        return ret;
    }


    /// <summary>
    /// Parsing from data buffer to M_i instance
    /// </summary>
    /// <param name="b">Source data buffer</param>
    /// <param name="offset">start offset in src buffer</param>
    /// <param name="len">len - must strict == M_i.Lenght</param>
    /// <returns></returns>
    public int Parse(byte[] b)
    {
        id = BitConverter.ToUInt16(b,  0);
        dat0.Set(b,  2);
        dat1.Set(b,  2 + 6);
        size = BitConverter.ToUInt32(b,  2 + 6 + 6);
        localmileage = BitConverter.ToUInt32(b,  2 + 6 + 6 + 4);
        mileage = BitConverter.ToUInt32(b,  2 + 6 + 6 + 4 + 4);
        return 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bsrc"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public static MatrixItem Parse(byte[] bsrc, int offset)
    {
      return null;
    }

    /// <summary>
    /// Find in source buffer required M_i block and parse it to M_i 
    /// </summary>
    /// <param name="buff">source buffer</param>
    /// <param name="startpos">offset position in (M_i)</param>
    /// <param name="len">len of payload in buffer</param>
    /// <returns>return 0</returns>
    public int SetFromBuff(byte[] buff, int startpos, int len)
    {
        

      if (startpos + 1 > (len / MatrixItem.Lenght))
          return -1;
      else
      {
        byte[] src = new byte[MatrixItem.Lenght];
        Array.Copy(buff, startpos * MatrixItem.Lenght, 
                    src, 0, Lenght); 
        Parse(src);
      }
      return 0;
    }

        

    public string MPrintLog(int i)
    {
      string stout;
      stout = (id.ToString() + "   " + dat0.FullDatString() + "   " + dat1.FullDatString() +
              "\t" + size + "\t" + localmileage + "\t" + mileage);
      return stout;
    }

      
    public string MPrintTime()
    {
        return (dat0.FullDatString() + "    " + dat1.FullDatString());
    }


    public string GetTrekName()
    {
        return frm.NameBuild(this);
    }


    public string GetTrekDir()
    {
      return (dat0.FitDirectory());
    }

    internal Int32 getNumsOfNaviNotes()
    {
      return ((Int32)size / navinote.Lenght);
    }


    internal bool DistanceValid
    {
      get { return localmileage > MINIMUM_DISTANCE_DETECT; }
    }

        public DateTime getStart()
        {
            return dat0.GetDateTime();
        }

        public DateTime getEnd()
        {
            return dat1.GetDateTime();
        }


    /// <summary>
    /// Minimun distance for auto downloading trek
    /// </summary>
    private static readonly Int32 MINIMUM_DISTANCE_DETECT = (200 * 10);
  };


  /* ------------------------------------------------------------------------------------------------ */
    public class GridData
    {
        public UInt16 ID { get; set; }
        public string Name {get; set;}
        public UInt32 SIZE { get; set; }
        public UInt32 KMDIST { get; set; }
        public GridData(UInt16 idx, string st, UInt32 ss, UInt32 km)
        {
            ID = idx;
            Name = st;
            SIZE = ss;
            KMDIST = km;
        }
        public GridData(MatrixItem mtrx)
        {
            ID = mtrx.id;
            Name = mtrx.MPrintTime();
            SIZE = mtrx.size;
            KMDIST = mtrx.localmileage;
        }

    };

  
  
  
  
  
  
  


  
    public class navinote
    {
        const Int32 COORDINATE_PRESCALER = 1000000;
        clnd0 clnd = new clnd0();
        Int16 altitude;
        Int32 lafull;
        Int32 lofull;
        UInt16 spd;
        UInt32 accum_dist;

        UInt16 spare;

        UInt16 adcsrc;
        UInt16 adc1;
        //UInt16 adc2;
        //UInt16 adc3;

        public static Int32 Lenght
        {
            get
            {
                //return (clnd0.Lenght + 4 * 3 + 2 * 3);
                return (clnd0.Lenght +  /* calendar */
                    (4 * 3) +           /* la + lo + distance */
                    (2 * 3) +           /* altitude + spd + spare */
                    //(2 * 4));           /* adc * 3 */
                    (2 * 2));
            }
        }

        public navinote()
        { }

        public navinote(byte[] b, int startpos)
        {
            startpos *= navinote.Lenght;
            clnd.Set(b, startpos);
            startpos += 6;
    
            altitude = BitConverter.ToInt16(b, startpos);
            startpos += 2;
            
            lafull = BitConverter.ToInt32(b, startpos);
            startpos += 4;
            
            lofull = BitConverter.ToInt32(b, startpos);
            startpos += 4;
            
            spd = BitConverter.ToUInt16(b, startpos);
            startpos += 2;
            
            accum_dist = BitConverter.ToUInt32(b, startpos);
            startpos += 4;
            
            spare = BitConverter.ToUInt16(b, startpos);
            startpos += 2;

            adcsrc = BitConverter.ToUInt16(b, startpos);
            startpos += 2;

            adc1 = BitConverter.ToUInt16(b, startpos);
            startpos += 2;

            //adc2 = BitConverter.ToUInt16(b, startpos);
            //startpos += 2;
        }

        public static bool BufferValid(int startpos, int len)
        {
            return (((startpos + 1) * navinote.Lenght) > len) ? (false) : (true);
        }



        public static string PrintCoor(int crd)
        {
            int man = crd / COORDINATE_PRESCALER;
            int div = crd % COORDINATE_PRESCALER;
            return string.Format("{0}.{1:D6}", man, div);
        }



        public string PrintDistance()
        {
            string ret = string.Format("{0}.{1:D3}", accum_dist / 10000, (accum_dist % 10000) / 10);
            return ret;
        }

        public string GetStringNotify()
        {
            string outvic;

            string stdate = clnd.FullDatJSString();
            outvic = "{ lat:" + navinote.PrintCoor(lafull) + ",lon:" + navinote.PrintCoor(lofull) + ",titl:";
            outvic += stdate + ",spd:" + spd / 100 + ",dist:" + PrintDistance() + " },";

            return outvic;
        }
       
    };
}

