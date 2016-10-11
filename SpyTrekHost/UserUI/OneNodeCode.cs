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
        public void AddNotesToGridView(Object obj, NoteListEventArgs e)
        {
            foreach (var item in e.NoteList)
            {
                float indist = 0;
                indist = (item.Dist / 10000);
                UInt32 allmileage = (item.Odometr/ 10000);
                dataGridView1.Rows.Add(item.Id, item.ToString(), item.TrekSize, indist, allmileage);
            }
        }
    }
}
