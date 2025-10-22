using System.Buffers;
using System.Numerics;
using System.Runtime.CompilerServices;
using MiHoMiao.Core.Numerics.Values;

namespace MiHoMiao.Core.Collections.Unsafe;

public ref struct InterpolatedString : IDisposable
{
    /// <summary>
    /// 用于存储字符数据的数组，从 <see cref="T:System.Buffers.ArrayPool`1"/> 租用
    /// </summary>
    private char[]? m_InternalCharArray;
    
    /// <summary>
    /// 表示当前字符数据的可写跨度
    /// </summary>
    private Span<char> m_CharSpan;

    /// <summary>
    /// 记录当前已写入的字符长度
    /// </summary>
    public int Length { get; private set; }

    public InterpolatedString()
    {
        Length = 0;
        m_InternalCharArray = ArrayPool<char>.Shared.Rent(8);
        m_CharSpan = m_InternalCharArray.AsSpan();
    }
    
    /// <summary>
    /// 初始化一个指定容量的 InterpolatedString 实例.
    /// 初始容量不得低于 8.
    /// </summary>
    public InterpolatedString(int capacity = 8)
    {
        Length = 0;
        m_InternalCharArray = ArrayPool<char>.Shared.Rent(capacity.Min(8));
        m_CharSpan = m_InternalCharArray.AsSpan();
    }
    
    /// <summary>
    /// 使用指定的 string 初始化 InterpolatedString 实例.
    /// </summary>
    public InterpolatedString(string initialString)
    {
        Length = initialString.Length;
        m_InternalCharArray = ArrayPool<char>.Shared.Rent(Length.Min(1 << (BitOperations.Log2((uint)Length) + 1)));
        m_CharSpan = m_InternalCharArray.AsSpan();
        initialString.CopyTo(m_CharSpan);
    }

    /// <summary>
    /// 获取当前已写入的字符跨度
    /// </summary>
    private Span<char> Text => m_CharSpan[..Length];
    
    /// <summary>
    /// 将当前字符跨度转换为字符串
    /// </summary>
    public override string ToString() => new string(Text);
    
    /// <summary>
    /// 将当前字符跨度转换为字符串并清除内部状态
    /// </summary>
    /// <returns>表示当前内容的字符串</returns>
    /// <remarks>
    /// 此方法会释放所有资源，仅应在最后调用一次。
    /// 后续使用此实例的行为未定义，可能导致进程不稳定。
    /// </remarks>
    public string ToStringAndClear()
    {
        string result = new string(Text);
        Clear();
        return result;
    }
    
    /// <summary>
    /// 清除实例状态并将租用的数组归还到ArrayPool
    /// </summary>
    /// <remarks>
    /// 此方法会释放所有资源，仅应在最后调用一次。
    /// 后续使用此实例的行为未定义，可能导致进程不稳定。
    /// </remarks>
    public void Clear()
    {
        char[]? toReturn = m_InternalCharArray;
        this = default; // defensive clear
        if (toReturn is not null) ArrayPool<char>.Shared.Return(toReturn);
    }
    
    /// <summary>
    /// 安全地清空, 仅仅只清空
    /// </summary>
    public void SafeClear() => Length = 0;

    #region AppendMethods
    
    /// <summary>
    /// 将指定值追加到字符跨度
    /// </summary>
    public void Append(byte value) => AppendFormatted(value);
    
    /// <summary>
    /// 将指定值追加到字符跨度
    /// </summary>
    public void Append(sbyte value) => AppendFormatted(value);
    
    /// <summary>
    /// 将指定值追加到字符跨度
    /// </summary>
    public void Append(char value) => AppendFormatted(value);
    
    /// <summary>
    /// 将指定值追加到字符跨度
    /// </summary>
    public void Append(short value) => AppendFormatted(value);
    
    /// <summary>
    /// 将指定值追加到字符跨度
    /// </summary>
    public void Append(ushort value) => AppendFormatted(value);
    
    /// <summary>
    /// 将指定值追加到字符跨度
    /// </summary>
    public void Append(int value) => AppendFormatted(value);
    
    /// <summary>
    /// 将指定值追加到字符跨度
    /// </summary>
    public void Append(uint value) => AppendFormatted(value);
    
    /// <summary>
    /// 将指定值追加到字符跨度
    /// </summary>
    public void Append(long value) => AppendFormatted(value);
    
    /// <summary>
    /// 将指定值追加到字符跨度
    /// </summary>
    public void Append(ulong value) => AppendFormatted(value);
    
    /// <summary>
    /// 将指定值追加到字符跨度
    /// </summary>
    public void Append(float value) => AppendFormatted(value);
    
    /// <summary>
    /// 将指定值追加到字符跨度
    /// </summary>
    public void Append(double value) => AppendFormatted(value);
    
    /// <summary>
    /// 将指定值追加到字符跨度
    /// </summary>
    public void Append(decimal value) => AppendFormatted(value);
    
    /// <summary>
    /// 将指定字符串追加到字符跨度
    /// </summary>
    public void Append(string value)
    {
        if (value.TryCopyTo(m_CharSpan[Length..])) Length += value.Length;
        else GrowThenCopyString(value);
    }
    
    /// <summary>
    /// 将新的一行追加到字符跨度
    /// </summary>
    public void AppendLine() => Append('\n');

    /// <summary>
    /// 将指定字符串追加到字符跨度
    /// </summary>
    public void AppendLine(string value)
    {
        if (value.TryCopyTo(m_CharSpan[Length..])) Length += value.Length;
        else GrowThenCopyString(value);
        Append('\n');
    }
    
    /// <summary>
    /// 将指定字符跨度追加到字符跨度
    /// </summary>
    public void Append(scoped ReadOnlySpan<char> value)
    {
        if (value.TryCopyTo(m_CharSpan[Length..])) Length += value.Length;
        else GrowThenCopySpan(value);
    }
    
    /// <summary>
    /// 将指定字符跨度追加到字符跨度
    /// </summary>
    public void Append(scoped Span<char> value)
    {
        if (value.TryCopyTo(m_CharSpan[Length..])) Length += value.Length;
        else GrowThenCopySpan(value);
    }
    
    /// <summary>
    /// 将指定内存跨度追加到字符跨度
    /// </summary>
    public void Append(Memory<char> value)
    {
        Span<char> span = value.Span;
        if (span.TryCopyTo(m_CharSpan[Length..])) Length += span.Length;
        else GrowThenCopySpan(span);
    }
    
    /// <summary>
    /// 将指定内存跨度追加到字符跨度
    /// </summary>
    public void Append(ReadOnlyMemory<char> value)
    {
        ReadOnlySpan<char> span = value.Span;
        if (span.TryCopyTo(m_CharSpan[Length..])) Length += span.Length;
        else GrowThenCopySpan(span);
    }
    
    /// <summary>
    /// 将指定内存跨度追加到字符跨度
    /// </summary>
    public void AppendLine(ReadOnlyMemory<char> value)
    {
        Append(value);
        Append('\n');
    }
    
    /// <summary>
    /// 将指定值追加到字符跨度
    /// </summary>
    public void Append<T>(T value) where T : notnull
    {
        if (value is ISpanFormattable formattable) AppendFormatted(formattable);
        else Append(value.ToString()!);
    }

    /// <summary>
    /// 将指定值追加到字符跨度
    /// </summary>
    public void AppendFormatted<T>(T value) where T : ISpanFormattable
    {
        int charsWritten;
        while (!value.TryFormat(m_CharSpan[Length..], out charsWritten, default, null)) 
            GrowCore(m_CharSpan.Length << 1);
        Length += charsWritten;
    }

    #endregion

    #region InsertMethods
    
    /// <summary>
    /// 在指定索引处插入一个字符
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Insert(int index, char value)
    {
        if (index < 0 || index > Length) throw new ArgumentOutOfRangeException(nameof(index));

        int newLength = Length + 1;
        if (newLength > m_CharSpan.Length) GrowCore(m_CharSpan.Length << 1);

        Span<char> span = m_CharSpan;
        span[index..Length].CopyTo(span[(index + 1)..]);
        span[index] = value;
        Length = newLength;
    }

    /// <summary>
    /// 在指定索引处插入一个字符串
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Insert(int index, string value) => Insert(index, value.AsSpan());

    /// <summary>
    /// 在指定索引处插入一个 ReadOnlySpan
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Insert(int index, scoped ReadOnlySpan<char> value)
    {
        if (index < 0 || index > Length) throw new ArgumentOutOfRangeException(nameof(index));

        int valueLength = value.Length;
        if (valueLength == 0) return;

        int newLength = Length + valueLength;
        if (newLength > m_CharSpan.Length) GrowCore(m_CharSpan.Length << 1);

        Span<char> span = m_CharSpan;
        span[index..Length].CopyTo(span[(index + valueLength)..]);

        if (valueLength <= 2)
        {
            span[index] = value[0];
            if (valueLength == 2) span[index + 1] = value[1];
        }
        else
        {
            value.CopyTo(span[index..]);
        }

        Length = newLength;
    }

    /// <summary>
    /// 在指定索引处插入一个对象（调用 ToString()）
    /// </summary>
    public void Insert<T>(int index, T value) where T : notnull => Insert(index, value.ToString()!);
    
    /// <summary>
    /// 在指定索引处插入一个实现了 ISpanFormattable 的对象，避免中间字符串分配。
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void InsertFormattable<T>(int index, T value) where T : ISpanFormattable
    {
        if (index < 0 || index > Length) throw new ArgumentOutOfRangeException(nameof(index));
        
        Span<char> buffer = stackalloc char[Length - index];
        m_CharSpan[index..Length].CopyTo(buffer);
        
        Span<char> destination = m_CharSpan[index..];
        if (value.TryFormat(destination, out int written, default, null))
        {
            Length = index + written;
            Append(buffer);
        }
        else
        {
            Insert(index, value);
        }
    }
    
    #endregion
    
    /// <summary>
    /// 当空间不足时，扩展字符跨度并复制字符串
    /// </summary>
    private void GrowThenCopyString(string value)
    {
        GrowCore(1 << (BitOperations.Log2((uint)Length + (uint)value.Length) + 1));
        value.CopyTo(m_CharSpan[Length..]);
        Length += value.Length;
    }
    
    /// <summary>
    /// 当空间不足时，扩展字符跨度并复制字符串
    /// </summary>
    private void GrowThenCopySpan(scoped ReadOnlySpan<char> value)
    {
        GrowCore(1 << (BitOperations.Log2((uint)value.Length) + 1));
        value.CopyTo(m_CharSpan[Length..]);
        Length += value.Length;
    }
    
    /// <summary>
    /// 当空间不足时，扩展字符跨度并复制字符串
    /// </summary>
    private void GrowThenCopySpan(scoped Span<char> value)
    {
        GrowCore(1 << (BitOperations.Log2((uint)value.Length) + 1));
        value.CopyTo(m_CharSpan[Length..]);
        Length += value.Length;
    }
    
    /// <summary>
    /// 扩展字符跨度以确保足够容量
    /// </summary>
    private void GrowCore(int requiredMinCapacity)
    {
        int newCapacity = (m_CharSpan.Length << 1).Min(requiredMinCapacity);
        int arraySize = newCapacity.Clamp(4, int.MaxValue);

        char[] newArray = ArrayPool<char>.Shared.Rent(arraySize);
        m_CharSpan[..Length].CopyTo(newArray);

        char[]? toReturn = m_InternalCharArray;
        m_CharSpan = m_InternalCharArray = newArray;

        if (toReturn is not null) ArrayPool<char>.Shared.Return(toReturn);
    }

    public void Dispose() => Clear();
    
}