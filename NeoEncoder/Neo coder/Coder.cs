namespace NeoEncoder.Neo_coder;

public delegate void InfoSize(int sizeBegin, int sizeEnd);
public class Coder
{

    private Compress c = new Compress();

    public InfoSize E_infoSize;

    public void EncodeRLEBWT(byte[] data, string namePath)
    {
        int sB = data.Length;
        data = c.RLECom(c.TransformBWT(data));
        namePath = $"{namePath}.neo";
        File.WriteAllBytes(namePath, data);
        E_infoSize?.Invoke(sB,data.Length);
    }

    public void DecodeRLEBWT(byte[] data, string namePath)
    {
        string[] txt = namePath.Split('.');
        namePath = namePath.Remove(namePath.Length - txt[txt.Length - 1].Length);
        File.WriteAllBytes(namePath, c.InverseTransformBWT(c.RLEDecom(data)));
    }

    public void EncodeLZWBWT(byte[] data, string namePath)
    {
        int sB = data.Length;
        data = c.LZWCom(c.TransformBWT(data));
        namePath = $"{namePath}.neo";
        File.WriteAllBytes(namePath, data);
        E_infoSize?.Invoke(sB, data.Length);
    }

    public void DecodeLZWBWT(byte[] data, string namePath)
    {
        string[] txt = namePath.Split('.');
        namePath = namePath.Remove(namePath.Length - txt[txt.Length - 1].Length);
        File.WriteAllBytes(namePath, c.InverseTransformBWT(c.LZWDecom(data)));
    }

    public void EncodeLZW(byte[] data, string namePath)
    {
        int sB = data.Length;
        data = c.LZWCom(data);
        namePath = $"{namePath}.neo";
        File.WriteAllBytes(namePath, data);
        E_infoSize?.Invoke(sB, data.Length);
    }

    public void DecodeLZW(byte[] data, string namePath)
    {
        string[] txt = namePath.Split('.');
        namePath = namePath.Remove(namePath.Length - txt[txt.Length - 1].Length);
        File.WriteAllBytes(namePath, c.LZWDecom(data));
    }

}
