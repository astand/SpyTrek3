using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrekTreeService.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace TrekTreeService.Concrete.Tests
{
    [TestClass()]
    public class TrekFileProviderTests
    {
        TrekFileProvider testobj = new TrekFileProvider(@"c:\Dropbox\VS10\Projects\FD\spytrek2\bin\Debug\user\");
        private readonly string IMEI_OK = "865733022132245";

        [TestMethod()]
        public void GetSubDirectoriesOkTest()
        {
            //var collection = testobj.GetSubDirectories("123",2016,04,null);
            //foreach (var alone in collection)
            //{
            //    Debug.WriteLine(" alone : " + alone);
            //}
            ////Debug.WriteLine(" length of collection : " + collection.Length);
            //Assert.IsTrue(collection.Count != 0);
        }

        [TestMethod()]
        public void GetSubDirectoriesBadTest()
        {
            //var collection = testobj.GetSubDirectories("123123",2016,04,null);
            //foreach (var alone in collection)
            //{
            //    Debug.WriteLine(" alone : " + alone);
            //}
            ////Debug.WriteLine(" length of collection : " + collection.Length);
            //Assert.IsTrue(collection.Count == 0);
        }

        [TestMethod()]
        public void GetInfoOkTest()
        {
            var ret = testobj.GetInfo(IMEI_OK, 2016, null, null);
            Debug.WriteLine("Count of: " + ret.Count);
            Assert.IsTrue(ret.Count != 0);
        }

        [TestMethod()]
        public void GetInfoFileOkTest()
        {
            var ret = testobj.GetInfo(IMEI_OK, 2016, 03, 07);
            Debug.WriteLine("Count of: " + ret.Count);
            Debug.WriteLine(ret[ret.Count - 1].ToString());
            Assert.IsTrue(ret.Count != 0);
        }

        [TestMethod()]
        public void GetContentOkTest()
        {
            var retres = testobj.GetContent(IMEI_OK, 2016, 4, 24, "201604241513_201604241522_00003_06411");
            Assert.IsTrue(retres.Length > 100);
        }

        //[ExpectedException(typeof(Exception))]
        [TestMethod()]
        public void GetContentNullTest()
        {
            var retres = testobj.GetContent(IMEI_OK, 2016, 4, 24, "adsasd");
            Assert.IsTrue(retres == null);
        }

    }
}