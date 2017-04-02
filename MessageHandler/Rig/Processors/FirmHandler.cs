using MessageHandler.DataUploading;
using MessageHandler.Rig.Common;
using StreamHandler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MessageHandler.Rig.Processors
{
    public class FirmHandler : IWriterProcessor
    {

        Int32 fileSize = 99999;
        Int32 blockSize = 850;

        public FirmHandler(Piper pipe)
        {
            piper = pipe;
            SetName("Firmware");
        }

        protected override Int32 OnWriteRequest()
        {
            rigFrame.Opc = OpCode.WRQ;
            rigFrame.RigId = OpID.Firmware;
            rigFrame.Data = BitConverter.GetBytes(fileSize);
            return 4;
        }

        protected override Int32 ReadDataChunk()
        {
            Int32 ret = fileSize - ((bid.BidSend - 1) * blockSize);

            if (ret > blockSize)
                ret = blockSize;
            else if (ret < 0)
                ret = 0;

            rigFrame.Data = new byte[ret];
            return ret;
        }

        protected override Int32 OnAckReceive()
        {
            var passed_bytes = (bid.BidAck * blockSize);

            if (passed_bytes > fileSize)
                passed_bytes = fileSize;

            var passed_percent = ((passed_bytes * 100.0) / fileSize) ;

            PState.Message = Name + $": Passed {passed_percent:F1} %. {passed_bytes.ToString().PadRight(5, ' ')} bytes of {fileSize}";
            return 0;
        }

        protected override Int32 OnErrorReceive()
        {
            return base.OnErrorReceive();
        }
    }
}
