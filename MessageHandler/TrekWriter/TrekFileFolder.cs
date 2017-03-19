using MessageHandler.DataFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.TrekWriter
{
    public class TrekFileFolder
    {

        string mainDestinationPath = "";

        internal MotherDirectory motherDir;

        public TrekFileFolder(string s)
        {
            motherDir = new MotherDirectory(s);
        }

        public Int32 AddOneNaviNote(NaviNote item)
        {
            if (mainDestinationPath == null) return -1;

            var fileContents = File.ReadAllText(mainDestinationPath);
            fileContents = fileContents.Replace("];/*replace*/", item.GetStringNotify() + "\n];/*replace*/");
            File.WriteAllText(mainDestinationPath, fileContents);

            return 0;
        }

        public Int32 AddNoteList(List<NaviNote> naviList)
        {
            using (StreamWriter sw = File.AppendText(mainDestinationPath))
            {
                foreach (var item in naviList)
                {
                    sw.WriteLine(item.GetStringNotify());
                }
            }
            return 0;
        }
        /// <summary>
        /// Must decide wheter to load trek or not
        /// </summary>
        /// <param name="desc">Trek descriptor</param>
        /// <param name="imei">Imei</param>
        /// <returns></returns>
        public bool IsTrekNotPresented(TrekDescriptor desc, string imei)
        {
            /// create file full file location with name
            mainDestinationPath = BuildFullFileNameChain(desc, imei);

            if (GetCountOfNotesFromFile() == desc.NotesCount)
            {
                return false;
            }
            /* no file or file not full */
            File.Delete(mainDestinationPath);

            return true;
        }

        /// <summary>
        /// Initialize mainDestinationPath by full path to file
        /// </summary>
        /// <param name="onetopic"></param>
        /// <param name="im"></param>
        public string BuildFullFileNameChain(TrekDescriptor desc, string im)
        {
            /// prepare directory
            string desc_dir = motherDir.UpdateAndCreateDir(im, desc.Start.ToString(@"yyyy\\MM\\dd\\"));

            /// prepare file name 
            string desc_fname = GetTrekFileName(desc);

            CleanAllPreviousCopies(desc_dir, GetTrekFileName(desc));

            return desc_dir + desc_fname;
        }


        /// <summary>
        /// Clean files that starts with same timePoint but other part of name is different
        /// </summary>
        /// <param name="fname"></param>
        /// <param name="directoryPath"></param>
        private void CleanAllPreviousCopies(string directoryPath, string fname)
        {
            //Int32 findex = 1;

            var alldirtreks = new DirectoryInfo(directoryPath);

            string mask_for_clean = fname.Substring(0, 12) + "*";

            foreach (var delfil in alldirtreks.EnumerateFiles(mask_for_clean))
            {
                /// clean all file that have same start time and another stopTime/dist/mileage
                if (delfil.Name != fname)
                    delfil.Delete();
            }
        }


        private string GetTrekFileName(TrekDescriptor desc)
        {
            StringBuilder ret = new StringBuilder(100);

            ret.Append(desc.Start.ToString("yyyyMMddHHmm"));
            ret.Append("_");
            ret.Append(desc.Stop.ToString("yyyyMMddHHmm"));
            ret.Append("_");
            ret.Append($"{desc.Dist / 1000:D5}_{desc.Odometr / 1000:D5}");
            ret.Append(".json");

            return ret.ToString();
        }

        private Int32 GetCountOfNotesFromFile()
        {
            var lineCount = 0;

            if (!File.Exists(mainDestinationPath))
            {
                return 0;
            }
            try
            {
                using (var reader = File.OpenText(mainDestinationPath))
                {
                    do
                    {
                        if (reader.ReadLine().Contains(",titl:"))
                            lineCount++;
                    } while (!reader.EndOfStream);
                }
            }
            catch (Exception)
            {
                File.Delete(mainDestinationPath);
                return 0;
            }
            return lineCount;
        }
    };



    /* ------------------------------------------------------------------------- */
    class MotherDirectory
    {
        static readonly string defaultPath = @"uncorrect_conf_dir\";

        readonly string mainPath;

        internal MotherDirectory(string s)
        {
            if (s != "")
                mainPath = InsertEndSlesh(s);
        }


        internal string UpdateAndCreateDir(string imei, string dir)
        {
            imei = InsertEndSlesh(imei);
            string ret = mainPath + imei + dir;

            if (SecureDirectoryCreator(ret) == false)
            {
                ret = defaultPath + imei + dir;
                SecureDirectoryCreator(ret);
            }
            ret = InsertEndSlesh(ret);
            return ret;
        }


        private string InsertEndSlesh(string s)
        {
            if (s.EndsWith(@"\") || s.EndsWith(@"/"))
                return s;
            return s + @"\";

        }

        bool SecureDirectoryCreator(string s)
        {
            if (Directory.Exists(s)) return true;
            try
            {
                Directory.CreateDirectory(s);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }

}
