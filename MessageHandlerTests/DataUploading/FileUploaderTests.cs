using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageHandler.DataUploading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.DataUploading.Tests
{
    [TestClass()]
    public class FileUploaderTests
    {
        FileUploader fu;
        [TestInitialize()]
        public void Initialize()
        {
            fu = new FileUploader(@"d:\wideprot.txt");
            fu.RefreshData();
        }

        [TestMethod()]
        public void FileUploader_FileUploader_Test()
        {
            var failuploader = new FileUploader(@"");
            bool ret = failuploader.RefreshData();
            Assert.AreEqual(false, ret);

            failuploader = new FileUploader(@"a;sdflasd;fh");
            ret = failuploader.RefreshData();
            Assert.AreEqual(false, ret);
        }

        [TestMethod()]
        public void FileUploader_ReadData_Test()
        {
            var input_array = new Byte[200];

            for(int i = 0; i < 100; i++)
            {
                Int32 ret = fu.ReadData(input_array, i * 200, 200);
                
            }
        }

        [TestMethod()]
        public void FileUploader_RefreshData_Test()
        {
            bool is_refresh = fu.RefreshData();
            Assert.AreEqual(is_refresh, true);
        }
    }
}