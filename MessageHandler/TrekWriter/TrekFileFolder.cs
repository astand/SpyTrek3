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

        static private Int32 MAX_SIZE
        {
            get { return 500; }
        }

        Int32[] itemSize = new Int32[MAX_SIZE];

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

        private Int32 getSavedTopicSize(Int32 index)
        {
            Int32 filesize = 0;

            try
            {
                filesize = itemSize[index];
            }
            catch (IndexOutOfRangeException)
            {
                return 0;
            }

            return filesize;
        }


        public bool PrepareDirectory(TrekDescriptor desc, string im, Int32 topic_id = -1)
        {
            mainDestinationPath = BuildFullFileNameChain(desc, im);


            if (getSavedTopicSize(topic_id) == CurrentFileSize())
            {
                return false;
            }

            Int32 count_of_navi_file = getCountOfNaviNotes();
            Int32 count_of_navi_trek = (int)(desc.TrekSize / NaviNote.Lenght);

            if (count_of_navi_file == count_of_navi_trek)
            {
                itemSize[topic_id] = (Int32)new FileInfo(mainDestinationPath).Length;

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
            string desc_dir = motherDir.UpdateAndCreateDir(im, desc.Start.ToString(@"yyyy\\MM\\dd\\"));
            /* prepare file name */
            return TrackUnconflictName(GetTrekFileName(desc), desc_dir, true);
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private Int32 TrackDirCreate(string dir)
        {
            /* check existing of directory */
            if (Directory.Exists(dir))
                return 0;

            try
            {
                Directory.CreateDirectory(dir);
            }
            catch (Exception)
            {
                return -1;
            }

            return 0;
        }
        
        /// <summary>
        /// Create full Path for trek
        /// </summary>
        /// <param name="fname">Name for new trek (without extension)</param>
        /// <param name="directory">Full directory path</param>
        /// <param name="rewritetrek">if true - existing trek will overwrite</param>
        /// <returns></returns>
        private string TrackUnconflictName(string fname, string directoryPath, bool rewritetrek = true)
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

            fname = directoryPath + fname;

            //if (!File.Exists(fname) || rewritetrek)
            {
                return (fname);
            }

            //while (File.Exists(fname + "_(" + findex + ")"))
            //{
            //    findex++;
            //}

            //return (fname + "_(" + findex + ")");
        }
        
        /// <summary>
        /// Check exsisting file size. If error occured return -1 that means no file
        /// </summary>
        /// <returns></returns>
        private Int32 CurrentFileSize()
        {
            try
            {
                return (Int32)(new FileInfo(mainDestinationPath)).Length;
            }
            catch (FileNotFoundException)
            {
                return -1;
            }
            catch (Exception)
            {
                return -2;
            }
        }

        private string GetTrekFileName(TrekDescriptor desc)
        {
            StringBuilder ret = new StringBuilder(100);

            ret.Append(desc.Start.ToString("yyyyMMddHHmm"));
            ret.Append("_");
            ret.Append(desc.Stop.ToString("yyyyMMddHHmm"));
            ret.Append("_");
            ret.Append($"{desc.Dist / 10000:D5}_{desc.Dist / 10000:D5}");
            ret.Append(".json");

            return ret.ToString();
        }

        private Int32 getCountOfNaviNotes()
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
