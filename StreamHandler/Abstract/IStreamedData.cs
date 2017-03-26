﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamHandler.Abstract
{
    public interface IStreamData
    {
        Byte[] SerializeToByteArray();
        bool SerializeToByteArray(Byte[] destination);
    }
}
