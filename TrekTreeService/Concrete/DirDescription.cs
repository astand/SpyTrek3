using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrekTreeService.Infrastructure.Extensions;

namespace TrekTreeService.Concrete
{
    public class DirDescription
    {
        //public string Name {
        //    get
        //    {
        //        return Name;
        //    }
        //    set
        //    {
        //        value.DirectoryName();
        //    }
        //}
        public string Name { get; set; }
        public string Path { get; set; }

        public DirDescription(string fullpath)
        {
            Path = fullpath;
            Name = Path.DirectoryName();
        }
    }
}
