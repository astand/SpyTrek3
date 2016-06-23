using System;

namespace StreamHandler
{
    public class ResizeableArray
    {
        private const Int32 MAX_CAPACITY  = 3000;

        private Int32 m_MaxIndex;

        private Byte[] m_Array;

        public Int32 Length { get; private set; }

        public byte this[int indx]
        {
            get
            {
                if (IndexOutOfRange(indx))
                    throw new IndexOutOfRangeException
                        ($"Index cannot be less 0 and more then current length of array. Value: {indx}");
                return m_Array[indx];
            }
        }

        public ResizeableArray(Int32 size = 0)
        {
            if (size < 1 || size > MAX_CAPACITY)
            {
                size = MAX_CAPACITY;
            }
            m_MaxIndex = size;
            m_Array = new Byte[size];
            Flush();
        }
        
        public Byte[] GetByteArray()
        {
            var outbuf = new Byte[Length];
            Array.Copy(m_Array, outbuf, outbuf.Length);

            return outbuf;
        }

        public void AddByte(Byte bt)
        {
            if (IndexCannotBeIncrement(Length))
                throw new IndexOutOfRangeException
                    ($"Index cannot be more less 0 or more then maximum availibe capacity. Value: {Length}");

            m_Array[Length++] = bt;
        }

        public void Flush() => Length = 0;

        private Boolean IndexOutOfRange(Int32 index) => ((index < 0) || (index >= Length));

        private Boolean IndexCannotBeIncrement(Int32 index) => (index < 0 || index >= m_MaxIndex);
    }
}