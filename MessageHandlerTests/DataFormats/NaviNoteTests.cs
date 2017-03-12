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
    public class NaviNoteTests
    {
        NaviNote note = new NaviNote();


        [TestInitialize()]
        public void Init()
        {
            note.TryParse(new byte[] {
            17, 3, 12, 9, 21, 11, 0x41, 0x05,
            0xea, 0x4e, 0x53, 0x03,
            0x6f, 0x0d, 0x40, 0x02,
            0xc0, 0,
            0xf5, 0x5b, 0x15, 0x00,
            0x10, 0x20, 0x0, 0x0, 0x0, 0x0}, 0);
        }

        [TestMethod()]
        public void NaviNote_GetStringNotify_Test()
        {
            Debug.WriteLine(note.GetStringNotify());
            Assert.Fail();
        }
    }
}