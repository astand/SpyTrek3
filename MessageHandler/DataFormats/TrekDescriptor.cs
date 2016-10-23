using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageHandler.DataFormats
{
    public class TrekDescriptor
    {
        public UInt16 Id { get; private set; }

        public DateTime Start { get; private set; }

        public DateTime Stop { get; private set; }

        public UInt32 TrekSize { get; private set; }
        public UInt32 Dist { get; private set; }
        public UInt32 Odometr { get; private set; }

        public static Int32 Length { get; } = 2 + 6 + 6 + 4 + 4 + 4;

        public Int32 NotesCount => ((int)(TrekSize / NaviNote.Lenght));

        private static readonly Int32 MINIMUM_DISTANCE_DETECT = (200 * 10);

        public TrekDescriptor() { }

        private void Parse(Byte[] buff, Int32 offset)
        {
            Int32 current_position = offset;

            Id = BitConverter.ToUInt16(buff, current_position);
            Start = DateTimeUtil.GetDateTime(buff, current_position += 2);
            Stop = DateTimeUtil.GetDateTime(buff, current_position += 6);
            TrekSize = BitConverter.ToUInt32(buff, current_position += 6);
            Dist = BitConverter.ToUInt32(buff, current_position += 4);
            Odometr = BitConverter.ToUInt32(buff, current_position += 4);
        }

        /// <summary>
        /// Exception secure method for parsing blocks
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public bool TryParse(Byte[] buff, Int32 offset)
        {
            try
            {
                Parse(buff, offset);
                return true;
            }
            catch (ArgumentOutOfRangeException) { }
            catch (IndexOutOfRangeException) { }
            catch (ArgumentException) { }

            return false;
        }

        public override String ToString() => $"{Id:D4}: {Start.ToJS()}  {Stop.ToJS()}. " +
            $"File size = {TrekSize:D5} bytes\tDist = {Dist:D5} km \tOdometr: {Odometr:D5} km";
    };

}

