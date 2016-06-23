using System;
using System.Linq;
using System.IO;


namespace ProtSys
{
  /// <summary>
  /// 
  /// </summary>
  class TrekFileFolder
  {

    string main_dst_file = "";

    string template_path = @"sys/temp";

    const string  trekextension = ".html";

    public string PATH { get { return main_dst_file; } }

    internal MotherDirectory dir_tree;

    static private Int32 MAX_SIZE {
      get { return 500; }
    }

    Int32[] topic_size = new Int32[MAX_SIZE];

    public TrekFileFolder(string s)
    {
      dir_tree = new MotherDirectory(s);
      
    }


    private Int32 getSavedTopicSize(Int32 index)
    {
      Int32 filesize = 0;

      try
      {
        filesize = topic_size[index];
      }
      catch (IndexOutOfRangeException)
      {
        return 0;
      }

      return filesize;
    }


    public bool PrepareDirectory(MatrixItem onetopic,  string im, Int32 topic_id = -1)
    {
      if (!BuildFullFileNameChain(onetopic, im))
        return false;

      if (getSavedTopicSize(topic_id) == CurrentFileSize())
      {
        debugcc.dbgDebug(" PrepareDirectory: trek size is same");
        return false;
      }

      Int32 count_of_navi_file = getCountOfNaviNotes();
      Int32 count_of_navi_trek = onetopic.getNumsOfNaviNotes();

      if (count_of_navi_file == count_of_navi_trek)
      {
        topic_size[topic_id] = (Int32)new FileInfo(main_dst_file).Length;

        debugcc.dbgDebug(" PrepareDirectory: trek already created");
        return false;
      }

      /* no file or file not full */
      File.Delete(main_dst_file);

      /* prepare template */
      try
      {
        File.Copy(template_path, main_dst_file);
      }
      catch (Exception ex)
      {
        debugcc.dbgError(" Error during copy template: " + ex.ToString());
        return false;
      }
      return true;
    }

    /// <summary>
    /// Initialize main_dst_file by full path to file
    /// </summary>
    /// <param name="onetopic"></param>
    /// <param name="im"></param>
    private bool BuildFullFileNameChain(MatrixItem onetopic, string im)
    {
      main_dst_file = null;
      /* prepare directory */
      try
      {
        string topic_name = onetopic.GetTrekDir();
      }
      catch (NullReferenceException)
      {
        debugcc.dbgWarn(" MI are null. Cannot build file name. MI = " + onetopic.id.ToString());
        return false;
      }

      try
      {
        main_dst_file = dir_tree.UpdateAndCreateDir(im, onetopic.GetTrekDir());
      }
      catch (NullReferenceException ex)
      {
        /* obj may be null. it should be checked */
        debugcc.dbgError(" Error in trying repare directory: " + ex.ToString());
        return false;
      }
      /* prepare file name */
      main_dst_file = TrackUnconflictName(onetopic.GetTrekName(), main_dst_file, true);

      return true;
    }


    public Int32 AddOneNaviNote(navinote navi)
    {
      if (main_dst_file == null) return -1;

      var fileContents = File.ReadAllText(main_dst_file);
      fileContents = fileContents.Replace("];/*replace*/", navi.GetStringNotify() + "\n];/*replace*/");
      File.WriteAllText(main_dst_file, fileContents);

      return 0;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    private Int32 TrackDirCreate(string dir)
    {
      /* check existing of directory */
      if (Directory.Exists(dir))
        return 0;

      try
      {
        Directory.CreateDirectory(dir);
      }
      catch (Exception ex)
      {
        debugcc.dbgWarn(" can't create directory for track: " + ex.ToString());
        return -1;
      }

      return 0;
    }


    /// <summary>
    /// Create full Path for trek
    /// </summary>
    /// <param name="fname">Name for new trek (without extension)</param>
    /// <param name="directory">Full directory path</param>
    /// <param name="rewritetrek">if true - existing trek will overwrite</param>
    /// <returns></returns>
    private string TrackUnconflictName(string fname, string directoryPath, bool rewritetrek = true)
    {
      Int32 findex = 1;

      var alldirtreks = new DirectoryInfo(directoryPath);

      string mask_for_clean = fname.Substring(0, 12) + "*";

      foreach (var delfil in alldirtreks.EnumerateFiles(mask_for_clean))
      {
        if (delfil.Name != fname + trekextension)
          delfil.Delete();
      }

      fname = directoryPath + fname;

      if (!File.Exists(fname + trekextension) || rewritetrek)
      {
        return (fname + trekextension);
      }

      while (File.Exists(fname + "_(" + findex + ")" + trekextension))
      {
        findex++;
      }

      return (fname + "_(" + findex + ")" + trekextension);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    internal Int32 getCountOfNaviNotes()
    {
      var lineCount = 0;

      if (!File.Exists(main_dst_file))
      {
        return 0;
      }
      try
      {
        using (var reader = File.OpenText(main_dst_file))
        {
          do
          {
            if (reader.ReadLine().Contains(",titl:"))
              lineCount++;
          } while (!reader.EndOfStream);
        }
      }
      catch (Exception)
      {
        File.Delete(main_dst_file);
        return 0;
      }
      return lineCount;
    }


    /// <summary>
    /// Check exsisting file size. If error occured return -1 that means no file
    /// </summary>
    /// <returns></returns>
    private Int32 CurrentFileSize()
    {
      try
      {
        return (Int32)(new FileInfo(main_dst_file)).Length;
      }
      catch (FileNotFoundException)
      {
        return -1;
      }
      catch (Exception)
      {
        return -2;
      }
    }
  };



  /* ------------------------------------------------------------------------- */
  class MotherDirectory
  {
    static readonly string help_path = @"uncorrect_conf_dir\";
    readonly string _path;

    internal MotherDirectory(string s)
    {
      if (s != "")
        _path = InsertEndSlesh(s);
    }


    internal string UpdateAndCreateDir(string imei, string dir)
    {
      imei = InsertEndSlesh(imei);
      string ret = _path + imei + dir;

      if (SecureDirectoryCreator(ret) == false)
      {
        ret = help_path + imei + dir;
        SecureDirectoryCreator(ret);
      }
      ret = InsertEndSlesh(ret);
      return ret;
    }


    private string InsertEndSlesh(string s)
    {
      if (s.EndsWith(@"\") || s.EndsWith(@"/"))
        return s;
      return s + @"\";

    }

    bool SecureDirectoryCreator(string s)
    {
      if (Directory.Exists(s)) return true;
      try
      {
        Directory.CreateDirectory(s);
      }
      catch (Exception ex)
      {
        debugcc.dbgError(" Directory cannot create: " + ex.ToString());
        return false;
      }
      return true;
    }

  }

}
