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
                Action del = new Action(UpdateListNodes);
                Invoke(del, new object[] { });
            }
            else
            {
                UpdateListNodes();
            }
        }

        private void UpdateListNodes()
        {
            dataGridView1.Rows.Clear();

            var list = delGetList?.Invoke();

            foreach (var item in list)
            {
                dataGridView1.Rows.Add(item.ToString());
            }
        }
    }
}
