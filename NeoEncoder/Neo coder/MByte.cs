using System.Text;

namespace NeoEncoder.Neo_coder
{
    public struct MByte
    {
        public MByte()
            => bytes = null;

        public MByte(byte b)
        {
            bytes = new byte[1];
            bytes[0] = b;
        }

        private MByte(MByte m, byte b)
        {
            bytes = new byte[m.Count + 1];
            for (int i = 0; i < m.Count; i++)
            {
                bytes[i] = m[i];
            }
            bytes[m.Count] = b;
        }

        public int Count
        {
            get => bytes is null ? 0 : bytes.Length;
        }

        public byte[]? bytes;

        public byte this[int a]
        {
            get => bytes[a];
            set => bytes[a] = value;
        }

        private static readonly EqualityComparer<byte> elementComparer = EqualityComparer<byte>.Default;

        public ushort Get()
            => bytes.Length > 1 ? BitConverter.ToUInt16(bytes) : bytes[0];

        public static MByte operator +(MByte l, byte r)
            => l.Count == 0 ? new MByte(r) : new MByte(l, r);

        public override bool Equals(object obj)
            => GetHashCode() == obj.GetHashCode();

        public override int GetHashCode()
        {
            if (bytes == null)
            {
                return 0;
            }
            int hash = 17;
            foreach (byte element in bytes)
            {
                hash = hash * 32677 + elementComparer.GetHashCode(element);
            }
            return hash;
        }

        public override string ToString()
            => bytes is null ? "null" : Encoding.UTF8.GetString(bytes);
    }
}
