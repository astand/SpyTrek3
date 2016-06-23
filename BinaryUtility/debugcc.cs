using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;



namespace ProtSys
{
  public class debugsettings 
  {

  }


  public static class debugcc
  {
    static bool leverror;
    static bool levwarn;
    static bool levinfo;
    static bool levdebug;
    static bool levtrace;

    static bool istime;
    static bool isfilec;
    static bool isconsole;
    static bool isdebugviewer;

    static string loggerfile;

    static string fullst;


    static debugcc()
    {
      changeLevels();
      changeOutDestination();
    }



    public static void changeLevels(bool err = true, 
                          bool warn = true,
                          bool info = true,
                          bool debug = true,
                          bool trace = true)
    {
      leverror = err;
      levwarn = warn;
      levinfo = info;
      levdebug = debug;
      levtrace = trace;
    }




    public static void changeOutDestination(bool timeneed = true,
                        bool fileneed = false, 
      bool consneed = true,
      bool dbgviewer = true)
    {
      loggerfile = null;

      istime = timeneed;
      isfilec = fileneed;
      isconsole = consneed;
      isdebugviewer = dbgviewer;

      if (isfilec)
      {
        loggerfile = DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss") + ".logg";
      }
    }

    
    public static void dbgError(string msg) { if (leverror) dbgPrint("err:" + msg); }
    public static void dbgWarn(string msg) { if (levwarn) dbgPrint("wrn:" + msg); }
    public static void dbgInfo(string msg) { if (levinfo)  dbgPrint("inf:" + msg); }
    public static void dbgDebug(string msg) { if (levdebug)  dbgPrint("dbg:" + msg); }
    public static void dbgTrace(string msg) { if (levtrace)  dbgPrint("trc:" + msg); }

    private static void dbgPrintD(string s) { Debug.WriteIf(isdebugviewer,s); }
    public static void dbgPrintCF(string s) { ConsoleWritter(s); fileWriteLog(s); }


    private static string timeNeed()
    {

      if (istime)
        return DateTime.Now.ToString("[hh:mm:ss.ms]") + "   ";
      return "";
    }


    static void dbgPrint(string s)
    {
      fullst = timeNeed();
      dbgPrintCF(fullst + s);
      dbgPrintD(s);
    }



    private static void fileWriteLog(string s)
    {
      if (loggerfile != null)
      {
        try
        {
          using (StreamWriter logw = File.AppendText(loggerfile))
          {
            logw.WriteLine(s);
          }
        }
        catch (Exception ex)
        {
          dbgPrintD(ex.Message);
        }
      }
    }

    private static void ConsoleWritter(string s)
    {
      if (isconsole)
      {
        Console.WriteLine(s);
      }
    }

    private static void DbgViewerWritter(string s)
    {
      
    }
  }
}
