using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageHandler.Rig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageHandler.Rig.Common;

namespace MessageHandler.Rig.Tests
{
    [TestClass()]
    public class RigFrameTests
    {

        RigFrame rigFrame = new RigFrame();

        static byte[] testArray = new byte[]
        {
            0x00, 0xc0, 0x01, 0xF0, 0x00, 0x01,
            0xde, 0x12, 0x21
        };

        static byte[] testArraySmall = new byte[]
        {
            0x00, 0xc0, 0x01, 0xF0
        };

        static byte[] testArrayBig = new byte[]
        {
            0x00, 0xc0, 0x01, 0xF0, 0x00, 0xc0,
            0x01, 0xF0, 0x00, 0xc0, 0x01, 0xF0, 0x00, 0xc0, 0x01, 0xF0,
            0x00, 0xc0, 0x01, 0xF0, 0x00, 0xc0, 0x01, 0xF0
        };


        static byte[] testArrayBig2 = new byte[]
        {
            0x00, 0xc0, 0x01, 0xF0, 0x00, 0xc0,
            0x01, 0xF0, 0x00, 0xc0, 0x01, 0xF0, 0x00, 0xc0, 0x01, 0xF0,
            0x00, 0xc0, 0x01, 0xF0, 0x00, 0xc0, 0x01, 0xF0, 0xF0, 0x00,
            0xc0, 0x01, 0xF0, 0x00, 0xc0, 0x01
        };


        [TestInitialize()]
        public void RigFrame_Init()
        {
        }

        [TestMethod()]
        public void RigFrame_RigFrame_ArrayConstruction()
        {
            var frame = new RigFrame();
            Assert.AreEqual(0, (int)frame.Opc);
            Assert.AreEqual(0, (int)frame.RigId);
            Assert.AreEqual(0, frame.Data.Length);
            Assert.AreEqual(0, frame.DataSize);
            frame.ConvertFromBytes(testArraySmall);
            Assert.AreEqual(0, frame.Data.Length);
            Assert.AreEqual(0, frame.DataSize);
            frame.ConvertFromBytes(testArray);
            Assert.AreEqual(3, frame.DataSize);
            Assert.AreEqual(3, frame.Data.Length);
            frame.ConvertFromBytes(testArrayBig2);
            Assert.AreEqual(26, frame.DataSize);
            Assert.AreEqual(26, frame.Data.Length);
            frame.ConvertFromBytes(testArrayBig);
            Assert.AreEqual(18, frame.DataSize);
            Assert.AreEqual(26, frame.Data.Length);
        }

        [TestMethod()]
        public void RigFrame_RigFrame_SerializeToArray()
        {
            var array = new byte[0];
            var array2 = new byte[50];
            var frame = new RigFrame()
            {
                Opc = OpCode.DATA,
                RigId = OpID.Firmware,
                BlockNum = 1,
                Data = new byte[23]
            };
            var ret = frame.ConvertToBytes(array);
            Assert.AreEqual(-1, ret);
            var ret2 = frame.ConvertToBytes(array2);
            Assert.AreEqual(23 + 6, ret2);
        }

        [TestMethod()]
        public void RigFrame_SerializeToByteArray()
        {
            var ret = rigFrame.ConvertToBytes();
            Assert.AreEqual(6, ret.Length);
            rigFrame.ConvertFromBytes(testArrayBig2);
            ret = rigFrame.ConvertToBytes();
            Assert.AreEqual(6 + 26, ret.Length);
            rigFrame.ConvertFromBytes(testArrayBig);
            ret = rigFrame.ConvertToBytes();
            Assert.AreEqual(6 + 18, ret.Length);
        }

        [TestMethod()]
        public void RigFrame_DeserializeFromArray()
        {
            var frame = new RigFrame();
            frame.ConvertFromBytes(new Byte[] { 0x33, 0x32, 3, 3, 23, 32 });
            Assert.AreEqual(0, frame.Data.Length);
        }

        [TestMethod()]
        public void RigFrame_AccessToOutOfRange()
        {
            var frame = new RigFrame();
            frame.ConvertFromBytes(testArrayBig2);
            byte b = frame.Data[25];
            frame.ConvertFromBytes(testArrayBig);
            b = frame.Data[25];
        }
    }
}