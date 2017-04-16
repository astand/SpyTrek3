using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrekTreeService.Concrete;

namespace TrekTreeService.Abstract
{
    interface ITrekInfoProvider
    {
        IList<TrekDetails> GetInfo(string imei, int? year, int? month, int? day);
        byte[] GetContent(string imei, int? year, int? month, int? day, string name);

        //string GetInfo(string fname);
        //string GetInfo(List<string> fnames);
        //string[] GetSplittedInfo(string onefile);
        //string[] GetSplittedInfo(List<string> fnames);
        //string[] GetSplittedInfo(string[] fnames);
        //DateTime GetStartTime();
        //DateTime GetStopTime();
    }
}
