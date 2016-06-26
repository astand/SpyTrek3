using Microsoft.VisualStudio.TestTools.UnitTesting;
using StreamHandler;
using StreamHandler.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamHandler.Tests
{
    [TestClass()]
    public class MemoryPipeTests
    {
        MemoryPipe memPipe;
        private MemoryStream sourceMem;

        [TestInitialize()]
        public void MemoryPipe_Creation()
        {
            sourceMem = new MemoryStream(1000);
            memPipe = new MemoryPipe(sourceMem);
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod()]
        public void MemoryPipe_MemoryPipe_Test()
        {
            var NullObject = new MemoryPipe(null);
        }

        [TestMethod()]
        public void MemoryPipe_Read_Test()
        {
            var test_array = new Byte[] { 0, 1, 19, 12, 59, 199 };

            ((IPipeWritable)memPipe).Write(test_array, 4);

            sourceMem.Position = 0;

            Assert.AreEqual(4, ((IPipeReadable)memPipe).DataAvailable());

            Assert.AreEqual(0, ((IPipeReadable)memPipe).ReadByte());
            Assert.AreEqual(1, ((IPipeReadable)memPipe).ReadByte());
            Assert.AreEqual(19, ((IPipeReadable)memPipe).ReadByte());
            Assert.AreEqual(12, ((IPipeReadable)memPipe).ReadByte());

        }
    }
}