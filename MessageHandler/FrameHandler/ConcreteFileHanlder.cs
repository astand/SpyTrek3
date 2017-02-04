using MessageHandler.Abstract;
using MessageHandler.Processors;
using StreamHandler;
using StreamHandler.Abstract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.ConcreteHandlers
{
    public delegate void ProcessorDing(Object obj, ProcState args);
    public class ConcreteFileHandler<T> : IHandler<T> where T : FramePacket
    {
        private IHandler<T> m_successor;

        private string name;

        private Func<UInt16, bool> CheckFileID;

        private IFrameProccesor processor;

        private IStreamData m_answer;

        private Func<IStreamData, int> sending;

        private Action<IFrameProccesor> ProcNotify;

        public ConcreteFileHandler(string name, IFrameProccesor processor,
            Func<IStreamData, int> pipe, Action<IFrameProccesor> notify = null)
        {
            this.name = name;
            this.processor = processor;
            sending = pipe;
            ProcNotify = notify;
        }

        public void HandleRequest(T o, UInt16 id)
        {
            if (CheckFileID(id))
            {
                m_answer = null;
                processor?.Process(o, ref m_answer);

                if (m_answer != null)
                {
                    sending(m_answer);
                }
                ProcNotify?.Invoke(processor);
            }
            else
                m_successor?.HandleRequest(o, id);


        }

        public void SetSpecification(Func<UInt16, Boolean> spec)
        {
            CheckFileID = spec;
        }

        public void SetSuccessor(IHandler<T> successor)
        {
            m_successor = successor;
        }

    }
}
