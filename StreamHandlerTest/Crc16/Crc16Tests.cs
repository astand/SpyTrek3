using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using StreamHandler;
using System.Diagnostics;

namespace StreamHandler.Tests
{
    [TestClass]
    public class Crc16Tests
    {
        public Crc16 victim = new Crc16();

        [TestMethod]
        public void Crc16_CalculationRigth()
        {
            var retarray = victim.GetCheckedArray(Crc16TestData.ShortCleanArray);
            Assert.AreEqual(Crc16TestData.ShortArrayHi, victim.Hi);
            Assert.AreEqual(Crc16TestData.ShortArrayLow, victim.Low);
        }


        [TestMethod]
        public void Crc16_CalculationRigthLong()
        {
            var retarray = victim.GetCheckedArray(Crc16TestData.LongCleanArray);
            Assert.AreEqual(Crc16TestData.LongArrayHi, victim.Hi);
            Assert.AreEqual(Crc16TestData.LongArrayLow, victim.Low);
        }

        [TestMethod]
        public void Crc16_CheckedArrayLength()
        {
            var testarray = new Byte[100];

            var retarray = victim.GetCheckedArray(testarray);

            Assert.AreEqual(testarray.Length + 2, retarray.Length);
        }



        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Crc16_TestNullAsParametr()
        {
            Byte[] nullarray = null;

            var retval = victim.GetCheckedArray(nullarray);
        }

    }
}