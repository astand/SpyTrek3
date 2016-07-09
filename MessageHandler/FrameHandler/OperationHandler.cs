using MessageHandler.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler
{
    public class OperationHandler<T,TSpec> : IHandler<T> where T : FramePacket where TSpec : IFrameSpecification, new()
    {

        private UInt16 ActualFID;

        private IHandler<T> m_successor;

        private TSpec dispatcher = new TSpec();

        protected readonly IHandler<T> fid_handler;

        public OperationHandler(IHandler<T> fid_handler)
        {
            this.fid_handler = fid_handler;
        }

        public void HandleRequest(T o, UInt16 fid = 0)
        {
            if (dispatcher.IsData(o.Opc) || dispatcher.IsHead(o.Opc))
            {
                if (dispatcher.IsHead(o.Opc))
                    /* Update Actual FID */
                    ActualFID = o.Id;

                fid_handler.HandleRequest(o, ActualFID);
            }
            else
            {
                m_successor?.HandleRequest(o, fid);
            }
        }

        
        public void SetSuccessor(IHandler<T> handler)
        {
            m_successor = handler;
        }

        
        public void SetSpecification(Func<UInt16, Boolean> spec)
        {

        }
    }
}
