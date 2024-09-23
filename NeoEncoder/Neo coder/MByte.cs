using System.Security.Cryptography;

namespace NeoEncoder.Neo_coder
{
    public struct MByte
    {
        public MByte()
            => bytes = null;

        public MByte(byte b)
            => bytes = [b];

        private MByte(MByte m, byte b)
        {
            bytes = new byte[m.Count + 1];
            for (int i = 0; i < m.Count; i++)
                bytes[i] = m[i];
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

        public static MByte operator +(MByte l, byte r)
            => l.Count == 0 ? new MByte(r) : new MByte(l, r);

        public override bool Equals(object obj)
        {
            if (obj is MByte) 
            {
                MByte m = (MByte)obj;
                if (m.Count == Count)
                {
                    for (int i = 0; i < bytes.Length; i++)
                        if (m[i] != bytes[i])
                            goto End;
                    return true;
                }
            }
            End:
            return false;
        }

        public override int GetHashCode()
        {
            if (bytes is null) return 0;
            using (SHA1Managed sHA1 = new SHA1Managed())
                return BitConverter.ToInt32(sHA1.ComputeHash(bytes), 0);
        }

    }
}
