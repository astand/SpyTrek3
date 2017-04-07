using StreamHandler.Abstract;
using System;

namespace MessageHandler.Rig
{
    /// <summary>
    /// IFrameProcessor uses for invoke action in Read, Write or any
    /// other custom IHandlers
    /// </summary>
    public abstract class IFrameProccesor<T>
    {
        public String Name { get; protected set; }

        public ProcFullState PState { get; protected set; } = new ProcFullState();

        public abstract void Process(T packet);

        public Action<T> SendAnswer;

        public abstract bool FrameAccepted(T o);
    }
}
