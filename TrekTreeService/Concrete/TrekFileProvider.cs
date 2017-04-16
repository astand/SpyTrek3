using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrekTreeService.Abstract;
using System.IO;
using TrekTreeService.Infrastructure.Extensions;
using System.Configuration;
using System.Diagnostics;

namespace TrekTreeService.Concrete
{
    public class TrekFileProvider : ITrekInfoProvider
    {
        private readonly string  TrekExtension = "json";

        private string m_root = (ConfigurationManager.AppSettings["LocalPath"]);
        private string full_search_path;

        public TrekFileProvider() { }
        public TrekFileProvider(string directoryroot)
        {
            m_root = directoryroot;
        }

        public Byte[] GetContent(String imei, Int32? year, Int32? month, Int32? day, String name)
        {
            BuildFullPath(imei, year, month, day);

            return ReadTrekContent(name);
        }

        public IList<TrekDetails> GetInfo(String imei, Int32? year, Int32? month, Int32? day)
        {
            var retlist = new List<TrekDetails>();

            /* need get all sub folders first order */
            BuildFullPath(imei, year, month, day);

            if (day != null)
            {
                retlist = GetSubFiles();
                return retlist;
            }


            var subdirs = SubLevelDirectories();
                        
            foreach (var alonedir in subdirs)
            {
                /* get treks collection for every sub folder */
                var subsummary = SubLevelFiles(alonedir.Path);
                subsummary.NodeName = alonedir.Name;
                subsummary.NodeId = alonedir.Name;
                retlist.Add(subsummary);
            }
            return retlist;
        }

        private TrekDetails SubLevelFiles(String alonedir)
        {
            var ret = new TrekDetails();
            /* read all condescending files by search pattern */
            //var allsubdir_treks = Directory.EnumerateFiles(alonedir, "*.htm*", SearchOption.AllDirectories);
            var search_pattern = "*." + TrekExtension;
            var allsubdir_treks = Directory.EnumerateFiles(alonedir, search_pattern, SearchOption.AllDirectories);

            return TrekDetails.AverageDetailsForFiles(allsubdir_treks);
        }

        private string BuildFullPath(string imei, int? year, int? month, int? day)
        {
            string retval = "";

            retval = imei + @"/";
            if (year != null)
            {
                retval += string.Format(@"{0:D4}/", year);
                if (month != null)
                {
                    retval += String.Format(@"{0:D2}/", month);
                    if (day != null)
                    {
                        retval += String.Format(@"{0:D2}/", day);
                    }
                }
            }
            return full_search_path = (m_root + retval);
        }


        private IList<DirDescription> SubLevelDirectories()
        {
            IEnumerable<string> dirlist = null;
            var retdescription = new List<DirDescription>();
            try
            {
                dirlist = Directory.EnumerateDirectories(full_search_path);
            }
            catch (Exception ex)
            {
                //throw new DirectoryNotFoundException("Sub directory scanning fails: " + ex.Message);
                Debug.WriteLine($"Sub directory scanning fails. Empty DirDescriptor returns. Message: {ex.Message}");
                return retdescription;
            }

            foreach (var onedir in dirlist)
            {
                retdescription.Add(new DirDescription(onedir));
            }

            return retdescription;
        }

        private List<TrekDetails> GetSubFiles()
        {
            var retfiles = new List<TrekDetails>();

            IEnumerable<string> files = null;
            var retdescription = new List<TrekDetails>();

            try
            {
                files = Directory.EnumerateFiles(full_search_path, "*." + TrekExtension, SearchOption.AllDirectories);
            }
            catch (Exception ex)
            {
                //throw new DirectoryNotFoundException("Sub directory scanning fails: " + ex.Message);
                Debug.WriteLine($"Sub files scanning fails. Empty TrekDetails list returns. Message: {ex.Message}");
                return retdescription;
            }
            foreach (var item in files)
            {
                try
                {
                    var onetrek = TrekDetails.ConcreteDetailForFile(item);
                    onetrek.NodeName = onetrek.Begin.ToShortTimeString();
                    onetrek.NodeId = item.RemoveExtension().DirectoryName();
                    retfiles.Add(onetrek);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Debug.WriteLine($"{item} error: {ex.Message}");
                }
            }
            return retfiles;

        }

        private byte[] ReadTrekContent(string filename)
        {
            string file_global_path = full_search_path + filename + "." + TrekExtension;
            Debug.WriteLine($"Target content file : {file_global_path}");

            try
            {
                return File.ReadAllBytes(file_global_path);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Cannot read Trek contetn. Ex message: {ex.Message}");
                return null;
            }

        }

    }
}
