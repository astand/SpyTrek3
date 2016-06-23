using System;
using System.IO;


namespace ProtSys
{

  internal class FileUpload
  {

    private string _fpath;

    private long fsize;

    //private long fsend;

    public FileUpload(string rootpath) { _fpath = rootpath; }

    internal void Begin()
    {
      //fsend = 0; 
      fsize = 0;
      if (File.Exists(_fpath))
      {
        fsize = new FileInfo(_fpath).Length;
      }
    }

    internal bool IsDataAvail(UInt16 offset, UInt32 blocksize)
    {
      return ((fsize) > ((offset) * blocksize));
    }

    internal int TryReadFromFile(UInt16 offset, UInt32 blocksize, byte[] dst)
    {
      int ret = -1;
      try
      {
        using (FileStream fs = new FileStream(_fpath, FileMode.Open, FileAccess.Read))
        {
          {
            fs.Position = offset * blocksize;
            ret = fs.Read(dst, (int)0, (int)blocksize);
            debugcc.dbgTrace(" Reading from file offset: " + offset
              + ", data size: " + ret);
          }
        }
      }
      catch (FileNotFoundException e)
      {
        debugcc.dbgError(" File out not founded: " + e.Message);
        ret = 0;
      }
      finally
      {

      }
      return ret;
    }


    internal Int32 Lenght()
    {
      Int32 ret;

      try
      {
        ret = (Int32)(new FileInfo(_fpath).Length);
      }
      catch (FileNotFoundException e)
      {
        debugcc.dbgError(" File out not founded: " + e.Message);
        ret = 0;
      }
      return ret;
    }
  }
}
