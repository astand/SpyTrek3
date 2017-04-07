using StreamHandler.Abstract;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MessageHandler.Rig
{
    public class RigRouter
    {
        List<RigHandler> handlerList;

        public Action<ProcFullState> ProcUpdateListener {
            get;
            set;
        }

        public RigRouter(List<RigHandler> list)
        {
            handlerList = list;
        }

        public void HandleFrame(RigFrame frame)
        {
            //rigAnswer = null;
            Debug.WriteLine(frame);

            foreach (var hand in handlerList)
            {
                if (hand.HandleFrame(frame) == HandleResult.Handled)
                {
                    ProcUpdateListener?.Invoke(hand.ProcHandler.PState);
                    return;
                }
            }
        }
    }
}
