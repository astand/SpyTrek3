using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
//using System.Windows.Forms;
using System.Threading;
using ProtSys;
using System.Diagnostics;


//using System.Collections.Generic;

namespace spytrek2
{

    //public delegate void GridAbsUpdate(Matrix_item nix);

    public partial class Form1 : Form
    {
        //string ROOT_DIR = Properties.Settings.Default.savepath;

        public Form1()
        {
            InitializeComponent();
            //string set = ConfigurationSettings.AppSettings["port"];
            //numericUpDown1.Value = 1;
            numericUpDown1.Value = Properties.Settings.Default.defport;

            debugcc.changeLevels(
              Properties.Settings.Default.sw_error,
              Properties.Settings.Default.sw_warn,
              Properties.Settings.Default.sw_info,
              Properties.Settings.Default.sw_debug,
              Properties.Settings.Default.sw_trace);

            debugcc.changeOutDestination(
              Properties.Settings.Default.sw_time,
              Properties.Settings.Default.sw_file);

            debugcc.dbgDebug("Hello");
            debugcc.dbgInfo("Hello");
            debugcc.dbgWarn("Hello");
            debugcc.dbgError("Hello");


        }


        public void LogTextBox(string str)
        {
            listBox1.Items.Add(str);
        }

        public void SetListItems(string str)
        {
            listBox1.Items.Add(str);
        }

        private IPAddress MainInit()
        {

            ////tMain = new Thread(new ThreadStart(ThreadMain));
            ////tMain.Start();
            //string myHostName = Dns.GetHostName();
            //IPAddress[] localAddrBuff = Dns.GetHostAddresses(myHostName);
            //myIP = localAddrBuff[1];
            myIP = Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);

            return myIP;
        }

        public string IpToLabel()
        {
            MainInit();
            return myIP.ToString();
        }

        public void StopSocket()
        {
            Thread tcpCloseSocketThread = new Thread(new ThreadStart(TcpSocketClose));
            tcpCloseSocketThread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        private void TcpSocketClose()
        {
            if (PRStream != null)
            {
                try
                {
                    PRStream.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

            if (PRListen != null)
            {
                try
                {
                    PRListen.Stop();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            if (PRClient != null)
            {
                try
                {
                    PRClient.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = IpToLabel();
            //dataGridView1.ContextMenuStrip = contextMenuStrip1;
            FillLog("\n");
            FillLog(" --------------------------------- ------------------------------- ");
            FillLog("                     GHaunt start application");
            FillLog(" --------------------------------- ------------------------------- ");
            FillLog("\n");

        }
        /* - */


        private void bGetFiles_Click(object sender, EventArgs e)
        {
            if (!socketalive) return;
            dataGridView1.Rows.Clear();
            sideundercontrol.SendMachine.SetStFile(TfBase.ID_FILENOTES);
        }
        private void bExit_Click(object sender, EventArgs e)
        {
            StopSocket();
            Application.Exit();
        }

        private void bClrLB_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }


        private void bDisconnect_click(object sender, EventArgs e)
        {
            bOpen.Enabled = true;
            bDisconnect.Enabled = false;
            StopSocket();
            StatusUI("Not connected!");
            LogUI(" > Connection closed");
            FillLog("[tcp]::connection closed!");
        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            StopSocket();
            Environment.Exit(0);
        }
        private void bConnect_Click(object sender, EventArgs e)
        {
            bOpen.Enabled = false;
            bDisconnect.Enabled = true;
            StatusUI("Wait connection...");
            Thread tcpServerOpenThread = new Thread(new ParameterizedThreadStart(OpenSocket));
            tcpServerOpenThread.Start(numericUpDown1.Value);
        }

        public void ThreadMain()
        {
            Thread.Sleep(100);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (!socketalive) return;
            sideundercontrol.SendMachine.SetStFile(TfBase.ID_FIRMWARE);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!socketalive) return;
            sideundercontrol.SendMachine.SetStFile(TfBase.ID_INFO);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!socketalive) return;
            sideundercontrol.SendMachine.SetStFile(TfBase.ID_ECHO);
        }
        private void bTestTrackLoad_Click(object sender, EventArgs e)
        {
            bool datanull = (dataGridView1.CurrentCell == null || sideundercontrol == null);

            if (!socketalive || datanull) return;

            Int32 listindex = dataGridView1.CurrentCell.RowIndex;
            //UInt16 dat = (UInt16)dataGridView1.Rows[i].Cells[0].Value;
            Int32 ret = sideundercontrol.RequestTrekByNum(listindex);

            if (ret < 0)
            {
                dataGridView1.Rows[listindex].DefaultCellStyle.BackColor = Color.Red;
            }
            //else if (ret == -1)
            //{
            //  active_row.DefaultCellStyle.BackColor = Color.Pink;
            //}

        }
    }


}
