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
            m_note = new TrekDescriptionProcessor();
            m_trek = new ReadProcessor("Trek");

            m_error = new ErrorProcessor();
        }

        public static IFrameProccesor GetTrekProcessor() => m_trek;

        public static IFrameProccesor GetNoteProcessor() => m_note;

        public static IFrameProccesor GetInfoProcessor() => m_info;

        public static IFrameProccesor GetErrorProcessor() => m_error;
    }

    internal class TrekDescriptionProcessor : IFrameProccesor
    {
        public void Process(FramePacket packet, ref IStreamData answer)
        {
            if (packet.Opc == OpCodes.DATA)
            {
                ProcessTrekDescriptors(packet.Data, packet.Id);
                answer = new FramePacket(opc: OpCodes.ACK, id: packet.Id, data: null);
            }
        }

        private void ProcessTrekDescriptors(Byte[] data, UInt16 block_num)
        {
            TrekDescriptor desc = new TrekDescriptor();

            Int32 current_offset = 0;

            while (desc.TryParse(data, current_offset) == true)
            {
                current_offset += TrekDescriptor.Length;
                Console.WriteLine(desc.ToString());
            }
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
