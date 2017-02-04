using MessageHandler.Abstract;
using StreamHandler.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageHandler;
using System.Diagnostics;
using MessageHandler.DataFormats;
using StreamHandler;
using MessageHandler.Processors;
using MessageHandler.Notifiers;

namespace SpyTrekHost
{
    static class ReadProcessorFactory
    {
        //static InfoProcessor m_info;
        ////static IFrameProccesor m_error;
        //static IFrameProccesor m_firmware;
        //static ReadProcessorFactory()
        //{
        //    m_info = new InfoProcessor();
        //}


        //public static IFrameProccesor GetFirmwareProcessor(Piper piper, string path_to_image)
        //{
        //    if (m_firmware == null)
        //    {
        //        m_firmware = new FirmwareProcessor(piper, path_to_image);
        //    }
        //    return m_firmware;
        //}
    }
}
