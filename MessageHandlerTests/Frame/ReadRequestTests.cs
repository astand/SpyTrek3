using MessageHandler;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StreamHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamHandler.Tests
{
    [TestClass()]
    public class ReadRequestTests
    {
        ReadRequest readRequest;
        [TestInitialize()]
        public void ReadRequest_Create()
        {
            readRequest = new ReadRequest(1);
        }

        [TestMethod()]
        public void ReadRequest_ReadRequest_Test()
        {
            Assert.AreEqual(4, readRequest.SerializeToByteArray().Length);
        }

        [TestMethod()]
        public void ReadRequest_SerializeToByteArray_Test()
        {
            var requestAsByte = readRequest.SerializeToByteArray();

            Assert.AreEqual(1, requestAsByte[0]);
            Assert.AreEqual(0, requestAsByte[1]);
            Assert.AreEqual(1, requestAsByte[2]);
            Assert.AreEqual(0, requestAsByte[3]);

        }
    }
}