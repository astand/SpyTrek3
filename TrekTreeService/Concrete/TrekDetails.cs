using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrekTreeService.Infrastructure.Extensions;
using TrekTreeService.MessageContracts;

namespace TrekTreeService.Concrete
{
    public class TrekDetails
    {

        public string NodeName { get; set; }
        public string NodeId { get; set; }


        public DateTime? Begin { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Duration { get; set; }

        public Int32 LocalDistance { get; set; }
        public Int32 FullDistance { get; set; }

        public Int32 Count { get; set; }
        //public TrekDetailsType detailtype { get; set; }

        public TrekDetails() { }

        /// <summary>
        /// Convert string with time to DateTime object
        /// </summary>
        /// <param name="time">string with time</param>
        /// <returns>parsed DateTime object</returns>
        /// <exception cref="ArgumentOutOfRangeException">If string not fit for parsing</exception>
        public static DateTime ToDateTime(string time)
        {
            DateTime retdate;
            try
            {
                Int32 ly = Convert.ToInt32(time.Substring(0,4));
                Int32 lm = Convert.ToInt32(time.Substring(4,2));
                Int32 ld = Convert.ToInt32(time.Substring(6,2));
                Int32 lhh = Convert.ToInt32(time.Substring(8,2));
                Int32 lmm = Convert.ToInt32(time.Substring(10,2));

                /* try build DateTime object */
                retdate = new DateTime(ly, lm, ld, lhh, lmm, 0);
            }
            catch (Exception)
            {
                throw new ArgumentOutOfRangeException(null, $"Time chunk is not valid {time}");
            }
            return retdate;

        }

        public void SetLocalDistance(string dist)
        {
            LocalDistance = int.Parse(dist);
        }

        public void SetFullDistance(string fulldistance)
        {
            FullDistance = int.Parse(fulldistance);
        }

        public void SetDuration()
        {
            if (Begin == null || End == null || Begin?.CompareTo(End) >= 0)
                throw new ArgumentOutOfRangeException(null, $"Cannot set duration: begin {Begin}, end {End}");
            Duration = Begin != null ? End.Subtract((DateTime)Begin) : TimeSpan.MinValue;
        }

        public double Speed()
        {
            return (Duration.TotalHours == 0) ? (0) : ((LocalDistance) / Duration.TotalHours);
        }

        /// <summary>
        /// Accumalate digital parameters
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns></returns>
        public static TrekDetails operator +(TrekDetails c1, TrekDetails c2)
        {
            return new TrekDetails
            {
                LocalDistance = c1.LocalDistance + c2.LocalDistance,
                FullDistance = c2.FullDistance,
                Duration = c1.Duration + c2.Duration
            };
        }

        /// <summary>
        /// Get collection of file names (include path) and calculate common TrekDetails info for all of them
        /// </summary>
        /// <param name="files">enumerable collection of file names</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">In case when parsing cannot be done</exception>
        public static TrekDetails AverageDetailsForFiles(IEnumerable<string> files)
        {
            TrekDetails ret = new TrekDetails();
            foreach (var file in files)
            {
                try
                { 
                    ret += TrekDetails.ParseFileName(file.DirectoryName().RemoveExtension());
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Debug.WriteLine($"{file} error: {ex.Message}");
                }

            }

            ret.Count = files.Count();
            return ret;
        }

        public static TrekDetails ConcreteDetailForFile(string file)
        {
            return TrekDetails.ParseFileName(file.DirectoryName().RemoveExtension());
        }

        public static TrekTreeInstance ConvertToMessage(TrekDetails info)
        {
            var ret = new TrekTreeInstance()
            {
                Start = info.Begin,
                Count = info.Count,
                Duration = info.Duration,
                LocalMile = info.LocalDistance / 10.0,
                FullMile = info.FullDistance / 10.0,
                AverageSpeed = info.Speed() / 10.0,
            };
            return ret;
        }


        private static TrekDetails ParseFileName(String filename)
        {
            string[] slices = filename.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

            if (slices.Length != 4)
            {
                throw new ArgumentOutOfRangeException(null, $"Lenght of chunks {slices.Length}. Must be 4");
            }
            TrekDetails ret = new TrekDetails();
            /* it must be : 0 - start full time, 1 - stop full time, 2 - local mileage, 3 - full mileage */
            ret.Begin = ToDateTime(slices[0]);
            ret.End = ToDateTime(slices[1]);
            ret.SetLocalDistance(slices[2]);
            ret.SetFullDistance(slices[3]);
            ret.SetDuration();
            return ret;
        }

        public override String ToString()
        {
            return $"Inf: {NodeName}. Count: {Count}. Duration: {Duration}";
        }
    }

    public enum TrekDetailsType
    {
        SubLevelDetails = 0,
        TrekDetails
    }
}
