using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtSys
{
    /* ----------------------------------------------------------------------- */
    /// <summary>
    /// Enumeration for get result from receive process
    /// </summary>
    [Flags]
    public enum ReceiveResult
    {
        Free = (1 << 0), // 0x0001

        WaitData = (1 << 1), // 0x0002
        WaitAck = (1 << 2), // 0x0004
        SendData = (1 << 3), //0x0008 

        Empty = 0,

        BlockWrong = (1 << 15)
    };


    /* ----------------------------------------------------------------------- */
    /// <summary>
    /// 
    /// </summary>
    public class ResultOfReceiveProcess
    {
        private ReceiveResult in_res;

        private ReceiveResult WAIT_FLAGS
        {
            get
            {
                return (ReceiveResult.WaitAck |
                        ReceiveResult.WaitData |
                        ReceiveResult.SendData);
            }
        }

        internal void ToStop() { in_res &= ~(WAIT_FLAGS); }

        internal void ToAction(ReceiveResult val)
        {
            ToStop();
            in_res |= val;
        }

        /// <summary>
        /// Get true if WAIT and SEND flags are dropped
        /// </summary>
        internal bool ISWAITSTOP
        {
            get { return ((in_res & (WAIT_FLAGS)) == 0); }
        }


        internal bool ToSendTrue(UInt16 opc)
        {
            ToAction(ReceiveResult.WaitAck);
            return (opc == TfBase.OPC_WRQ) ? (true) : (false);
        }



        internal bool IsFlagsSet(ReceiveResult f)
        {
            return ((in_res & f) != 0);
        }

        internal void DropFlags(ReceiveResult f)
        {
            in_res &= ~f;
        }

        internal void Wrong() { in_res |= ReceiveResult.BlockWrong; }


        /// <summary>
        /// return Wrong block state
        /// </summary>
        /// <param name="f">if true - wrong block bit clear</param>
        /// <returns></returns>
        internal bool IsWrong(bool f = false)
        {
            bool ret;
            ret = WRONG;
            if (f) { DropFlags(ReceiveResult.BlockWrong); }
            return ret;
        }

        internal bool WRONG
        {
            get { return (in_res & ReceiveResult.BlockWrong) != 0; }
        }

        public override string ToString()
        {
            return in_res.ToString();
        }
    };




}
