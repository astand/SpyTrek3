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

namespace spytrek2
{
    public partial class Form1 : Form
    {
        static MasterSideTftp sideundercontrol;
        //string listmessage;
        enum MainState
        {
            IDLE,
            NOTELIST,
            TRACK,
            INFO,
            command,
            data,
            error
        };
        //static g2route blkontrol = new g2route();
        //static MainState mState = MainState.IDLE;
        static IPAddress myIP = null;
        static NetworkStream PRStream = null;

        static TcpClient PRClient = null;
        //static G2_Client cl1 = new G2_Client();

        static TcpListener PRListen = null;

        //static int blockCommSize = 0;
        static bool socketalive = false;


        private void OpenSocket(object ob)
        {
            int inner_port = Convert.ToUInt16(ob);
            LogUI(" > Listening: " + inner_port.ToString());
            debugcc.dbgInfo("Start listen port: " + inner_port.ToString());
            PRListen = new TcpListener(IPAddress.Any, inner_port);
            PRListen.Start();
            while (true)
            {
                try
                {
                    PRClient = PRListen.AcceptTcpClient();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[socket] open error: " + ex.Message);
                    return;
                }



                LogUI(" > Connected!");
                StatusUI("Connected!");
                start2Thread(PRClient, 0);
                socketalive = true;


            }
            //PRStream = PRClient.GetStream();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="insocket"></param>
        /// <param name="id"></param>
        public void start2Thread(TcpClient insocket, int id)
        {
            Thread clThr = new Thread(new ParameterizedThreadStart(handle2Thread));
            clThr.Start(insocket);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="insocket"></param>
        public void handle2Thread(object insocket)
        {
            MasterSideTftp cl = new MasterSideTftp(Properties.Settings.Default.TIMEECHO,
                                             Properties.Settings.Default.TIMEWAIT,
                                             Properties.Settings.Default.savepath);

            cl.startG2Client((TcpClient)insocket, 0);
            sideundercontrol = cl;
            cl.UpdateInfoDelegate(new Action<SpyTrekInfo>(FullInformationUpdate));
            cl.UpdateStatusDelegate(new Action<string>(StatusUI));
            cl.UpdateListDelegate(UIList);
            cl.UpdateGrid(GridUpdate);
            cl.proccessMain();
            cl.Dispose();

            debugcc.dbgDebug(" Handle Thread End here!");
        }

    }

}
