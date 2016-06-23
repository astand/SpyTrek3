using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtSys
{
  class AutoRotateCommands
  {
    private UInt16[] m_cmds = {TfBase.ID_INFO, TfBase.ID_FILENOTES, TfBase.ID_TRACK, TfBase.ID_NONE };

    private Int32 actual_indx = 0;

    System.Timers.Timer recall_auto;

    bool cmd_send_enable;

    internal AutoRotateCommands()
    {
      recall_auto = new System.Timers.Timer(15 * 60 * 1000);
      recall_auto.Elapsed += Recall_auto_Elapsed;
      cmd_send_enable = true;
    }

    public Int32 INDEX
    {
      get { return actual_indx; } 
      private set
      {
        if (value < m_cmds.Length && value >= 0)
          actual_indx = value;
      }
    }


    /// <summary>
    /// Getting current command in chain. If index out of chain return ID_NONE
    /// </summary>
    /// <returns>Actual CMD or ID_NONE</returns>
    internal UInt16 getActualCommand()
    {
      if (INDEX + 1 == m_cmds.Length)
      {
        if (cmd_send_enable)
        {
          cmd_send_enable = false;
          recall_auto.Start();
        }
      }

      return m_cmds[INDEX];
    }

    private void Recall_auto_Elapsed(Object sender, System.Timers.ElapsedEventArgs e)
    {
      recall_auto.Stop();
      INDEX = 0;
      cmd_send_enable = true;
    }


    /// <summary>
    /// 
    /// </summary>
    internal void StepForwardCmd()
    {
      INDEX++;
    }


    /// <summary>
    /// 
    /// </summary>
    internal void ResetCommandChain()
    {
      actual_indx = 0;
    }

    /// <summary>
    /// pluff
    /// </summary>
    /// <returns></returns>
    private bool ActualIndexCorrect()
    {
      return false;
    }



    

    public void Dispose()
    {
      try
      {
        recall_auto.Dispose();
      }
      catch (Exception)
      {
        
      }
    }
  }
}
