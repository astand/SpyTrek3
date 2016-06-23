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

            retArray = victim.GetStuffed(new Byte[4] { 0, 0xc0, 0xDB, 0 });
            SimpleHandlerTests.PrintArray(retArray);

            Assert.AreEqual(8, retArray.Length);
            Assert.AreEqual(retArray[0], 0xC0);
            Assert.AreEqual(retArray[3], 0xDC);

            Byte[] stuffed_array = new Byte[retArray.Length - 2];
            Array.Copy(retArray, 1, stuffed_array, 0, stuffed_array.Length);
            var unstuffed_successful = victim.GetUnstuffed(stuffed_array);
            SimpleHandlerTests.PrintArray(stuffed_array);

            Assert.AreEqual(4, unstuffed_successful.Length);
           //Assert.Fail();
        }
    }
}