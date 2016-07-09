using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.Abstract
{
    /// <summary>
    /// Base class for chain of responsibility. Objects of this class 
    /// interlock for handling appropriated PacketFrames
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHandler<T>
    {
        void SetSuccessor(IHandler<T> handler);
        void HandleRequest(T o, UInt16 fid);
        void SetSpecification(Func<UInt16, bool> spec);
    }
}
