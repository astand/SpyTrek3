using Microsoft.VisualStudio.TestTools.UnitTesting;
using StreamHandler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamHandler.Tests
{
    [TestClass()]
    public class PiperTests
    {
        static MemoryStream fifo = new MemoryStream(1000);
        Piper pipe = new Piper(fifo);
       

        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void Piper_PiperNullCreation()
        {
            Piper victim = new Piper(null);
        }

        [TestMethod()]
        public void Piper_PiperCreation()
        {
            Boolean dataGet = false;
            Piper pip = new Piper(new MemoryStream());

            pip.OnData += delegate
            {
                dataGet = true;
            };

            pip.OnDataCaller();

            Assert.AreEqual(true, dataGet);
        }

        [TestMethod()]
        public void Piper_SendData_Test()
        {
            StreamData streamData = new StreamData(8);
            pipe.SendData(streamData);

            var length_of_writed = fifo.Length;

            Assert.AreEqual(12, length_of_writed);
        }
    }
}