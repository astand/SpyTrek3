using MessageHandler;
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
        static MemoryPipe dataPipe = new MemoryPipe(fifo);
        Piper pipe = new Piper(dataPipe, dataPipe);
       

        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void Piper_PiperNullCreation()
        {
            Piper victim = new Piper(null, null);
        }

        [TestMethod()]
        public void Piper_PiperCreation()
        {
            Boolean dataGet = false;
            byte[] received_data = new byte[0];
            Piper pip = new Piper(new MemoryPipe(new MemoryStream()), new MemoryPipe(new MemoryStream()));

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
    }
}