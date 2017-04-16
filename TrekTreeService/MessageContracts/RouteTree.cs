using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrekTreeService.MessageContracts
{
    public class RouteTree
    {
        public int? Year { get; set; }

        public int? Month { get; set; }

        public int? Day { get; set; }

        public string FName { get; set; }


        internal static RouteTree GetRouteTree(TrekTreeRequest request, String info)
        {
            int ret = 0;
            int.TryParse(info, out ret);

            
            var retroute = new RouteTree();
            retroute.Year = request.Year;
            retroute.Month = request.Month;
            retroute.Day = request.Day;

            if (retroute.Year == null)
                retroute.Year = ret;
            else if (retroute.Month == null)
                retroute.Month = ret;
            else if (retroute.Day == null)
                retroute.Day = ret;
            else
                retroute.FName = info;

            return retroute;

        }
    }
}
