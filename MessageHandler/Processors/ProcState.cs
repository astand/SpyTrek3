using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.Processors
{
    public enum ProcState { CmdAck = 0, Data, Finished, Idle };

    public class ProcFullState
    {
        public ProcState State {
            get;
            set;
        }
        public String Message {
            get;
            set;
        }
    }
}
