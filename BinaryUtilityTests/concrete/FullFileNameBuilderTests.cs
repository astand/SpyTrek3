using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtSys.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtSys;
using ProtSys.Abstract;

namespace ProtSys.Concrete.Tests
{
    [TestClass()]
    public class FullFileNameBuilderTests
    {
        [TestMethod()]
        public void NameBuildTest()
        {
            byte[] array = { 128, 0,
                            15, 10, 12, 12, 00, 00,
                            15, 10, 12, 12, 10, 10,
                            0,  0,  0,  90,
                            10, 12, 0,  0,
                            10, 99, 0,  0};
            Int32 offset = 0;
            MatrixItem test0 = MatrixItem.Factory(array, ref offset);
            ITrekNameFormatter frm = new FullFileNameBuilder();
            var retval = frm.NameBuild(test0);

            //Assert.Fail();
        }
    }
}