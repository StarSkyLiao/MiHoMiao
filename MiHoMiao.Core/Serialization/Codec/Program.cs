namespace MiHoMiao.Core.Serialization.Codec;

public static class Program
{
    public static void Main()
    {
        var original = new PlayerData(Guid.NewGuid(), 99);

        Span<byte> buf = stackalloc byte[33];
        Span<byte> writable = buf;
        CodecEncoder.Encode(ref writable, original);

        Span<byte> readable = buf;
        CodecEncoder.Decode(ref readable, out PlayerData decoded);

        Console.WriteLine($"Original: {original}");
        Console.WriteLine($"Decoded : {decoded}");
        Console.WriteLine($"Equal   : {original == decoded}");
    }
}