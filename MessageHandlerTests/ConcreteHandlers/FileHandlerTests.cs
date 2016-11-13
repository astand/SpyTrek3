using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageHandler.ConcreteHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MessageHandler.Abstract;
using StreamHandler.Abstract;

namespace MessageHandler.ConcreteHandlers.Tests
{
    [TestClass()]
    public class FileHandlerTests
    {

        IHandler<FramePacket> test1;
        IHandler<FramePacket> test2;
        IHandler<FramePacket> test3;

        IHandler<FramePacket> customtest1;
        IHandler<FramePacket> customtest2;
        IHandler<FramePacket> customtest3;

        TestReadHandler notes = new TestReadHandler(
            "Note list",
            "Data",
            "Head",
            "Error");



        TestReadHandler info = new TestReadHandler(
            "Info",
            "Data",
            "Head",
            "Error");


        TestReadHandler track = new TestReadHandler(
            "track",
            "Data",
            "Head",
            "Error");

        TestReadHandlerCustom customnotes = new TestReadHandlerCustom("notes");
        TestReadHandlerCustom custominfo = new TestReadHandlerCustom("Info");
        TestReadHandlerCustom customtrek = new TestReadHandlerCustom("TREK");

        [TestInitialize()]
        public void FileHandler_FileHandler_Creation()
        {
            Func<IStreamData, int> testpipe = delegate(IStreamData data)
            {
                Debug.WriteLine($"Send answer length {data.SerializeToByteArray().Length}");
                return 0;
            };

            test1 = new ConcreteFileHandler<FramePacket>("notes", notes, testpipe);
            test2 = new ConcreteFileHandler<FramePacket>("Info", info, testpipe);
            test3 = new ConcreteFileHandler<FramePacket>("Other", track, testpipe);

            test1.SetSuccessor(test2);
            test2.SetSuccessor(test3);

            test1.SetSpecification(fid => fid == FiledID.Filenotes);
            test2.SetSpecification(fid => fid == FiledID.Info);
            test3.SetSpecification(fid => fid == FiledID.Track);

            customtest1 = new ConcreteFileHandler<FramePacket>("Notes", customnotes, testpipe);
            customtest2 = new ConcreteFileHandler<FramePacket>("Info", custominfo, testpipe);
            customtest3 = new ConcreteFileHandler<FramePacket>("Trek", customtrek, testpipe);

            customtest1.SetSuccessor(customtest2);
            customtest2.SetSuccessor(customtest3);

            customtest1.SetSpecification(fid => fid == FiledID.Filenotes);
            customtest2.SetSpecification(fid => fid == FiledID.Info);
            customtest3.SetSpecification(fid => fid == FiledID.Track);

        }

        [TestMethod()]
        public void FileHandler_HandleTest()
        {
            var current_fid = FiledID.Track;

            test1.HandleRequest(new FramePacket(OpCodes.RRQ, current_fid, null), current_fid);
            test1.HandleRequest(new FramePacket(OpCodes.DATA, 1, null), current_fid);
            test1.HandleRequest(new FramePacket(OpCodes.DATA, 2, null), current_fid);
            test1.HandleRequest(new FramePacket(OpCodes.DATA, 3, null), current_fid);

            // check handling 
            Assert.AreEqual(1, track.HeadCount);
            Assert.AreEqual(3, track.DataCount);


            current_fid = FiledID.Info;
            test1.HandleRequest(new FramePacket(OpCodes.RRQ, current_fid, null), current_fid);
            test1.HandleRequest(new FramePacket(OpCodes.DATA, 1, null), current_fid);
            test1.HandleRequest(new FramePacket(OpCodes.DATA, 2, null), current_fid);

            // check handling
            Assert.AreEqual(1, info.HeadCount);
            Assert.AreEqual(2, info.DataCount);
            Assert.AreEqual(0, info.ErrorCount);

            current_fid = FiledID.Filenotes;

            test1.HandleRequest(new FramePacket(OpCodes.RRQ, current_fid, null), current_fid);
            test1.HandleRequest(new FramePacket(OpCodes.DATA, 1, null), current_fid);
            test1.HandleRequest(new FramePacket(OpCodes.WRQ, current_fid, null), current_fid);
            test1.HandleRequest(new FramePacket(OpCodes.ERROR, current_fid, null), current_fid);

            Assert.AreEqual(1, notes.HeadCount);
            Assert.AreEqual(1, notes.DataCount);
            Assert.AreEqual(2, notes.ErrorCount);
        }

        [TestMethod()]
        public void FileHandler_HandleCustomIProcessor()
        {
            var current_fid = FiledID.Track;

            customtest1.HandleRequest(new FramePacket(OpCodes.RRQ, current_fid, null), current_fid);
            customtest1.HandleRequest(new FramePacket(OpCodes.DATA, 1, null), current_fid);
            customtest1.HandleRequest(new FramePacket(OpCodes.DATA, 2, null), current_fid);
            customtest1.HandleRequest(new FramePacket(OpCodes.DATA, 3, null), current_fid);

            // check handling 
            Assert.AreEqual(1, customtrek.HeadCount);
            Assert.AreEqual(3, customtrek.DataCount);


            current_fid = FiledID.Info;
            customtest1.HandleRequest(new FramePacket(OpCodes.RRQ, current_fid, null), current_fid);
            customtest1.HandleRequest(new FramePacket(OpCodes.DATA, 1, null), current_fid);
            customtest1.HandleRequest(new FramePacket(OpCodes.DATA, 2, null), current_fid);

            // check handling
            Assert.AreEqual(1, custominfo.HeadCount);
            Assert.AreEqual(2, custominfo.DataCount);
            Assert.AreEqual(0, custominfo.ErrorCount);

            current_fid = FiledID.Filenotes;

            customtest1.HandleRequest(new FramePacket(OpCodes.RRQ, current_fid, null), current_fid);
            customtest1.HandleRequest(new FramePacket(OpCodes.DATA, 1, null), current_fid);
            customtest1.HandleRequest(new FramePacket(OpCodes.WRQ, current_fid, null), current_fid);
            customtest1.HandleRequest(new FramePacket(OpCodes.ERROR, current_fid, null), current_fid);

            Assert.AreEqual(1, customnotes.HeadCount);
            Assert.AreEqual(1, customnotes.DataCount);
            Assert.AreEqual(2, customnotes.ErrorCount);
        }

    }

    internal class TestReadHandler : FrameProccesorTemplate
    {
        public int HeadCount = 0;
        public int DataCount = 0;
        public int ErrorCount = 0;

        private String DataMessage { get; }
        private String HeadMessage { get; }
        private String ErrorMessage { get; }

        private String Name { get; }

        public TestReadHandler(String name, String data, String head, String error)
        {
            Name = name;
            DataMessage = data;
            HeadMessage = head;
            ErrorMessage = error;
        }

        public void ReInit()
        {
            HeadCount = DataCount = ErrorCount = 0;
        }

        protected override void ProcessData(FramePacket packet)
        {
            Debug.WriteLine($"{Name} : {DataMessage}");
            DataCount++;
        }

        protected override void ProcessError(FramePacket packet)
        {
            ErrorCount++;
            Debug.WriteLine($"{Name} : {ErrorMessage}");
        }

        protected override void ProcessHead(FramePacket packet)
        {
            HeadCount++;
            Debug.WriteLine($"{Name} : {HeadMessage}");
        }

    }

    internal class TestReadHandlerCustom : IFrameProccesor
    {
        public Int32 HeadCount;
        public Int32 DataCount;
        public Int32 ErrorCount;

        string Name;

        public TestReadHandlerCustom(string name)
        {
            Name = name;
            ReInit();
        }

        void ReInit()
        {
            HeadCount = DataCount = ErrorCount = 0;
        }


        public void Process(FramePacket packet, ref IStreamData answer, out Processors.State state)
        {
            state = Processors.State.Idle;

            answer = null;
            if (packet.Opc == OpCodes.RRQ)
            {
                PrintResult("RRQ");
                HeadCount++;
            }
            else if (packet.Opc == OpCodes.DATA)
            {
                PrintResult("DATA");
                DataCount++;
                answer = new FramePacket(opc: OpCodes.ACK, id: packet.Id, data: null);
            }

            else
            {
                PrintResult("ERROR");
                ErrorCount++;
            }
            
        }

        private void PrintResult(string message)
        {
            Debug.WriteLine($"{Name} : type - {message}");
        }
    }

    class TestPipeWriter : IPipeWriter
    {
        public Int32 Write(Byte bt)
        {
            Debug.WriteLine($"byte to pipe");
            return 0;
        }

        public Int32 Write(Byte[] array, Int32 length = -1)
        {
            Debug.WriteLine($"Array to pipe {array.Length}. Opc = {BitConverter.ToUInt16(array,0)} Id = {BitConverter.ToUInt16(array, 2)}");
            return 0;
        }
    }
}