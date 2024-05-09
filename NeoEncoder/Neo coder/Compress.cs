namespace NeoEncoder.Neo_coder
{
    public class Compress
    {

        private const int tableSize = 65353; // 64K

        public byte[] RLECom(byte[] bytes) 
        {
            List<byte> lbytes = new List<byte>();
            byte currentByte = bytes[0];
            int count = 0;

            for (int i = 1; i < bytes.Length; i++)
            {
                if (bytes[i] == currentByte)
                {
                    count++;
                    if (count > 255)
                    {
                        SetListRLE(lbytes, currentByte, 255);
                        count = 0;
                    }
                }
                else
                {
                    SetListRLE(lbytes, currentByte, (byte)count);
                    currentByte = bytes[i];
                    count = 0;
                }
            }

            SetListRLE(lbytes, currentByte, (byte)count);

            return lbytes.ToArray();
        }

        public byte[] RLEDecom(byte[] bytes)
        {
            List<byte> lbytes = new List<byte>();
            byte b, count;
            for (int i = 0, j; i < bytes.Length; )
            {
                b = bytes[i];
                i++;
                count = bytes[i];
                i++;
                for (j = 0; j <= count; j++)
                {
                    lbytes.Add(b);
                }
            }

            return lbytes.ToArray();
        }
        
        public byte[] TransformBWT(byte[] bytes)
        {
            Matrix[] m = new Matrix[bytes.Length];

            for (int i = 0; i < bytes.Length; i++)//Создаю матрицы
            {
                m[i] = new Matrix(bytes, i);
            }

            Array.Sort(m, (a, b) =>
            {
                if (a == b) { return 0;  }
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i] < b[i]) return -1;
                    if (a[i] > b[i]) return 1;
                }
                return 0;
            });//Сортируют матрицы

            List<byte> bwt = new List<byte>();
            int offset = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                bwt.Add(m[i].Last());
                if (m[i].SequenceEqual()) { offset = i; } //Стартового смещение
            }

            bwt.AddRange(BitConverter.GetBytes(offset));//Запись стартового смещение

            return bwt.ToArray();
        }

        public byte[] InverseTransformBWT(byte[] b)
        {
            byte[] bytesOffset = new byte[4];
            Array.Copy(b, b.Length - 4, bytesOffset,0 ,4);
            int offset = BitConverter.ToInt32(bytesOffset); //Получение стартового смещение

            byte[] bwt = new byte[b.Length - 4];
            Array.Copy(b,0, bwt,0, b.Length - 4);

            (int, byte)[] offsetBWT = new (int, byte)[bwt.Length];
            for (int i = 0; i < bwt.Length; i++) //Сборка смешных байтов
            {
                offsetBWT[i].Item1 = i;
                offsetBWT[i].Item2 = bwt[i];
            }

            Array.Sort(offsetBWT,
                (a, b) =>
                {
                    int next = a.Item2 - b.Item2;
                    return next == 0 ? a.Item1 - b.Item1 : next;
                });//Сортировка смешных байтов

            byte[] bytes = new byte[bwt.Length];
            for (int i = 0; i < bwt.Length; i++) //Сборка в исходных данных
            {
                bytes[i] = offsetBWT[offset].Item2;
                offset = offsetBWT[offset].Item1;
            }

            return bytes;
        }

        public byte[] LZWCom(byte[] bytes)
        {
            Dictionary<MByte, ushort> dictionary = new Dictionary<MByte, ushort>();
            for (ushort i = 0; i < 256; i++) //Инициализируем стартовый словарь
            {
                dictionary.Add(new MByte((byte)i), i);
            }

            List<ushort> ushorts = new List<ushort>();
            MByte p = new MByte(), wc;
            foreach (byte c in bytes)//Создания словарь и сжатых данных
            {
                wc = p + c;
                if (dictionary.ContainsKey(wc))
                {
                    p = wc;
                }
                else
                {
                    ushorts.Add(dictionary[p]);
                    if (dictionary.Count < tableSize)
                    {
                        dictionary.Add(wc, (ushort)dictionary.Count);//Добавление нового слова в словарь
                    }
                    p = new MByte(c);
                }
            }
            if (p.Count > 0) { ushorts.Add(dictionary[p]); }

            List<byte> data = new List<byte>();//Преобразование сжатых данных в byte
            for (int i = 0; i < ushorts.Count; i++)
            { data.AddRange(BitConverter.GetBytes(ushorts[i])); }
            return data.ToArray();
        }

        public byte[] LZWDecom(byte[] bytes)
        {
            List<ushort> ushorts = new List<ushort>();
            ushort k;

            for (int i = 0; i < bytes.Length;)//Обратное преобразование byte в ushort
            {
                k = bytes[i];
                i++;
                k += (ushort)(256 * bytes[i]);
                i++;
                ushorts.Add(k);
            }

            Dictionary<ushort, MByte> dictionary = new Dictionary<ushort, MByte>();
            for (ushort i = 0; i < 256; i++)//Инициализируем стартовый словарь
            {
                dictionary.Add(i, new MByte((byte)i));
            }

            MByte w = dictionary[ushorts.First()], entry;
            List<byte> lresult = new List<byte>();

            lresult.AddRange(w.bytes);

            for (int i = 1; i < ushorts.Count; i++)
            {
                k = ushorts[i];
                if (dictionary.ContainsKey(k)) { entry = dictionary[k]; }
                else { entry = w + w[0]; }

                lresult.AddRange(entry.bytes);
                if (dictionary.Count < tableSize)
                {
                    dictionary.Add((ushort)dictionary.Count, w + entry[0]);
                }
                w = entry;

            }

            return lresult.ToArray();
        }

        private void SetListRLE(List<byte> lbytes, byte currentByte, byte count)
        {
            lbytes.Add(currentByte);
            lbytes.Add((byte)count);
        }

    }
}
