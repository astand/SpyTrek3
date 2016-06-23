using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtSys;

namespace ProtSys.Abstract
{
    public interface ITrekNameFormatter
    {
        string NameBuild(MatrixItem src);
    }
}
