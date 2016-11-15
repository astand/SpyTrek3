using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandlerTests.DiffTests
{
    [TestClass]
    public class StringFormatTests
    {
        [TestMethod]
        public void InfoProcessor_TestNotify()
        {
            int start_size = 0;
            int full_size = 50000;

            do
            {
                var percent = start_size * 100.0 / full_size;

                Debug.WriteLine($"({percent:F1} %)  {start_size} / {full_size} ... ");
                start_size += 1221;

            } while (start_size < full_size);
        }
    }

}
