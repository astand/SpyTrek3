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
        Int32 fileSize = 0;
        Int32 blockSize = 4 * 240;

        private IDataUploader dataUploader;
        byte[] rawBuff;

        public FirmHandler() : base("FIRMWARE", OpID.Firmware)
        {
            dataUploader = new DiskFileUploader("st8.bin");
            rawBuff = new byte[blockSize];
        }

        protected override Int32 OnWriteRequest()
        {
            fileSize = dataUploader.Length;
            rigFrame.Opc = OpCode.WRQ;
            rigFrame.RigId = OpID.Firmware;
            rigFrame.Data = BitConverter.GetBytes(fileSize);
            PState.Message = Name + $": Write request is sent. Size {fileSize}";
            return 4;
        }

        protected override Int32 ReadDataChunk()
        {
            Int32 offset = (bid.BidSend - 1) * blockSize;
            var ret = dataUploader.ReadData(rawBuff, offset, blockSize);
            rigFrame.Data = new byte[ret];
            Array.Copy(rawBuff, rigFrame.Data, ret);
            return ret;
        }

        protected override Int32 OnAckReceive()
        {
            var passed_bytes = (bid.BidAck * blockSize);

            if (passed_bytes > fileSize)
                passed_bytes = fileSize;

            var passed_percent = ((passed_bytes * 100.0) / fileSize) ;
            PState.Message = Name + $": Passed {passed_percent:F1} %. {passed_bytes.ToString().PadRight(5, ' ')} bytes of {fileSize}";

            if (PState.State == ProcState.Finished)
                PState.Message += ". Finished.";

            return 0;
        }

        protected override Int32 OnErrorReceive()
        {
            return base.OnErrorReceive();
        }
    }
}
