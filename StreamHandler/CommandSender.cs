using StreamHandler.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StreamHandler
{
    public class CommandSender
    {
        private Piper pipe;

        public EventHandler<PiperEventArgs> OnData;


        public CommandSender(Piper pipe)
        {
            this.pipe = pipe;
            pipe.OnData += Pipe_OnData;
        }

        private void Pipe_OnData(Object sender, PiperEventArgs e)
        {
            Volatile.Read(ref OnData)?.Invoke(sender, e);
        }

        public void SendCommand(IStreamData streamData)
        {
            pipe.SendData(streamData);
        }



    }
}
