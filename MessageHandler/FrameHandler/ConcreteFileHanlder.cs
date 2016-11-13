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

        ProcessorDing ding_;


        public ConcreteFileHandler(string name, IFrameProccesor processor, Func<IStreamData, int> pipe, ProcessorDing ding = null)
        {
            this.name = name;
            this.processor = processor;
            sending = pipe;
            ding_ = ding;
        }

        public void HandleRequest(T o, UInt16 id)
        {
            if (CheckFileID(id))
            {
                ProcState outstate = ProcState.Idle;
                m_answer = null;
                processor?.Process(o, ref m_answer, out outstate);

                if (m_answer != null)
                {
                    sending(m_answer);
                }
                ding_?.Invoke(processor, outstate);
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
