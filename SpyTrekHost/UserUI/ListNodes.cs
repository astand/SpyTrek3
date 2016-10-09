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
    public partial class ListNodesForm : Form
    {
        public ListNodesForm()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(Object sender, DataGridViewCellEventArgs e)
        {
            string msg = $"Index {e.RowIndex}. info: {listOfNodes[e.RowIndex]?.ToString()}";

            MessageBox.Show(msg);
        }
    }
}
