using Microsoft.VisualStudio.TestTools.UnitTesting;
using StreamHandler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamHandler.Tests
{
    [TestClass()]
    public class BlockDriverTests
    {
        [TestMethod()]
        public void BlockDriver_PassAckBlock_Test()
        {
            BlockDriver blockDriver = new BlockDriver();

            UInt16[] bids = { 0, 1, 2, 3, 4, 5, 6};

            foreach(var bid in bids)
            {
                bool ret = blockDriver.PassAckBlock(bid);
                Debug.WriteLine($"[{bid}] : ret = {ret}");
                Assert.AreEqual((bid == 0) ? false : true, ret);

            }
        }
    }
}