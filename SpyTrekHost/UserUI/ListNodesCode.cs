﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpyTrekHost.UserUI
{
    public partial class ListNodesForm : Form
    {
        private void btnRefresh_Click(Object sender, EventArgs e)
        {
            UpdateListNodes();
        }


        public void UpdateListNodes()
        {
            if (InvokeRequired)
            {
                var del = new Action(UpdateListNodes);
                Invoke(del, new object[] { });

            }
            else
            {
                UpdateGridView();
            }
        }

        private void UpdateGridView()
        {
            dataGridView1.Rows.Clear();
            Int32 num = 0;
            //lock (HICollection.List)
            //{
                var collection = HICollection.List;
                foreach (var item in collection)
                {
                    var index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells[0].Value = ++num;
                    dataGridView1.Rows[index].Cells[1].Value = item.ToString();
                    dataGridView1.Rows[index].Cells[2].Value = item.Connected.ToString("HH:mm:ss.ff");
                }
            //}
        }
    }
}
