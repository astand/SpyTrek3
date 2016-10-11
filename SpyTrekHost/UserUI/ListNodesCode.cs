using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpyTrekHost.UserUI
{
    public partial class ListNodesForm : Form
    {
        protected Func<List<HandleInstance>> delGetList;

        public void SetListGetter(Func<List<HandleInstance>> d)
        {
            delGetList = d;
        }


        private void btnRefresh_Click(Object sender, EventArgs e)
        {
            UpdateListNodes();
        }

        public void EnforceUpdating()
        {
            if (InvokeRequired)
            {
                //Action del = new Action(UpdateListNodes);
                //Invoke(del, new object[] { });
            }
            else
            {
                UpdateListNodes();
            }
        }

        private void UpdateListNodes()
        {
            dataGridView1.Rows.Clear();

            var tempList = delGetList?.Invoke();

            Int32 num = 0;

            lock (HICollection.List)
            {
                var collection = HICollection.List;
                foreach (var item in collection)
                {
                    var index = dataGridView1.Rows.Add();
                    dataGridView1.Rows[index].Cells[0].Value = ++num;
                    dataGridView1.Rows[index].Cells[1].Value = item.ToString();
                    dataGridView1.Rows[index].Cells[2].Value = item.Connected.ToString("HH:mm:ss.ff");
                }
            }
        }
    }
}
