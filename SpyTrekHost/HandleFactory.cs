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
        static IFrameProccesor m_trek;
        static InfoProcessor m_info;

        static IFrameProccesor m_error;

        static IFrameProccesor m_firmware;
        static ReadProcessorFactory()
        {
            m_info = new InfoProcessor();
            m_trek = new ReadProcessor("Trek");
            m_error = new ErrorProcessor();
        }


        public static IFrameProccesor GetErrorProcessor() => m_error;


        public static IFrameProccesor GetFirmwareProcessor(Piper piper, string path_to_image)
        {
            if (m_firmware == null)
            {
                m_firmware = new FirmwareProcessor(piper, path_to_image);
            }
            return m_firmware;
        }
    }


    internal class ReadProcessor : IFrameProccesor
    {
        public Int32 HeadCount;
        public Int32 DataCount;
        public Int32 ErrorCount;

        string Name;

        public ReadProcessor(string name)
        {
            Name = name;
            ReInit();
        }

        void ReInit()
        {
            HeadCount = DataCount = ErrorCount = 0;
        }


        public void Process(FramePacket packet, ref IStreamData answer, out State state)
        {
            state = State.Idle;

            if (packet.Opc == OpCodes.RRQ)
            {
                PrintResult("RRQ");
                HeadCount++;
            }
            else if (packet.Opc == OpCodes.DATA)
            {
                PrintResult("DATA");
                DataCount++;
                answer = new FramePacket(opc: OpCodes.ACK, id: packet.Id, data: null);
            }

            else
            {
                PrintResult("ERROR");
                ErrorCount++;
            }

        }

        private void PrintResult(string message)
        {
            Debug.WriteLine($"{Name} : type - {message}");
        }


    }

    internal class ErrorProcessor : IFrameProccesor
    {
        public void Process(FramePacket packet, ref IStreamData answer, out State state)
        {
            state = State.Idle;
            Debug.WriteLine($"Error packet received!!!");
            Console.WriteLine($"Error packet received!!!");
        }
    }
}
