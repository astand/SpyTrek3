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
    public class ResizeableArrayTests
    {
        [TestMethod()]
        public void Test_ResizeableArray()
        {
            ResizeableArray victim = new ResizeableArray();
            Assert.AreEqual(victim.Length, 0);

            victim.AddByte(0);
            Assert.AreEqual(1, victim.Length);
        }

        
        [TestMethod()]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Test_ResizeableArray_AddingOverByte()
        {
            ResizeableArray victim = new ResizeableArray(4);

            for (int i = 0; i < 4; i++)
            {
                victim.AddByte((byte)i);
            }

            victim.AddByte(5);
        }

        [TestMethod()]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Test_ResizeableArray_ReadingOverByte()
        {
            ResizeableArray victim = new ResizeableArray(4);

            var waiting_byte = victim[3];
            Assert.AreEqual(0, waiting_byte);

            waiting_byte = victim[4];
        }

    }
}