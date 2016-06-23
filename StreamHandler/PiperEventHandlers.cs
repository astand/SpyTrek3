using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamHandler
{
    public delegate void DataEventHandler(Object sender, EventArgs e);

    public delegate void ErrorventHandler(Object sender, EventArgs e);
}
