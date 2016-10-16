using MessageHandler.DataFormats;
using MessageHandler.Notifiers;
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


        public void AddNotesToGridView(List<TrekDescriptor> list, bool isNew)
        {

            if (InvokeRequired)
            {
                var del = new Action<List<TrekDescriptor>, bool>(AddNotesToGridView);
                Invoke(del, new object[] { list, isNew });
            }
            else
            {
                dataGridView1.Rows.Clear();
                foreach (var item in list)
                {
                    AddDescriptorToGrid(item);
                }
            }


        }

        private void AddDescriptorToGrid(TrekDescriptor dsc)
        {
            float indist = 0;
            indist = (dsc.Dist / 10000);
            UInt32 allmileage = (dsc.Odometr/ 10000);
            dataGridView1.Rows.Add(dsc.Id, dsc.ToString(), dsc.TrekSize, indist, allmileage);
        }
    }
}
