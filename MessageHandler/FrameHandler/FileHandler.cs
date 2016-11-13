using MessageHandler.Abstract;
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
    public class ConcreteFileHandler<T> : IHandler<T> where T : FramePacket
    {
        private IHandler<T> m_successor;

        private string name;

        private Func<UInt16, bool> CheckFileID;

        private IFrameProccesor processor;

        private IStreamData m_answer;

        private Func<IStreamData, int> sending;

        public ConcreteFileHandler(string name,
            IFrameProccesor processor,
            Func<IStreamData, int> pipe)
        {
            this.name = name;
            this.processor = processor;
            sending = pipe;
        }

        public void HandleRequest(T o, UInt16 id)
        {
            if (CheckFileID(id))
            {
                Processors.ProcState outstate;
                m_answer = null;
                processor?.Process(o, ref m_answer, out outstate);
                if (m_answer != null)
                {
                    sending(m_answer);
                }
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
