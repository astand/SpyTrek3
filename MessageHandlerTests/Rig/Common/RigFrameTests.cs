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

        RigFrame rigFrame;

        static byte[] testArray = new byte[]
        {
            0x00, 0xc0, 0x01, 0xF0, 0x00, 0x01, 0xde, 0x12, 0x21
        };

        [TestInitialize()]
        public void RigFrame_Init()
        {
            rigFrame = new RigFrame()
            {
                Opc = (OpCode)0x1213,
                RigId = OpID.SoleTrek,
                BlockNum = 2134,
                Data = new byte[12]
            };
        }

        [TestMethod()]
        public void RigFrame_RigFrame_ArrayConstruction()
        {
            var frame = new RigFrame(testArray);
            Assert.AreEqual(3, frame.Data.Length);
        }

        [TestMethod()]
        public void RigFrame_RigFrame_SerializeToArray()
        {
            var array = new byte[0];
            var array2 = new byte[23 + 6];
            var frame = new RigFrame()
            {
                Opc = OpCode.DATA,
                RigId = OpID.Firmware,
                BlockNum = 1,
                Data = new byte[23]
            };
            var ret = frame.SerializeToByteArray(array);
            var ret2 = frame.SerializeToByteArray(array2);
            Assert.AreEqual(false, ret);
            Assert.AreEqual(true, ret2);
        }

        [TestMethod()]
        public void RigFrame_SerializeToByteArray()
        {
            var ret = rigFrame.SerializeToByteArray();
            Assert.AreEqual(12 + 6, ret.Length);
        }

        [TestMethod()]
        public void RigFrame_DeserializeFromArray()
        {
            var frame = new RigFrame();
            frame.DeserializeFromArray(new Byte[] { 0x33, 0x32, 3, 3, 23, 32 });
            Assert.AreEqual(0, frame.Data.Length);
        }
    }
}