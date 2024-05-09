namespace NeoEncoder.Neo_coder
{
    public delegate void InfoSize(int sizeBegin, int sizeEnd);
    public class Coder
    {

        private Compress compress = new Compress();

        public InfoSize E_infoSize;

        public void EncodeRLEBWT(byte[] data, string namePath)
        {
            int sB = data.Length;
            data = compress.TransformBWT(data);
            data = compress.RLECom(data);
            namePath = $"{namePath}.neo";
            File.WriteAllBytes(namePath, data);
            E_infoSize?.Invoke(sB,data.Length);
        }

        public void DecodeRLEBWT(byte[] data, string namePath)
        {
            string[] txt = namePath.Split('.');
            data = compress.RLEDecom(data);
            data = compress.InverseTransformBWT(data);
            namePath = namePath.Remove(namePath.Length - txt[txt.Length - 1].Length);
            File.WriteAllBytes(namePath, data);
        }
        public void EncodeLZWBWT(byte[] data, string namePath)
        {
            int sB = data.Length;
            data = compress.TransformBWT(data);
            data = compress.LZWCom(data);
            namePath = $"{namePath}.neo";
            File.WriteAllBytes(namePath, data);
            E_infoSize?.Invoke(sB, data.Length);
        }
        public void DecodeLZWBWT(byte[] data, string namePath)
        {
            string[] txt = namePath.Split('.');
            data = compress.LZWDecom(data);
            data = compress.InverseTransformBWT(data);
            namePath = namePath.Remove(namePath.Length - txt[txt.Length - 1].Length);
            File.WriteAllBytes(namePath, data);
        }

        public void EncodeLZW(byte[] data, string namePath)
        {
            int sB = data.Length;
            data = compress.LZWCom(data);
            namePath = $"{namePath}.neo";
            File.WriteAllBytes(namePath, data);
            E_infoSize?.Invoke(sB, data.Length);
        }

        public void DecodeLZW(byte[] data, string namePath)
        {
            string[] txt = namePath.Split('.');
            data = compress.LZWDecom(data);
            namePath = namePath.Remove(namePath.Length - txt[txt.Length - 1].Length);
            File.WriteAllBytes(namePath, data);
        }


    }
}
