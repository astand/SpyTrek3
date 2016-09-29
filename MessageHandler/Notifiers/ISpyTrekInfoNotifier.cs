using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.Notifiers
{
    public interface ISpyTrekInfoNotifier
    {
        event EventHandler<InfoEventArgs> Notify;
    }
}
