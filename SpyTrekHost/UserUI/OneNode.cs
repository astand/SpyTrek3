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
            node_.SetListUpdater(AddNotesToGridView);
            node_.SetInfoUpdater(AddInfoToView);
            InitializeComponent();
        }

        private void btnInfo_Click(Object sender, EventArgs e)
        {
            node_.Pipe.SendData(new ReadRequest(FiledID.Info));
        }

        private void button3_Click(Object sender, EventArgs e)
        {
            node_.Pipe.SendData(new ReadRequest(FiledID.Filenotes));
        }

        private void button4_Click(Object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void button1_Click(Object sender, EventArgs e)
        {
            node_.ReadTrekCmd(0);
        }
    }
}
