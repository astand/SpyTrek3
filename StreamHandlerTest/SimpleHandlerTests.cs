using Microsoft.VisualStudio.TestTools.UnitTesting;
using StreamHandler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamHandler.Tests
{
    [TestClass()]
    public class SimpleHandlerTests
    {
        SimpleHandler victim = new SimpleHandler();

        [TestMethod()]
        public void SimpleHandler_PackPacket_Test()
        {
            var stuffed_arr = victim.PackPacket(Crc16TestData.ShortCleanArray);

            Boolean unpack_result = false;

            foreach (var ontbt in stuffed_arr)
                unpack_result = victim.UnpackPacket(ontbt);

            Assert.AreEqual(true, unpack_result);
        }

        
        [TestMethod()]
        public void SimpleHandler_UnPackPacket_ArrayAsChunks()
        {
            var idx = 0;
            Boolean retval = false;
            Debug.WriteLine($"{ Crc16TestData.ShortFullPacked.Length }");
            for (idx = 0; idx < 10; idx++)
                retval = victim.UnpackPacket(Crc16TestData.ShortFullPacked[idx]);

            Assert.AreEqual(false, retval);

            for (idx = 10; idx < 30; idx++)
                retval = victim.UnpackPacket(Crc16TestData.ShortFullPacked[idx]);

            Assert.AreEqual(false, retval);

            for (idx = 30; idx < Crc16TestData.ShortFullPacked.Length && retval == false; idx++)
                retval = victim.UnpackPacket(Crc16TestData.ShortFullPacked[idx]);

            Assert.AreEqual(true, retval);


        }

        [TestMethod()]
        public void SimpleHandler_UnPackPacket_MixedStuff()
        {
            Boolean retval = false;
            Int32 good_packet_count = 0;
            foreach (var item in Crc16TestData.MixStuffed)
            {
                retval = victim.UnpackPacket(item);
                if (retval)
                    good_packet_count++;

            }

            Assert.AreEqual(2, good_packet_count);
        }


        [TestMethod()]
        public void SimpleHandler_UnPackPacket_FullArray()
        {
            var idx = 0;
            Boolean retval = false;

            while (idx < Crc16TestData.ShortFullPacked.Length && retval == false)
                retval = victim.UnpackPacket(Crc16TestData.ShortFullPacked[idx++]);

            Assert.AreEqual(true, retval);

        }


        internal static void PrintArray(Byte[] buff)
        {
            foreach (var item in buff)
            {
                Debug.Write($": {item:X} ");
            }
            Debug.WriteLine("\n");
        }
    }
}