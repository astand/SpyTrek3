using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtSys
{
  /// <summary>
  /// Text information about module
  /// </summary>
  public class SpyTrekInfo
  {
    private string m_iMei;
    private string m_version;
    private string m_name;

    public string Imei
    {
      get { return m_iMei ?? "Undefined"; }
      private set { m_iMei = value; }
    }

    public bool TrueInfo
    {
      get { return Imei == "Undefined" ? false : true; }
    }

    public string Version
    {
      get { return m_version; }
      internal set { m_version = value; }
    }

    public string Name
    {
      get { return m_name; }
      internal set { m_name = value; }
    }

    /// <summary>
    /// search in string dedicated inforamtion
    /// </summary>
    /// <param name="s">one of existing substrings</param>
    private void getField(string s)
    {
      if (s.Contains("IMEI=")) { Imei = s.Substring(s.IndexOf("=") + 1); }

      else if (s.Contains("VERSION=")) { Version = s.Substring(s.IndexOf("=") + 1); }

      else if (s.Contains("NAME=")) { Name = s.Substring(s.IndexOf("=") + 1); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="s"></param>
    internal void TryParse(string s)
    {
      string[] vic = s.Split(new char[] { ',' });

      foreach (var subvic in vic)
      {
        getField(subvic);
      }
    }

    public override string ToString()
    {
      string ostr;
      ostr = String.Format("Imei = {0}, Version = {1}, Name = {2}",
        Imei, Version, Name);

      return ostr;

    }
  } // class
}
