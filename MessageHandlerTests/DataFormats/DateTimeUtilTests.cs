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
    public class DateTimeUtilTests
    {
        Byte[] short_array = { 10, 4, 4, 12, 12 };
        Byte[] wrong_param_array = { 10, 4, 4, 99, 90, 00 };
        Byte[] okdate = { 10, 4, 4, 12, 12, 00 };

        DateTime victim;
        [TestInitialize()]
        public void Init()
        {
            victim = DateTimeUtil.GetDateTime(okdate);
        }

        [ExpectedException(typeof(IndexOutOfRangeException))]
        [TestMethod()]
        public void DateTimeFromByteArray_ShortArray()
        {
            DateTime dt = DateTimeUtil.GetDateTime(short_array, 0);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod()]
        public void DateTimeFromByteArray_WrongArray()
        {
            DateTime dt2 = DateTimeUtil.GetDateTime(wrong_param_array, 0);
        }

        [TestMethod()]
        public void DateTimeFromByteArray_Ok()
        {
            DateTime dt2 = DateTimeUtil.GetDateTime(okdate, 0);
        }



        [TestMethod()]
        public void DateTimeFromByteArray_ToFrmString_Test()
        {
            Assert.AreEqual(2010, victim.Year);
            Debug.WriteLine(victim.ToDirectory());
            Debug.WriteLine(victim.ToJS());
        }
    }
}