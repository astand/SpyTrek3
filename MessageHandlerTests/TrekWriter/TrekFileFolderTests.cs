using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageHandler.TrekWriter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageHandler.DataFormats;
using MessageHandlerTests.TrekWriter;
using System.Diagnostics;

namespace MessageHandler.TrekWriter.Tests
{
    [TestClass()]
    public class TrekFileFolderTests
    {
        List<TrekDescriptor> testList = new List<TrekDescriptor>();

        TrekFileFolder builder = new TrekFileFolder("sdafasdf");

        [TestInitialize()]
        public void Init()
        {
            //testList.Add(new TrekDescriptor { Id = 0, Start = new DateTime(2016, 10, 9, 12, 12, 43), });
            Int32 current_offset = 0;
            bool parseOk;

            do
            {
                var item = new TrekDescriptor();
                parseOk = item.TryParse(TrekDescriptorArray.Array, current_offset);
                if (parseOk)
                {
                    testList.Add(item);
                    current_offset += TrekDescriptor.Length;
                }
            }
            while (parseOk);
        }


        [TestMethod()]
        public void TrekFileFolder_BuildFullFileNameChain_Test()
        {
            foreach (var item in testList)
            {
                string ret = builder.BuildFullFileNameChain(item, "9329829347");
                Debug.WriteLine(ret);
            }
        }
    }
}