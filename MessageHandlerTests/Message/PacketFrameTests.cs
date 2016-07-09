using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.Tests
{
    [TestClass()]
    public class MessagePacketTests
    {
        FramePacket victim;

        [TestInitialize()]
        public void MessagePacket_Init()
        {
            victim = new FramePacket(new Byte[] { 1, 2, 5, 3, 1, 29 });
        }

        [TestMethod()]
        public void MessagePacket_MessagePacket_MinLength()
        {
            var test_array = new Byte[] { 0, 0, 0, 0 };

            FramePacket packet = new FramePacket(test_array);
            Assert.AreEqual(0, packet.Data.Length);

            //Assert.Fail();
        }

        [TestMethod()]
        public void MessagePacket_Create_Long_Array()
        {
            Assert.AreEqual(2, victim.Data.Length);

            Assert.AreEqual(1, victim.Data[0]);
            Assert.AreEqual(29, victim.Data[1]);
        }


        [ExpectedException(typeof(ArgumentException))]
        [TestMethod()]
        public void MessagePacket_Create_NonValidArray()
        {
            var test_array = new Byte[] { 0, 0 ,0 };

            var packet = new FramePacket(test_array);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod()]
        public void MessagePacket_NullCreation()
        {
            Byte[] test_array = null;

            var packet = new FramePacket(test_array);
        }

        [TestMethod()]
        public void MessagePacket_SerializeToByteArray_Test()
        {
            var deserialized_array = victim.SerializeToByteArray();
            Assert.AreEqual(6, deserialized_array.Length);

            Assert.AreEqual(1, deserialized_array[0]);
            Assert.AreEqual(29, deserialized_array[5]);

        }
    }
}