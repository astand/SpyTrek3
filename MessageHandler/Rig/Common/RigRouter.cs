using StreamHandler.Abstract;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MessageHandler.Rig
{
    public class RigRouter
    {
        Func<IStreamData, int> SendAnswer;

        List<RigHandler> handlerList;

        IStreamData rigAnswer;

        public Action<ProcFullState> ProcUpdateListener {
            get;
            set;
        }

        public RigRouter(Func<IStreamData, int> sender, List<RigHandler> list)
        {
            SendAnswer = sender;
            handlerList = list;
        }

        public void HandleFrame(RigFrame frame)
        {
            rigAnswer = null;
            Debug.WriteLine(frame);

            foreach (var hand in handlerList)
            {
                if (hand.HandleFrame(frame, ref rigAnswer) == HandleResult.Handled)
                {
                    if (rigAnswer != null)
                        SendAnswer?.Invoke(rigAnswer);

                    ProcUpdateListener?.Invoke(hand.ProcHandler.PState);
                    return;
                }
            }
        }
    }
}
