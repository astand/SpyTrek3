using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrekTreeService.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TrekTreeService.Concrete.Tests
{
    [TestClass()]
    public class TrekDetailsTests
    {
        [TestMethod()]
        public void UpdateOkTest()
        {
            IEnumerable<string> files = new List<string>
            {
                "201604161600_201604161652_00032_06245",
                "201604161450_201604161539_00014_06213",
                "201604161402_201604161414_00003_06199",
                "201604161329_201604161335_00002_06196",
                "201604161256_201604161259_00001_06193"
            };

            TrekDetails inst = TrekDetails.AverageDetailsForFiles(files);
            Debug.WriteLine($"detail: speed = {inst.LocalDistance} distance = {inst.Duration.TotalHours}");
            Debug.WriteLine($"speed {inst.LocalDistance / inst.Duration.TotalHours:F2}");
            Assert.AreEqual<int>(52, inst.LocalDistance);
            //Assert.AreEqual<double>(12, details.LocalDistance / details.Duration.TotalHours);
        }

        [TestMethod()]
        public void ParseFilesOk()
        {
            IEnumerable<string> files = new List<string>
            {
                "201604161600_201604161652_00032_06245",
                "201604161450_201604161539_00014_06213",
                "201604161402_201604161414_00003_06199",
                "201604161329_201604161335_00002_06196"
            };

            var ret = TrekDetails.AverageDetailsForFiles(files);
            Assert.AreEqual<Int32>(51, ret.LocalDistance);
        }

        [TestMethod()]
        public void ParseFilesWithWrongNames()
        {
            IEnumerable<string> files = new List<string>
            {
                "20160416asdsa_201604161652_00032_06245",
                "201604161450_2016039_00014_06213",
                "2011402_201604161414_00003_06199",
                "201604161335_201604161329_00002_06196",
                "000000000000_201604161259_00001_06193"
            };

            var ret = TrekDetails.AverageDetailsForFiles(files);
            Assert.AreEqual<Int32>(0, ret.LocalDistance);
        }
    }
}