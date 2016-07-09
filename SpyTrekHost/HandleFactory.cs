using MessageHandler.Abstract;
using StreamHandler.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageHandler;
using System.Diagnostics;

namespace SpyTrekHost
{
    static class ReadProcessorFactory
    {
        static IFrameProccesor m_trek;
        static IFrameProccesor m_info;
        static IFrameProccesor m_note;

        static IFrameProccesor m_error;

        static ReadProcessorFactory()
        {
            m_info = new ReadProcessor("Info");
            m_note = new ReadProcessor("Note");
            m_trek = new ReadProcessor("Trek");

            m_error = new ErrorProcessor();
        }

        public static IFrameProccesor GetTrekProcessor() => m_trek;

        public static IFrameProccesor GetNoteProcessor() => m_note;

        public static IFrameProccesor GetInfoProcessor() => m_info;

        public static IFrameProccesor GetErrorProcessor() => m_error;
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


        public void Process(FramePacket packet, ref IStreamData answer)
        {
            answer = null;
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
        public void Process(FramePacket packet, ref IStreamData answer)
        {
            Debug.WriteLine($"Error packet received!!!");
            Console.WriteLine($"Error packet received!!!");
        }
    }
}
