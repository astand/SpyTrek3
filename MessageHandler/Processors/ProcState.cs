using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.Processors
{
    public enum State { CmdAck = 0, Data, Finished, Idle };
}
