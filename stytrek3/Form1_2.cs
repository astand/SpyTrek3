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
  public partial class Form1 : Form
  {
    /// <summary>
    /// Function send string to ListBox
    /// </summary>
    /// <param name="s">printing string</param>
    private void LogUI(string s)
    {

      Func<int> del = delegate()
      {
        listBox1.Items.Add(DateTime.Now.ToLongTimeString() + s + "\n");
        Debug.WriteLine(s);
        listBox1.SelectedIndex = listBox1.Items.Count - 1;
        listBox1.SelectedIndex = -1;
        return 0;
      };
      Invoke(del);
    }


    /// <summary>
    /// Put one Grid note on form
    /// </summary>
    /// <param name="o">Item of Matrix_item</param>
    private void GridUI(MatrixItem o)
    {
      Func<int> del = delegate()
      {
        float indist = 0;
        indist = (o.localmileage / 10000);
        UInt32 allmileage = (o.mileage / 10000);
        dataGridView1.Rows.Add(o.id, o.MPrintTime(), o.size, indist, allmileage);
        //dataGridView1.Rows.Add(o.id, o.GetStringTime(), o.size, o.kmdist);
        Debug.WriteLine(dataGridView1.Rows);
        return 0;
      };
      del();
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    private void StatusUI(string s)
    {

      Func<int> del = delegate()
      {
        lStatus.Text = s;
        return 0;
      };
      Invoke(del);
    }


    /// <summary>
    /// 
    /// </summary>
    private void CloseUI()
    {
      Func<int> del = delegate()
      {
        bDisconnect_click(this, null);

        return 0;
      };
      Invoke(del);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    public void FillLog(string s)
    {
      Debug.WriteLine(s);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    private void ImeiUpdate(string s)
    {
      Action<string> _printimei = delegate(string vic)
      {
        if (vic != null)
        {
          lab_Version.Text = vic;
        }
      };
      _printimei.Invoke(":delegate test:" + s);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="inst"></param>
    private void FullInformationUpdate(ProtSys.SpyTrekInfo inst)
    {
      if (this.InvokeRequired)
      {
        Action<SpyTrekInfo> del = new Action<SpyTrekInfo>(this.FullInformationUpdate);
        this.Invoke(del, new object[] { inst });
      }
      else
      {
        lab_Imei.Text = (inst.TrueInfo) ? inst.Imei : ("Undefinied");
        lab_Version.Text = (inst.Version) ?? ("Undefinied");
        lab_Name.Text = (inst.Name) ?? ("Undefinied");
      }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    private void UIStatus(string s)
    {
      lStatus.Text = s;
    }






    public delegate void GUIList(string text);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nix"></param>
    private void GridUpdate(MatrixItem nix)
    {
      if (this.InvokeRequired)
      {
        Action<MatrixItem> del = new Action<MatrixItem>(this.GridUpdate);
        this.Invoke(del, new object[] { nix });
      }
      else
      {
        float indist = 0;
        indist = (nix.localmileage / 10000);
        UInt32 allmileage = (nix.mileage / 10000);
        dataGridView1.Rows.Add(nix.id, nix.MPrintTime(), nix.size, indist, allmileage);
        //dataGridView1.Rows.Add(o.id, o.GetStringTime(), o.size, o.kmdist);
        debugcc.dbgTrace(dataGridView1.Rows.ToString());
      }
    }


    private void UIList(string s)
    {
      if (this.InvokeRequired)
      {
        Action<string> dellist = new Action<string>(this.UIList);
        this.Invoke(dellist, new object[] { s });
      }
      else
      {
        listBox1.Items.Add(DateTime.Now.ToLongTimeString() + " " + s + "\n");
        listBox1.SelectedIndex = listBox1.Items.Count - 1;
        listBox1.SelectedIndex = -1;
      }
    }

  
  }

}
