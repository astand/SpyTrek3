using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ProtSys
{

  public enum HandleState { IDLE, SEND, WAIT, FREE };

  public sealed class MasterSideTftp : G2_Client
  {

    TrekFileFolder fileForTR;

    FileUpload oufile = new FileUpload(@"st8.bin");

    UInt16 t_echo;
    UInt16 t_wait;

    private System.Timers.Timer echotimeout = new System.Timers.Timer();

    private TimControl tw0;

    private HandleState hs = HandleState.IDLE;

    private CommonFileSize comFsize = new CommonFileSize();

    private AutoRotateCommands autocmds = new AutoRotateCommands();

    Int32 index_for_mitems = 0;

    /// <summary>
    /// Constructor
    /// </summary>
    public MasterSideTftp(UInt16 echo, UInt16 wait, string rootdir)
    {
      t_echo = echo;
      t_wait = (wait > 5) ? (wait) : (UInt16)(5) ;
      fileForTR = new TrekFileFolder(rootdir);
    }


    protected override void Dispose(bool disposing)
    {

      if (disposing)
      { 
        tw0.Dispose();
        echotimeout.Dispose();
        autocmds.Dispose();
      }
      base.Dispose(true);
      debugcc.dbgInfo(" Finalize for master Side");
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="insocket"></param>
    /// <param name="id"></param>
    public override void startG2Client(System.Net.Sockets.TcpClient insocket, int id)
    {
      /* ??? */
      tw0 = new TimControl(t_wait);

      /* wait 2 sec for first echo ack */
      echotimeout.Interval = 2 * 1000;
      echotimeout.Elapsed += echotimeout_Elapsed;
      if (t_echo != 0)
        echotimeout.Start();

      base.startG2Client(insocket, id);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="lenrx"></param>
    protected override void mainDataHandler(int lenrx)
    {

      ReqProcess();
      if (UnpackReceive(lenrx) != 0) { return; }
      RecProcess();
    }


    /// <summary>
    /// 
    /// </summary>
    protected override void ReqProcess()
    {
      int ret = -1;

      AutoGeneratingRequest();


      if (receivestatus.IsFlagsSet(ReceiveResult.SendData))
        ret = SendData();
      else
        ret = SendRequest();

      if (ret >= 0)
      {
        debugcc.dbgInfo(" --:> [] " + from0.ToString() + " Size: " + ret.ToString());
        tw0.Load(5 * 1000); // Reload for resend action
        m_netraw.handleOutStream(from0.getBytes(ret));
      }

      if (receivestatus.IsFlagsSet(ReceiveResult.WaitData) && tw0.ISREADY)
      {
        debugcc.dbgDebug(" Timeout in recieve data state");
        DeathOfConnection();
      }
    }




    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Int32 SendRequest()
    {
      Int32 ret = -1;
      if (hs != HandleState.SEND && receivestatus.ISWAITSTOP)
      {
        hs = HandleState.IDLE;
      }
      switch (hs)
      {
        case HandleState.IDLE:
          if (receivestatus.ISWAITSTOP && SendMachine.reqFile != TfBase.ID_NONE)
          {
            hs = HandleState.SEND;
            bchief.reloadBlocks();
          }
          break;

        case HandleState.SEND:
          ret = HandleRequests();

          if (ret >= 0)
            hs = HandleState.WAIT;
          else if (ret == -1)
            hs = HandleState.FREE;
          else
            hs = HandleState.IDLE;
          break;

        case HandleState.WAIT:
          if (tw0.ISREADY && receivestatus.IsFlagsSet(ReceiveResult.WaitAck))
          {
            if (!bchief.RollBackBidSend())
            {
              hs = HandleState.FREE;
              break;
            }
            hs = HandleState.SEND;
          }
          break;

        case HandleState.FREE:
          debugcc.dbgDebug(" Timeout in send request state");
          DeathOfConnection();
          hs = HandleState.IDLE;
          break;
      }
      return ret;
    }

    /// <summary>
    /// Determines needness of sending action. 
    /// </summary>
    /// <returns>ret >= 0 - need send. -1. broken</returns>
    private Int32 HandleRequests()
    {
      Int32 ret = 0;

      from0.headUp(SendMachine.reqFile);

      if (SendMachine.reqFile == TfBase.ID_FILENOTES)
        alltreks.setActiveIndex(0);

      else if (SendMachine.reqFile == TfBase.ID_TRACK)
      {
        ret = from0.LoadTrackIndex(alltreks.REQUEST_ID);
      }

      else if (SendMachine.reqFile == TfBase.ID_FIRMWARE)
      {
        comFsize.New(oufile.Lenght());
        from0.loadFSizeData(comFsize.LenghtFile);
        ret = 6;
      }

      receivestatus.ToAction(ReceiveResult.WaitAck);
      /* from0.OPC WRQ or RRQ already */
      come0.headUp(from0);
      updateStatus("Send request...");

      return ret;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Int32 SendData()
    {
      Int32 ret = -1;
      
      if (bchief.IsBlockSendPermitted())
      {
        /* ??? other function */
        from0 = new GTftp2((Int32)G2_OUT_DATA_LEN + 4);
        ret = oufile.TryReadFromFile((UInt16)(bchief.bidsend - 1), 
          G2_OUT_DATA_LEN, from0.DATA);
        from0.headUp(TfBase.OPC_DATA, bchief.bidsend);
        bchief.FixLastBid(ret);
      }
      else
      {
        if (tw0.ISREADY)
        {
          if (!bchief.RollBackBidSend())
          {
            //updateStatus("Conncetion is broken");
            debugcc.dbgDebug(" Timeout in send data state");
            receivestatus.ToStop();
            DeathOfConnection();
            return -1;
          }
        }
      }
      return ret;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="blocklen"></param>
    /// <returns></returns>
    protected override int UnpackReceive(int blocklen)
    {
      return base.UnpackReceive(blocklen);
    }


    /// <summary>
    /// 
    /// </summary>
    protected override void RecProcess()
    {
      string sthead = " <:-- [] " + come0.ToString() + " " + 
        receivestatus.ToString();
      debugcc.dbgInfo(sthead);
      receivestatus.IsWrong(true);


      if (receivestatus.IsFlagsSet(ReceiveResult.WaitAck))
      {
        GtftpACK(come0);
      }
      else if (receivestatus.IsFlagsSet(ReceiveResult.WaitData))
      {
        GtftpDATAReceive();
      }
      else if (receivestatus.IsFlagsSet(ReceiveResult.SendData))
      {
        GtftpDATASend(come0);
      }

      else if (come0.OPC == TfBase.OPC_ERR)
      {
        /* error filter */
        debugcc.dbgInfo(sthead + " Error OPC received!!!Code = " + come0.ID);
        updateStatus(" Error message was got");
        receivestatus.ToStop();
      }

      else 
      {
        debugcc.dbgDebug("Unknown state. Error");
        receivestatus.ToStop();
      }
      updateList(" <:-- IN " + come0.ToString());
      if (receivestatus.ISWAITSTOP) { FreeWaiting(); }
      else if (!receivestatus.IsWrong())
      {
      }
      else
      {
        debugcc.dbgDebug(" WrongBlock. Error.");
      }
    }

    
    /// <summary>
    /// Method for handle ACK block. Depending of @block_id next state select
    /// </summary>
    /// <param name="tf"></param>
    private void GtftpACK(GTftp2 tf)
    {
      string message = " Parse ACK: Transfer state to >>> ";
      bchief.reloadBlocks();

      if (!tf.IsEqual(from0))
      {
        receivestatus.Wrong();
        message += "IDLE. Non correct ACKNOWLEDGE";
      }
      
      commonFSize = tf.getFSizeFromData();

      if (from0.OPC == TfBase.OPC_WRQ)
      {
        receivestatus.ToAction(ReceiveResult.SendData);
      }
      else if (commonFSize == 0)
      {
        updateStatus(" Request ACK OK");
        message += "IDLE.";
        receivestatus.ToStop();
      }
      else
      {
        updateStatus(" Request ACK OK. Wait data");
        comFsize.New((Int32)commonFSize);
        message += "WAIT_DATA.";
        receivestatus.ToAction(ReceiveResult.WaitData);
        tw0.Load(10 * 1000); // waiting data first Load
      }

      debugcc.dbgTrace(message);
      return;
    }


    /// <summary>
    /// 
    /// </summary>
    private void AutoGeneratingRequest()
    {
      UInt16 waitingrequest;
      waitingrequest = autocmds.getActualCommand();
      
      
      if (RequestCompleted(waitingrequest))
        autocmds.StepForwardCmd();
    }



    private bool RequestCompleted(UInt16 actualcmd)
    {
      if (SendMachine.IsNotIdle()) { return false; }
      /* state is idle */


      if (actualcmd == TfBase.ID_FILENOTES)
        index_for_mitems = 0;

      if (actualcmd == TfBase.ID_TRACK)
      {
        try
        {
          bool condition_for_break = true;
          while (condition_for_break)
          {
            updateList("Check trek N: " + index_for_mitems.ToString());
            if (RequestTrekByNum(index_for_mitems) == 0)
              condition_for_break = false;
            index_for_mitems++;
          }
          return false;
        }
        catch (NullReferenceException nex)
        {
          debugcc.dbgWarn(" Stop trek loading, Null received " + nex.Message);
          return true;
        }
        catch (Exception other_ex)
        {
          debugcc.dbgError(" Undetected exception : " + other_ex.ToString());
          return true;
        }
        
      }
      SendMachine.SetStFile(actualcmd);
      return true;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bk"></param>
    internal void GtftpDATASend(GTftp2 bk)
    {
      /* wait data */
      if (bchief.blockCheck(come0.ID))
      {
        comFsize.AddPassing((Int32)G2_OUT_DATA_LEN);
        updateStatus("Sending File: " + comFsize.ToString());
        bchief.reloadResendCounter();
      }
      else
      {
        receivestatus.Wrong();
      }

      if (bchief.IsLastBlock())
      {
        debugcc.dbgInfo(" Last ACK was got. Stop machine");
        updateList("File uploaded: OK");
        receivestatus.ToStop();
      }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="come0"></param>
    internal void GtftpDATAReceive()
    {
      /* wait data */
      int ret = come0.DATA.Length;
      string message = " Parse data: ";

      if (!bchief.blockCheck(come0.ID))
      {
        message += String.Format("Incorrect data ID, ACK but not handle");
        debugcc.dbgWarn(message);
        receivestatus.Wrong();
      }
      else
      {
        DataHandle(come0, SendMachine.reqFile);
        if (ret == 0)
        {
          updateStatus(comFsize.ToString() + " Finished OK");
          receivestatus.ToStop();
          debugcc.dbgInfo(message + " Finished OK");
        }
        else
        {
          updateStatus(comFsize.ToString());
        }
        comFsize.AddPassing(ret);
        tw0.Load(10 * 1000); // waiting data process
      }

      from0.headAckUp(come0.ID);
      debugcc.dbgInfo(" --:> [] " + from0.ToString() + " Size: 0");
      m_netraw.handleOutStream(from0.getBytes(0));
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="bl"></param>
    /// <param name="f_id"></param>
    internal void DataHandle(GTftp2 bl, UInt16 f_id)
    {
      switch (f_id)
      {
        case (TfBase.ID_INFO):
          if (bl.DATA.Length > 0)
          {
            //new 
            spInfo.TryParse(UTF8Encoding.UTF8.GetString(bl.DATA)); ;
            debugcc.dbgInfo(" Board information : " + spInfo.ToString());
            updateInfo(spInfo);
            updateList(" Info - " + spInfo.ToString());
          }
          break;


        case (TfBase.ID_FILENOTES):


          if (bl.DATA.Length == 0)
          {
            updateList(" File list downloaded: OK");
          }
          else
          {
            SplitDataToMatrixItem(bl.DATA, 0);
          }
          
          break;

        case (TfBase.ID_TRACK):
          SplitDataToNaviNote(bl.DATA, bl.DATA.Length);
          break;
        default:
          break;
      }
    }

    /// <summary>
    /// Perform all preparing work for downloading new trek
    /// if trek already exist and size are equal then work will not be 
    /// doing. Causes:
    /// 1 - No correct info
    /// 2 - Error in file creation
    /// 3 - wrong ID (in this case MItem will be null
    /// </summary>
    /// <param name="indx">desired ID of topics</param>
    /// <returns></returns>
    //internal Int32 RequestTrekByID(Int32 indx)
    //{
    //  /* if not actual info -> will not perform trek loading */
    //  if (!spInfo.TrueInfo)
    //    return -1;
      
    //  var MItem = alltreks.getItemByRequestId((UInt16)indx);

    //  return CheckMItemAndInvokeRequest(MItem);
    //}


    public Int32 RequestTrekByNum(Int32 num)
    {
      /* if not actual info -> will not perform trek loading */
      if (!spInfo.TrueInfo)
        throw (new NullReferenceException());


      var MItem = alltreks.getItemByNumInList(num);

      return CheckMItemAndInvokeRequest(MItem, num);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="indx"></param>
    /// <returns></returns>
    //private Int32 CheckMItemAndInvokeRequest(MatrixItem mitem)
    //{
    //  /* can be nullreferenceexception */
    //  if (!mitem.DistanceValid)
    //    return -3;

    //  if (!fileForTR.PrepareDirectory(mitem, spInfo.Imei))
    //    return -2;

    //  alltreks.UpdateRequestID();
    //  SendMachine.SetStFile(TfBase.ID_TRACK);
    //  return 0;
    //}


    /// <summary>
    /// Vlidation trek: Check Distance, Creating template, Existing files
    /// </summary>
    /// <param name="mitem">Item for handling</param>
    /// <returns>0 - succsses decesion about downloding</returns>
    private Int32 CheckMItemAndInvokeRequest(MatrixItem mitem, Int32 index_in_mi = -1)
    {
      /* can be nullreferenceexception */
      if (!mitem.DistanceValid)
        return -3;

      if (!fileForTR.PrepareDirectory(onetopic: mitem, im: spInfo.Imei, topic_id: index_in_mi))
        return -2;

      alltreks.UpdateRequestID();
      SendMachine.SetStFile(TfBase.ID_TRACK);
      return 0;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="b"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public Int32 SplitDataToMatrixItem(byte[] b, UInt16 blocknum)
    {
      Int32 ret = 0; 
      string debugmessage;

      debugcc.dbgTrace("Split MI...");

      do
      {
        debugmessage = String.Format("MI[{0}]. data len:{1}. offset:{2}", ret, b.Length, ret * MatrixItem.Lenght);
        debugcc.dbgTrace(debugmessage);

        try
        {
          alltreks.LoadNewItemInCurrentIndex(MatrixItem.Factory(b, ref ret));
        }
        catch (NullReferenceException)
        {
          /* end of data */
          break;
        }
        catch (Exception ex)
        {
          debugcc.dbgError(" Error in split data to MI: " + ex.ToString());
        }

        updateGrid(alltreks.getItemByCurrentIndex());
        alltreks.StepAheadIndex();
        
      } while (true);
      debugcc.dbgTrace("Split MI End");
      return ret;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="b"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    public Int32 SplitDataToNaviNote(byte[] b, Int32 len)
    {
      navinote nav = new navinote();
      int i = 0;
      while (navinote.BufferValid(i, len))
      {
        nav = new navinote(b, i);
        fileForTR.AddOneNaviNote(nav);
        i++;
        //debugcc.dbgDebug(i + " track: " + nav.GetStringNotify());
      }
      return 0;
    }


    /// <summary>
    /// Dispose all resources and send event about end of connection
    /// </summary>
    private void DeathOfConnection()
    {
      m_netraw.MarkAsDead();
      FreeWaiting();
      echotimeout.Dispose();
      tw0.Dispose();
    }


    /// <summary>
    /// Stop WaitTimeOut and Set IDLE state for SendMachine
    /// </summary>
    private void FreeWaiting()
    {
      debugcc.dbgTrace(" Free waiting state");
      //waittimeout.Stop();
      SendMachine.ToIdle();
    }
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="val0"></param>
    private void reloadEchoTimer()
    {
      echotimeout.Interval = t_echo * 1000;
      echotimeout.Start();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    private void echotimeout_Elapsed(Object source, System.Timers.ElapsedEventArgs e)
    {
      string message = String.Format("[{0,2}] Thread. Echo timeout. ", curentindex);

      if (m_netraw.isDead())
      {
        message += "SOCKET DISPOSED. Stop echo";
        echotimeout.Stop();
      }

      if (receivestatus.ISWAITSTOP)
      {
        message += "SEND REQUEST";
        SendMachine.reqFile = TfBase.ID_ECHO;
        reloadEchoTimer();
      }
      else
      {
        reloadEchoTimer();
      }
      debugcc.dbgTrace(message);
    }
  }
}
