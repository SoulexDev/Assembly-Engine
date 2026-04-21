using OpenTK.Mathematics;

namespace AssemblyEngine.Data
{
    public struct BitBuffer
    {
        public byte[] buffer;

        public BitBuffer(int sizeInBits)
        {
            buffer = new byte[sizeInBits / 8];
        }
        public void SetBits(long value, int index, int length)
        {
            int arrIndex = index / 8;

            if (arrIndex > buffer.Length)
            {
                Console.WriteLine("Index is out of range of bit buffer");
                return;
            }

            //if length is 9 bits, then span should be 2 bytes(arrSpan = 2)
            int arrSpan = (int)MathHelper.Ceiling(length / 8.0f);

            index -= arrIndex;

            for (int i = 0; i < arrSpan; i++)
            {
                byte bits = buffer[i + arrIndex];
                bits &= (byte)~(1 << index);
            }
        }
    }
}
