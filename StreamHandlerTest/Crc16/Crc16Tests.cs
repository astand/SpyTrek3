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
        public void Test_CalculationRigth()
        {
            var retarray = victim.GetCheckedArray(Crc16TestData.ShortCleanArray);
            Assert.AreEqual(Crc16TestData.ShortArrayHi, victim.Hi);
            Assert.AreEqual(Crc16TestData.ShortArrayLow, victim.Low);
        }


        [TestMethod]
        public void Test_CalculationRigthLong()
        {
            var retarray = victim.GetCheckedArray(Crc16TestData.LongCleanArray);
            Assert.AreEqual(Crc16TestData.LongArrayHi, victim.Hi);
            Assert.AreEqual(Crc16TestData.LongArrayLow, victim.Low);
        }

        [TestMethod]
        public void Test_CheckedArrayLength()
        {
            var testarray = new Byte[100];

            var retarray = victim.GetCheckedArray(testarray);

            Assert.AreEqual(testarray.Length + 2, retarray.Length);
        }



        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestNullAsParametr()
        {
            Byte[] nullarray = null;

            var retval = victim.GetCheckedArray(nullarray);
        }

        [TestMethod()]
        public void Test_GetUncheckedArrayRightLength()
        {
            Byte[] retArray;

            var res = victim.GetUncheckedArray(new Byte[0], out retArray);
            Assert.AreEqual(0, retArray.Length);

            res = victim.GetUncheckedArray(new Byte[2], out retArray);
            Assert.AreEqual(0, retArray.Length);

            res = victim.GetUncheckedArray(new Byte[3], out retArray);
            Assert.AreEqual(1, retArray.Length);
        }


        [TestMethod()]
        [ExpectedException(typeof(NullReferenceException))]
        public void Test_GetUncheckedArrayNull()
        {
            Byte[] retArray;
            var res = victim.GetUncheckedArray(null, out retArray);

            Assert.AreEqual(0, retArray.Length);
        }

        [TestMethod()]
        public void Test_GetUncheckedArrayOk()
        {
            Byte[] retArray;

            var res = victim.GetUncheckedArray(Crc16TestData.ShortPacked, out retArray);

            Assert.AreEqual(true, res);
            Assert.AreEqual(Crc16TestData.ShortPacked.Length - 2, retArray.Length);
        }

        [TestMethod()]
        public void Test_GetUncheckedArrayOkLongArray()
        {
            Byte[] retArray;

            var res = victim.GetUncheckedArray(Crc16TestData.LongPacked, out retArray);

            Assert.AreEqual(true, res);
            Assert.AreEqual(Crc16TestData.LongPacked.Length - 2, retArray.Length);
        }

    }
}