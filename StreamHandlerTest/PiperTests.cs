using Microsoft.VisualStudio.TestTools.UnitTesting;
using StreamHandler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
            byte[] received_data = new byte[0];
            Piper pip = new Piper(new MemoryStream());

            pip.OnData += delegate(object obj, PiperEventArgs e)
            {
                dataGet = true;
                received_data = e.Data;
                Debug.WriteLine($"Message from Event: " + e.Message);
            };

            pip.TestOnDataInvoker();

            Assert.AreEqual(true, dataGet);
            Assert.AreEqual(5, received_data.Length);
        }

        [TestMethod()]
        public void Piper_SendData_Test()
        {
            Boolean received = false;
            Byte[] received_array = null;
            StreamData streamData = new StreamData(8);
            pipe.SendData(streamData);

            var can_read = fifo.CanRead;

            Assert.AreEqual(true, can_read);
            Assert.AreEqual(12, fifo.Length);

            pipe.OnData += delegate(object obj, PiperEventArgs e)
            {
                received = true;
                received_array = e.Data;
            };

            var count = 100;
            /// wait some time for handling
            while ((count-- != 0) && (received == false))
                Thread.Sleep(10);

            Assert.AreEqual(true, received);
            Assert.AreEqual(8, received_array.Length);
            //SimpleHandlerTests.PrintArray(fifo.ToArray());
        }
    }
}