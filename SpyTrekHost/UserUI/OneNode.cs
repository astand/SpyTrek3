using MessageHandler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpyTrekHost.UserUI
{
    public partial class OneNodeForm : Form
    {

        private HandleInstance node_;

        public OneNodeForm(HandleInstance handleInstance)
        {
            node_ = handleInstance;
            InitializeComponent();
        }

        private void btnInfo_Click(Object sender, EventArgs e)
        {
            var info = node_.Info;

            lblImei.Text = info.Imei;

            lblName.Text = info.Name;

            lblVer.Text = info.Version;
        }

        private void button3_Click(Object sender, EventArgs e)
        {
            node_.Pipe.SendData(new ReadRequest(FiledID.Filenotes));
        }
    }
}
