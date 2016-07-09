using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageHandler.DataFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MessageHandler.DataFormats.Tests
{
    [TestClass()]
    public class TrekDescriptorTests
    {
        Byte[] array_ok = new byte[] {
            1, 0,
            12, 1, 21, 11, 11, 12,
            12, 1, 21, 11, 23, 02,
            12, 2, 0, 0,
            3, 1, 0, 0,
            10, 20, 0, 0
        };

        Byte[] array_short = new byte[] {
            1, 0,
            12, 1, 21, 11, 11, 12,
            12, 1, 21, 11, 23, 02,
            12, 2, 0, 0,
            3, 1, 0, 0,
            10, 20
        };


        [TestInitialize()]
        public void Init()
        {

        }

        [TestMethod()]
        public void TrekDescriptor_Constructor_Ok()
        {
            var desc = new TrekDescriptor();
            
            var ret = desc.TryParse(array_ok, 0);
            Debug.WriteLine("Ok TrekDescriptor test " + desc.ToString());
            Assert.AreEqual(true, ret);
        }

        [TestMethod()]
        public void TrekDescriptor_Constructor_Bad()
        {
            var desc = new TrekDescriptor();

            var ret = desc.TryParse(array_short, 0);
            Debug.WriteLine("Bad TrekDescriptor test " + desc.ToString());
            Assert.AreEqual(false, ret);
        }
    }
}