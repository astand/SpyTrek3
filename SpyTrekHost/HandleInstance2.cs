using MessageHandler.Rig.Common;
using MessageHandler.Rig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Diagnostics;
using MessageHandler.DataFormats;

namespace SpyTrekHost
{
    public partial class HandleInstance
    {
        enum RoundState { UpdateList, UpdateTreks, Sleep };

        RoundState rState = RoundState.UpdateList;

        bool IsRequested = false;

        int currentTrekPosition = 0;

        private void RoundTimer_Elapsed(Object sender, ElapsedEventArgs e)
        {
            switch (rState)
            {
                case RoundState.UpdateList:
                    if (IsRequested == false)
                    {
                        roundTimer.Interval = 500;
                        listHandler.SendReadRequest(new RigRrqFrame(OpID.TrekList));
                        IsRequested = true;
                    }
                    else if (IsRequested)
                    {
                        if (listHandler.PState.State == ProcState.Finished)
                        {
                            IsRequested = false;
                            rState = RoundState.UpdateTreks;
                        }
                    }

                    break;

                case RoundState.UpdateTreks:
                    if (IsRequested == false)
                    {
                        int reqTrekId;
                        int read_ret = -1;

                        do
                        {
                            if (currentTrekPosition >= listHandler.TrekList.Count)
                            {
                                rState = RoundState.Sleep;
                                break;
                            }

                            reqTrekId = listHandler.TrekList[currentTrekPosition].Id;
                            read_ret = ReadTrekCmd(reqTrekId);

                            if (read_ret < 0)
                            {
                                Debug.WriteLine($"List[{currentTrekPosition}] with ID: {reqTrekId} don't load " + read_ret);
                            }
                            else
                            {
                                Debug.WriteLine($"List[{currentTrekPosition}] with ID: {reqTrekId} Perform downloading");
                            }

                            currentTrekPosition++;
                        } while (read_ret < 0);

                        IsRequested = true;
                    }
                    else if (IsRequested)
                    {
                        if (saveHandler.PState.State == ProcState.Finished)
                        {
                            IsRequested = false;
                        }
                    }

                    break;

                case RoundState.Sleep:
                    IsRequested = false;
                    rState = RoundState.UpdateList;
                    currentTrekPosition = 0;
                    roundTimer.Interval = 15 * 60 * 1000;
                    break;
            }
        }

        public List<NaviNote> ReadCachedPoints()
        {
            return echoHandler.GetCachedList();
        }
    }
}
