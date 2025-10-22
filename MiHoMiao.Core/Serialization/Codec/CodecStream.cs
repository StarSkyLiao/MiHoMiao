using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MiHoMiao.Core.Serialization.Codec;

/// <summary>
/// 基于 ArrayPool.Shared 的字节流.
/// 这个类型是用于 Codec 编码的.
/// </summary>
public sealed class CodecStream(int initialCapacity = 16) : IDisposable
{
    private byte[] m_Buffer = ArrayPool<byte>.Shared.Rent(initialCapacity);

    public int Position { get; set; }

    public int Length { get; private set; }

    public CodecStream(ReadOnlySpan<char> content) : this(2 * content.Length)
    {
        int byteLen = content.Length * sizeof(char);
        EnsureCapacity(byteLen);
        ref byte src = ref Unsafe.As<char, byte>(ref Unsafe.AsRef(in content[0]));
        Unsafe.CopyBlockUnaligned(ref m_Buffer[0], ref src, (uint)byteLen);
        Length = byteLen;
    }
    
    public CodecStream(ReadOnlySpan<byte> content) : this(2 * content.Length)
    {
        EnsureCapacity(content.Length);
        content.CopyTo(m_Buffer.AsSpan());
        Length = Position;
    }

    public void ResetPosition() => Position = 0;
    
    public unsafe void Write<T>(T value) where T : unmanaged
    {
        int size = sizeof(T);
        EnsureCapacity(Position + size);
        ref byte dst = ref m_Buffer[Position];
        Unsafe.WriteUnaligned(ref dst, value);
        Position += size;
        if (Position > Length) Length = Position;
    }
    
    public unsafe void Write<T>(ReadOnlySpan<T> value) where T : unmanaged
    {
        Write(value.Length);
        int byteLen = value.Length * sizeof(T);
        EnsureCapacity(Position + byteLen);
        ref byte dst = ref m_Buffer[Position];
        ref byte src = ref Unsafe.As<T, byte>(ref Unsafe.AsRef(in value[0]));
        Unsafe.CopyBlockUnaligned(ref dst, ref src, (uint)byteLen);
        Position += byteLen;
        if (Position > Length) Length = Position;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe T Read<T>() where T : unmanaged
    {
        int size = sizeof(T);
        if (Position + size > Length) throw new EndOfStreamException();
        ref byte src = ref m_Buffer[Position];
        T result = Unsafe.ReadUnaligned<T>(ref src);
        Position += size;
        return result;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe Span<T> ReadSpan<T>() where T : unmanaged
    {
        int length = Read<int>();
        int size = sizeof(T);
        if (Position + length * size > Length) throw new EndOfStreamException();
        ref byte src = ref m_Buffer[Position];
        ref T result = ref Unsafe.As<byte, T>(ref src);
        Position += length * size;
        return new Span<T>(Unsafe.AsPointer(ref result), length);
    }

    public override string ToString()
    {
        ref char firstChar = ref Unsafe.As<byte, char>(ref m_Buffer[0]);
        return new string(MemoryMarshal.CreateReadOnlySpan(ref firstChar, m_Buffer.Length / sizeof(char)));
    }
    
    private void EnsureCapacity(int needed)
    {
        ObjectDisposedException.ThrowIf(m_Buffer is null, typeof(CodecStream));
        if (needed <= m_Buffer.Length) return;

        int next = Math.Max(needed, m_Buffer.Length * 2);
        byte[] old = m_Buffer;
        m_Buffer = ArrayPool<byte>.Shared.Rent(next);
        old.AsSpan(0, Length).CopyTo(m_Buffer);
        ArrayPool<byte>.Shared.Return(old, clearArray: true);
    }

    public void Dispose()
    {
        byte[] buf = Interlocked.Exchange(ref m_Buffer, null!);
        ArrayPool<byte>.Shared.Return(buf, clearArray: true);
        Position = Length = 0;
    }
}