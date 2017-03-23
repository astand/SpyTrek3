using MessageHandler.Rig;
using StreamHandler.Abstract;


namespace MessageHandler
{

    /// <summary>
    /// IFrameProcessor uses for invoke action in Read, Write or any
    /// other custom IHandlers
    /// </summary>
    public abstract class IFrameProccesor
    {
        public ProcState State { get; protected set; } = ProcState.Idle;

        public abstract void Process(FramePacket packet, ref IStreamData answer);
    }
}
