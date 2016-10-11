using StreamHandler.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler
{

    /// <summary>
    /// IFrameProcessor uses for invoke action in Read, Write or any
    /// other custom IHandlers
    /// </summary>
    public interface IFrameProccesor
    {
        void Process(FramePacket packet, ref IStreamData answer);

        event EventHandler ProcessFinished;
    }

   
    
}
