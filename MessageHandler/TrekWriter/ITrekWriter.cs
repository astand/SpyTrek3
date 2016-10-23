using MessageHandler.DataFormats;
using System;
using System.Collections.Generic;

namespace MessageHandler.TrekWriter
{
    public interface ITrekWriter
    {
        /// <summary>
        ///  Must reset all current writer settings
        /// </summary>
        void ResetWriter();

        /// <summary>
        ///  perform checking is requested trek must be download
        /// </summary>
        /// <param name="nodeMark">Imei of node</param>
        /// <param name="desc">Trek descriptor</param>
        /// <returns></returns>
        bool TrekCanBeWrite(string nodeMark, TrekDescriptor desc);


        /// <summary>
        /// Perform writing notes from param
        /// </summary>
        /// <param name="notes">List of notes</param>
        /// <param name="start">flag indicates need start new writing</param>
        /// <returns>Current count of written notes</returns>
        Int32 WriteNotes(List<NaviNote> notes, bool start);
    }
}
