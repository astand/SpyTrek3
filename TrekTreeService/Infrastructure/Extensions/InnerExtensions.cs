using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrekTreeService.Infrastructure.Extensions
{
    public static class InnerExtensions
    {
        public static string DirectoryName(this string source)
        {
            string[] chunks = source.Split(new char[] { '/' , '\\' }, StringSplitOptions.RemoveEmptyEntries);
            return chunks[chunks.Length - 1];
        }

        public static string RemoveExtension(this string source)
        {
            int indexofdot = source.LastIndexOf('.');
            if (indexofdot > 0)
                return source.Remove(indexofdot);
            return source;

        }
    }
}
