using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
//using TrekTreeService;

namespace TrekTreeServiceHost
{
    class Program
    {
        static void Main()
        {
            using (ServiceHost host = new ServiceHost(typeof(TrekTreeService.TrekTreeService)))
            {
                host.Open();
                Console.WriteLine("TrekTreeService started @ " + DateTime.Now);
                Console.ReadKey();
            }
        }
    }
}
