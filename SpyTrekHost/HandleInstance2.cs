using MessageHandler.Rig.Common;
using MessageHandler.Rig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Diagnostics;

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
                        Pipe.SendData(new RigRrqFrame(OpID.TrekList));
                        IsRequested = true;
                    }
                    else if (IsRequested)
                    {
                        if (listHandler.PState.State == ProcState.Finished)
                        {
                            rState = RoundState.UpdateTreks;
                            IsRequested = false;
                        }
                    }

                    break;

                case RoundState.UpdateTreks:
                    if (IsRequested == false)
                    {
                        var reqTrekId = listHandler.TrekList[currentTrekPosition].Id;
                        Debug.WriteLine($"List[{currentTrekPosition}] with ID: {reqTrekId}");
                        ReadTrekCmd(reqTrekId);
                        IsRequested = true;
                    }
                    else if (IsRequested)
                    {
                        if (saveHandler.PState.State == ProcState.Finished)
                        {
                            IsRequested = false;

                            if (++currentTrekPosition >= listHandler.TrekList.Capacity)
                            {
                                rState = RoundState.Sleep;
                            }
                        }
                    }

                    break;

                case RoundState.Sleep:
                    IsRequested = false;
                    rState = RoundState.UpdateList;
                    currentTrekPosition = 0;
                    roundTimer.Stop();
                    break;
            }
        }
    }
}
