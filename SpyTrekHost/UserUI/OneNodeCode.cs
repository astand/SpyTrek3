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

        private void AddDescriptorToGrid(TrekDescriptor dsc)
        {
            float indist = 0;
            indist = (dsc.Dist / 10000);
            UInt32 allmileage = (dsc.Odometr/ 10000);
            var avr_speed = (dsc.Dist / 10000) / dsc.Duration.TotalHours;
            dataGridView1.Rows.Add(dsc.Id, dsc.TrekTime(), dsc.TrekDuration(), avr_speed.ToString("000.00"), dsc.TrekSize, indist, allmileage);
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
