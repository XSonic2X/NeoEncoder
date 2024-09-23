namespace NeoEncoder.Neo_coder;

public class Matrix(byte[] _bytes, int _offset)
{
    public int Length { get => bytes.Length; }
    public readonly int offset = _offset;//Смещение матрицы

    private byte[] bytes = _bytes;

    public byte this[int index]
    {
        get
        {
            index += offset;
            return index < bytes.Length ? bytes[index] : bytes[index - bytes.Length];
        }
    }

    public byte Last() 
        => this[Length - 1];

    public bool SequenceEqual() 
        => offset == 0;

}
