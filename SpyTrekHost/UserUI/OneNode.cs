using MessageHandler;
using MessageHandler.Rig;
using MessageHandler.Rig.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpyTrekHost.UserUI
{
    public partial class OneNodeForm : Form
    {

        HandleInstance node_;

        Timer rateTime = new Timer();

        public OneNodeForm(HandleInstance handleInstance)
        {
            node_ = handleInstance;
            node_.SetListUpdater(AddNotesToGridView);
            node_.SetInfoUpdater(AddInfoToView);
            node_.SetTrekUpdater(Label2Updater);
            InitializeComponent();
            rateTime.Interval = 500;
            rateTime.Start();
            rateTime.Tick += RateTime_Tick;
            labRate.Text = String.Empty;
        }

        private void RateTime_Tick(Object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                var del = new Action<Object, EventArgs>(RateTime_Tick);
                Invoke(del, new object[] { sender, e });
            }
            else
            {
                var upload = node_.Pipe.UploadRate.Rate / 500.0;
                var download = node_.Pipe.DownloadRate.Rate / 500.0;
                labRate.Text = $"UP {upload:F1} .DOWN {download:F1}";
            }
        }

        private void btnInfo_Click(Object sender, EventArgs e)
        {
            //node_.Pipe.SendData(new ReadRequest(FiledID.Info));
            node_.Pipe.SendData(new RigRrqFrame(OpID.Info));
        }

        private void button3_Click(Object sender, EventArgs e)
        {
            node_.Pipe.SendData(new RigRrqFrame(OpID.TrekList));
        }

        private void button4_Click(Object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void button1_Click(Object sender, EventArgs e)
        {
            var fid = GetTrekIdFromGrid();
            /// for coloring grid row that cannot be downloaded.
            //Int32 listindex = dataGridView1.CurrentCell.RowIndex;
            //UInt16 dat = (UInt16)dataGridView1.Rows[i].Cells[0].Value;
            //if (ret < 0)
            //{
            //    dataGridView1.Rows[listindex].DefaultCellStyle.BackColor = Color.Red;
            //}
            Int32 retval = node_.ReadTrekCmd(fid);
            label2.Text = (retval < 0) ? $"File ({fid}) cannot be downloaded [{retval}]" : "Send trek request";
        }

        private Int32 GetTrekIdFromGrid()
        {
            Int32 retindex = -1;

            try
            {
                retindex = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            }
            catch (Exception)
            {
                retindex = -2;
            }

            return retindex;
        }
    }
}
