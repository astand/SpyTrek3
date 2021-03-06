﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamHandler
{
    /// <summary>
    /// Byte value 0xC0 split to pair of 0xDB and 0xDC bytes
    /// Byte value 0xDB split to pair of 0xDB and 0xDD bytes 
    /// </summary>
    public class ByteStuffer
    {
        private enum StuffState { stuff_NEED, stuff_NONE };

        private enum ReceiveState { wait_START, wait_END };

        private const Byte ESC = 0xC0;

        private const Byte MARKB = 0xDB;

        private const Byte MARKD = 0xDD;

        private const Byte MARKC = 0xDC;

        internal Byte[] output_buff = new byte [3000];

        Int32 input_index = 0;

        Int32 output_buff_length = 0;

        private StuffState stuffState = StuffState.stuff_NONE;

        private ResizeableArray resizeArray = new ResizeableArray();

        public Byte[] ArrayToStuff(Byte[] buf)
        {
            resizeArray.Flush();

            resizeArray.AddByte(ESC);

            foreach (var onebyte in buf)
                AddStuffedByte(onebyte);

            resizeArray.AddByte(ESC);

            return resizeArray.GetByteArray();
        }

        public Int32 TryStripDataFlow(Byte bt)
        {
            if (bt == ESC)
            {
                Int32 ret_length = input_index;
                input_index = 0;
                return (ret_length < 3) ? (0) : (output_buff_length = ret_length);
            }

            Int32 out_byte = RestoreByte(bt);

            if (out_byte >= 0)
                output_buff[input_index++] = (byte)out_byte;

            return 0;
        }

        public Byte[] UnstuffedToArray()
        {
            Byte[] retbuff = new Byte[output_buff_length];
            Array.Copy(output_buff, retbuff, output_buff_length);

            return retbuff;
        }

        private Int32 RestoreByte(Byte bt)
        {
            Byte retbyte = bt;

            if (bt == MARKB)
            {
                stuffState = StuffState.stuff_NEED;
                return -1;
            }

            if (stuffState == StuffState.stuff_NEED)
            {
                stuffState = StuffState.stuff_NONE;
                if (bt == MARKC)
                    retbyte = ESC;
                else if (bt == MARKD)
                    retbyte = MARKB;
            }
            return retbyte;
        }

        private void AddStuffedByte(Byte bt)
        {
            if (bt == ESC)
            {
                resizeArray.AddByte(MARKB);
                resizeArray.AddByte(MARKC);
            }
            else if (bt == MARKB)
            {
                resizeArray.AddByte(MARKB);
                resizeArray.AddByte(MARKD);
            }
            else
                resizeArray.AddByte(bt);
        }
    }
}
