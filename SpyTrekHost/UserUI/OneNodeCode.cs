using MessageHandler.DataFormats;
using MessageHandler.Notifiers;
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


        public void AddNotesToGridView(List<TrekDescriptor> list, bool isNew)
        {
            if (InvokeRequired)
            {
                var del = new Action<List<TrekDescriptor>, bool>(AddNotesToGridView);
                Invoke(del, new object[] { list, isNew });
            }
            else
            {
                AddDescriptorToGrid(list, isNew);
            }
        }

        public void AddInfoToView(SpyTrekInfo info)
        {
            if (InvokeRequired)
            {
                var del = new Action<SpyTrekInfo>(AddInfoToView);
                Invoke(del, new object[] { info });
            }
            else
            {
                if (info == null)
                    return;

                lblImei.Text = info.Imei;
                lblName.Text = info.Name;
                lblVer.Text = info.Version;
            }
        }

        private void AddDescriptorToGrid(List<TrekDescriptor> list, bool need_clear)
        {
            /// This try block is needed for non-exception dataGrid row adding after first OneNode window closing
            /// It throws InvalidOperationException - no columns in dataGrid
            try
            {
                dataGridView1.Rows.Clear();

                foreach (var item in list)
                {
                    var dist_str = (item.Dist < 10) ? (item.Dist.ToString("0.0")) : (item.Dist.ToString("F0"));
                    dataGridView1.Rows.Add(item.Id, item.TrekTime(), item.TrekDuration(),
                                           item.GetAvrSpd.ToString("000.00"), item.TrekSize,
                                           dist_str, item.Odometr.ToString("F0"));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error when notes adding. {e.GetType()}. {e.Message}");
            }
        }

        private void Label2Updater(String str)
        {
            if (InvokeRequired)
            {
                var del = new Action<string>(Label2Updater);
                Invoke(del, new object[] { str });
            }
            else
            {
                label2.Text = str;
            }
        }

        private void btnFirmware_Click(Object sender, EventArgs e)
        {
            node_.StartFirmwareUpdating();
        }
    }
}
