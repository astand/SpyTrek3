using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtSys;
using ProtSys.Abstract;

namespace ProtSys.Concrete
{
    public class FullFileNameBuilder : ITrekNameFormatter
    {


        public string NameBuild(MatrixItem src)
        {
            string retstring = string.Empty;

            //retstring = String.Format("{0:D5}_", src.id);
            retstring += ParseMITime(src.getStart()) + "_";
            retstring += ParseMITime(src.getEnd()) + "_";
            retstring += String.Format("{0:D5}_", src.localmileage / 10000);
            retstring += String.Format("{0:D5}", src.mileage /  10000);
            return retstring;
        }


        string ParseMITime(DateTime time)
        {
            return String.Format("{0:yyyyMMddHHmm}",time);
        }


        
    }
}
