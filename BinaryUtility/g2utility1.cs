using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;


namespace ProtSys
{

  /// <summary>
  /// 
  /// </summary>
  public class CommonFileSize
  {
    private Int32 _full_file_size;
    private Int32 _success_passed;

    internal Int32 LenghtFile
    {
      set { _full_file_size = (value > 0) ? (value) : (0); }
      get { return _full_file_size; }
    }

    internal Int32 Passed
    {
      get { return _success_passed; }
      private set { _success_passed = value; }
    }

    internal void New(Int32 fsize)
    {
      LenghtFile = fsize;
      Passed = 0;
    }

    internal Int32 AddPassing(Int32 bsize)
    {
      Passed += bsize;
      if (Passed > LenghtFile) Passed = LenghtFile;
      return (Passed);
    }

    public override string ToString()
    {
      //string message = String.Format(" Passed {0:6}b/{1:6}b", LenghtPassed, LenghtFile);
      return (String.Format(" Passed: {0,8}b / {1}b.", Passed, LenghtFile));
    }
  };

  /* ----------------------------------------------------------------------- */
  public enum TimCtrlStatus{ OPEN, BUSY, FINISHED };

  public sealed class TimControl : IDisposable
  {

    private System.Timers.Timer twait;

    private Int32 treloadms;

    private TimCtrlStatus tstate = TimCtrlStatus.OPEN;

    /// <summary>
    /// Set BASE value for relaod action
    /// </summary>
    /// <param name="basevalue"></param>
    public TimControl(Int32 basevalue)
    {
      treloadms = basevalue;
      twait = new System.Timers.Timer(basevalue);
      twait.Elapsed += TimerHandler;

      twait.Start();
    }


    internal Int32 NOW {  get { return (Int32)twait.Interval; } }


    internal bool ISBUSY { get { return (tstate == TimCtrlStatus.BUSY); } }


    internal bool ISREADY {  get { return (tstate == TimCtrlStatus.FINISHED); } }

    internal void Free() { tstate = TimCtrlStatus.OPEN; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="val">Period of waiting</param>
    /// <param name="waitfinish">if false then timer will be stop 
    /// and start again forcibly</param>
    /// <returns></returns>
    internal bool Load(Int32 val, bool waitfinish = false)
    {
      if (ISBUSY && waitfinish) return false;
      twait.Stop();
      tstate = TimCtrlStatus.BUSY;
      twait.Interval = val;
      twait.Start();
      return true;
    }

    internal void Stop()
    {
      twait.Stop();
      tstate = TimCtrlStatus.OPEN;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    private void TimerHandler(Object source, System.Timers.ElapsedEventArgs e)
    {
      tstate = (tstate == TimCtrlStatus.BUSY) ? 
          (TimCtrlStatus.FINISHED) : (tstate);
    }

    public void Dispose()
    {
      twait.Stop();
      twait.Elapsed -= TimerHandler;
      twait.Dispose();
    }
  };



}
