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
    public class WriteRequestTests
    {
        [TestMethod()]
        public void WriteRequest_WriteRequest_Test()
        {
            var victim = new WriteRequest(1, 20000);

            var retarray = victim.SerializeToByteArray();


            Assert.AreEqual(10, retarray.Length);
        }
    }
}