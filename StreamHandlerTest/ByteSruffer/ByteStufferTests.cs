using StreamHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace StreamHandler.Tests
{
    [TestClass()]
    public class ByteStufferTests
    {
        ByteStuffer victim = new ByteStuffer();

        [TestMethod()]
        public void Test_StuffAndUnstuffProcess()
        {
            Byte[] retArray;

            retArray = victim.ArrayToStuff(new Byte[4] { 0, 0xc0, 0xDB, 0 });
            SimpleHandlerTests.PrintArray(retArray);

            Assert.AreEqual(8, retArray.Length);
            Assert.AreEqual(retArray[0], 0xC0);
            Assert.AreEqual(retArray[3], 0xDC);

            Byte[] stuffed_array = new Byte[retArray.Length - 2];
            Array.Copy(retArray, 1, stuffed_array, 0, stuffed_array.Length);

            var unstuffed_successfully = 0;

            foreach (var item in retArray)
            {
                if (victim.TryStripDataFlow(item) > 0)
                {
                    unstuffed_successfully++;
                }
            }

            Assert.AreEqual(1, unstuffed_successfully);
        }

        [TestMethod()]
        public void ByteStuffer_TryStripDataFlow_Test()
        {
            Int32 [] lengths = new int[6];
            Int32 count_of_good = 0;
            Int32 retval = -1;

            foreach (var item in Crc16TestData.MixStuffed)
            {
                retval = victim.TryStripDataFlow(item);
                if (retval > 0)
                {
                    lengths[count_of_good] = retval;
                    count_of_good++;

                }
            }

            Assert.AreEqual(6, count_of_good);
            Assert.AreEqual(254, lengths[count_of_good - 1]);
        }

    }
}