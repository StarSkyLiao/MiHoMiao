//------------------------------------------------------------
// MiHoMiao
// Written by Mingxuan Liao.
// [Version] 1.0
//------------------------------------------------------------


// 使用 Emit 来生成方法; 关闭时, 将使用反射.
// 反射的性能在高压力下稍逊一筹; 一般情形下两者性能相当.
#undef UseEmit

#if UseEmit
using System.Reflection.Emit;
#endif
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


[assembly: InternalsVisibleTo("MiHoMiao.xUnit")]

namespace MiHoMiao.Core.Collections.Specialized;

public ref struct DynamicString : IDisposable
{
    /// <summary>
    /// 底层字符串的长度的比特位最小值.
    /// 默认为 4, 即字符串至少需要 2^4=16 个字符.
    /// </summary>
    internal const int MinBitLengthOfStringLength = 4;
    
    internal int RawStringLength;
    
    internal int CurrStringLength;
    
    internal Span<char> StringSpan;

    internal Action<string>? ReleaseAction;
    
    internal string StringValue;

    public int Length => CurrStringLength;
    
    /// <summary>
    /// 创建默认的 DynamicString 对象
    /// </summary>
    public DynamicString()
    {
        StringValue = new string('\0', 1 << MinBitLengthOfStringLength);
        RawStringLength = 1 << MinBitLengthOfStringLength;
        StringSpan = AsSpan(StringValue.AsSpan());
    }

    /// <summary>
    /// 使用指定的长度创建 DynamicString 对象
    /// </summary>
    public DynamicString(int capacity, Action<string>? releaseAction = null)
    {
        StringValue = new string('\0', capacity);
        RawStringLength = capacity;
        StringSpan = AsSpan(StringValue.AsSpan());
        ReleaseAction = releaseAction;
    }
    
    /// <summary>
    /// 使用指定的长度创建 DynamicString 对象
    /// </summary>
    public DynamicString(int capacity)
    {
        StringValue = new string('\0', capacity);
        RawStringLength = capacity;
        StringSpan = AsSpan(StringValue.AsSpan());
    }
    
    /// <summary>
    /// 使用指定的字符串创建 DynamicString 对象
    /// </summary>
    public DynamicString(string rawString, Action<string>? releaseAction = null)
    {
        CurrStringLength = RawStringLength = rawString.Length;
        StringValue = rawString;
        StringSpan = AsSpan(rawString.AsSpan());
        ReleaseAction = releaseAction;
    }
    
    /// <summary>
    /// 读取字符串的内容.
    /// [Warning]: 返回的字符串不能被保存下来！
    /// 如果需要保存返回值, 参阅<see cref="ToString"/>方法.
    /// </summary>
    [Pure]
    public string Read()
    {
        if (StringValue.Length != CurrStringLength) SetLength(StringValue, CurrStringLength);
        return StringValue;
    }

    /// <summary>
    /// 返回字符串的拷贝副本.
    /// 需要保存字符串时应当使用这个方法.
    /// </summary>
    public override string ToString()
    {
        if (StringValue.Length != CurrStringLength) SetLength(StringValue, CurrStringLength);
        return new string(StringValue);
    }

    #region AppendMethods
    
    /// <summary>
    /// 在字符串的尾部插入一个字符
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(char value)
    {
        ref int oldLength = ref CurrStringLength;
        int newLength = oldLength + 1;
        if (newLength > RawStringLength) ExpandString(newLength);
        
        Unsafe.Add(ref MemoryMarshal.GetReference(StringSpan), oldLength++) = value;
    }

    /// <summary>
    /// 在字符串的尾部插入一个对象
    /// </summary>
    public void Append<T>(T value) where T: notnull => AppendString(value.ToString()!);
    
    /// <summary>
    /// 在字符串的尾部插入一个字符串
    /// </summary>
    public void Append(string value) => AppendString(value.AsSpan());
    
    /// <summary>
    /// 在字符串的尾部插入一个可以被 Span 格式化的对象
    /// </summary>
    public void AppendFormattable<T>(T value) where T : ISpanFormattable => AppendFormattableInternal(value);
    
    /// <summary>
    /// 在字符串的尾部插入指定的内容与新的一行
    /// </summary>
    public void AppendLine<T>(T value) where T: notnull
    {
        AppendString(value.ToString()!);
        Append('\n');
    }

    /// <summary>
    /// 在字符串的尾部插入指定可以被 Span 格式化的内容与新的一行
    /// </summary>
    public void AppendLineFormattable<T>(T value) where T : ISpanFormattable
    {
        AppendFormattableInternal(value);
        Append('\n');
    }

    /// <summary>
    /// 在字符串的尾部插入一个 DynamicString 对象
    /// </summary>
    public void Append(DynamicString value) => AppendString(value.StringValue);
    
    /// <summary>
    /// 在字符串的尾部插入指定的内容与新的一行
    /// </summary>
    public void AppendLine(DynamicString value)
    {
        AppendString(value.StringValue);
        Append('\n');
    }

    #endregion

    #region InsertMethods

    /// <summary>
    /// 在指定索引处插入一个字符
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Insert(int index, char value)
    {
        if (index < 0 || index > CurrStringLength) throw new ArgumentOutOfRangeException(nameof(index));

        int newLength = CurrStringLength + 1;
        if (newLength > RawStringLength) ExpandString(newLength);

        Span<char> span = StringSpan;
        span[index..CurrStringLength].CopyTo(span[(index + 1)..]);
        span[index] = value;
        CurrStringLength = newLength;
        
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
    public void Insert(int index, ReadOnlySpan<char> value)
    {
        if (index < 0 || index > CurrStringLength) throw new ArgumentOutOfRangeException(nameof(index));

        int valueLength = value.Length;
        if (valueLength == 0) return;

        int newLength = CurrStringLength + valueLength;
        if (newLength > RawStringLength) ExpandString(newLength);

        Span<char> span = StringSpan;
        span[index..CurrStringLength].CopyTo(span[(index + valueLength)..]);

        if (valueLength <= 2)
        {
            span[index] = value[0];
            if (valueLength == 2) span[index + 1] = value[1];
        }
        else
        {
            value.CopyTo(span[index..]);
        }

        CurrStringLength = newLength;
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
        if (index < 0 || index > CurrStringLength)
            throw new ArgumentOutOfRangeException(nameof(index));
        
        Span<char> buffer = stackalloc char[CurrStringLength - index];
        StringSpan[index..CurrStringLength].CopyTo(buffer);
        
        Span<char> destination = StringSpan[index..];
        if (value.TryFormat(destination, out int written, default, null))
        {
            CurrStringLength = index + written;
            AppendString(buffer);
        }
        else
        {
            Insert(index, value);
        }
    }

    #endregion
    
    /// <summary>
    /// 清空字符串的内容.
    /// </summary>
    public void Clear() => CurrStringLength = 0;

    /// <summary>
    /// 释放这个字符串
    /// </summary>
    public void Dispose()
    {
        if (StringValue == null) return;
        
        SetLength(StringValue, RawStringLength);
        RawStringLength = 0;
        StringSpan = default;
        string rawString = StringValue;
        StringValue = null!;

        if (ReleaseAction == null) return;
        ReleaseAction(rawString);
        ReleaseAction = null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AppendFormattableInternal<T>(T value) where T : ISpanFormattable
    {
        ref int length = ref CurrStringLength;
        Span<char> destination = StringSpan[length..];
        if (value.TryFormat(destination, out int written, default, null)) length += written;
        else AppendString(value.ToString().AsSpan());
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AppendString(scoped ReadOnlySpan<char> value)
    {
        int valueLength = value.Length;
        if (valueLength is 0) return;

        int oldLength = CurrStringLength;
        int newLength = oldLength + valueLength;
        if (newLength > RawStringLength) ExpandString(newLength);
        if (valueLength <= 2)
        {
            StringSpan[oldLength] = MemoryMarshal.GetReference(value);
            if (valueLength is 2)
            {
                Unsafe.Add(
                    ref MemoryMarshal.GetReference(StringSpan), oldLength + 1
                ) = Unsafe.Add(
                    ref MemoryMarshal.GetReference(value), 1
                );
            }
        }
        else value.CopyTo(StringSpan[oldLength..]);
        CurrStringLength = newLength;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ExpandString(int newLength)
    {
        int bitLength = BitOperations.Log2((uint)newLength) + 1;
        if (bitLength < MinBitLengthOfStringLength) bitLength = MinBitLengthOfStringLength;
        string newString = new string('\0', 1 << bitLength);
        SetLength(StringValue, RawStringLength);
        CopyTo(StringValue, newString);
        StringValue = newString;
        StringSpan = AsSpan(StringValue);
        RawStringLength = newString.Length;
    }

    #region Operators

    public static implicit operator ReadOnlySpan<char>(DynamicString dynamicString) 
        => dynamicString.StringSpan[..dynamicString.RawStringLength];
    
    public static explicit operator string(DynamicString dynamicString) 
        => dynamicString.ToString();
    
    public static implicit operator DynamicString(string input) 
        => new DynamicString(input);

    #endregion
    
    #region Strings

#if UseEmit
    
    private static readonly Action<string, int> s_SetLengthAction = CreateSetStringLength();

    /// <summary>
    /// 强制将 content 字符串的长度修改为 newLength.
    /// [Warning]: 不会进行任何检查.
    /// 这意味着 newLength 如果大于 content 的实际长度,
    /// 修改仍将发生, 此时继续访问 content 可能会读取到一些意料之外的内容.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void SetLength(string content, int newLength) => s_SetLengthAction(content, newLength);
    
    private static Action<string, int> CreateSetStringLength()
    {
        // 获取 string 类型中的私有字段 _stringLength
        FieldInfo stringLengthField = typeof(string).GetField(
            "_stringLength", BindingFlags.NonPublic | BindingFlags.Instance
        )!;
        
        // 创建动态方法
        DynamicMethod dynamicMethod = new DynamicMethod(
            nameof(SetLength), typeof(void), [typeof(string), typeof(int)]
        );

        // 获取 IL 生成器
        ILGenerator il = dynamicMethod.GetILGenerator();

        // IL 代码生成
        il.Emit(OpCodes.Ldarg_0);           // 加载 string 参数
        il.Emit(OpCodes.Ldarg_1);           // 加载 int 参数
        il.Emit(OpCodes.Stfld, stringLengthField); // 设置 _stringLength 字段
        il.Emit(OpCodes.Ret);               // 返回

        // 创建委托
        return (Action<string, int>)dynamicMethod.CreateDelegate(typeof(Action<string, int>));
    }

#else
    /// <summary>
    /// 强制将 content 字符串的长度修改为 newLength.
    /// [Warning]: 不会进行任何检查.
    /// 这意味着 newLength 如果大于 content 的实际长度,
    /// 修改仍将发生, 此时继续访问 content 可能会读取到一些意料之外的内容.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void SetLength(string content, int newLength) 
        => s_StringLengthField.SetValue(content, newLength);

    private static readonly FieldInfo s_StringLengthField = typeof(string).GetField(
        "_stringLength",
        BindingFlags.NonPublic | BindingFlags.Instance
    )!;

#endif
    
    #endregion

    #region Spans
    
    /// <summary>
    /// 将 source 中的字符复制到 destination 中.
    /// [Warning]: 不会进行安全检查.
    /// 如果 source 的长度高于 destination, 会由运行时抛出异常. 
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void CopyTo(ReadOnlySpan<char> source, ReadOnlySpan<char> destination) 
        => source.CopyTo(AsSpan(destination));
    
    /// <summary>
    /// 将 source 中的字符复制到 destination 中.
    /// [Warning]: 不会进行安全检查.
    /// 如果 source 的长度高于 destination, 会由运行时抛出异常. 
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void CopyTo(Span<char> source, ReadOnlySpan<char> destination) 
        => source.CopyTo(AsSpan(destination));
    
#if UseEmit


    /// <summary>
    /// 将 ReadOnlySpan 强制作为 Span 类型返回.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    internal static Span<char> AsSpan(ReadOnlySpan<char> input) => s_AsSpan(input);
    
    private static readonly Func<ReadOnlySpan<char>, Span<char>> s_AsSpan = CreateAsSpan();

    
    private static Func<ReadOnlySpan<char>, Span<char>> CreateAsSpan()
    {
        // 创建动态方法
        DynamicMethod dynamicMethod = new DynamicMethod(
            nameof(AsSpan), typeof(Span<char>), [typeof(ReadOnlySpan<char>)]
        );

        // 获取 IL 生成器
        ILGenerator il = dynamicMethod.GetILGenerator();
        il.DeclareLocal(typeof(Span<char>)); // 本地变量存储 Span<char>

        // 获取字段
        Type readOnlySpanType = typeof(ReadOnlySpan<char>);
        Type spanType = typeof(Span<char>);
        FieldInfo rosReferenceField = readOnlySpanType.GetField(
            "_reference", BindingFlags.NonPublic | BindingFlags.Instance
        )!;
        FieldInfo rosLengthField = readOnlySpanType.GetField(
            "_length", BindingFlags.NonPublic | BindingFlags.Instance
        )!;
        FieldInfo spanReferenceField = spanType.GetField(
            "_reference", BindingFlags.NonPublic | BindingFlags.Instance
        )!;
        FieldInfo spanLengthField = spanType.GetField(
            "_length", BindingFlags.NonPublic | BindingFlags.Instance
        )!;

        // 初始化 Span<char>
        il.Emit(OpCodes.Ldloca_S, 0); // 加载本地变量地址
        il.Emit(OpCodes.Initobj, spanType); // 初始化 Span<char>

        // 设置 _reference 字段
        il.Emit(OpCodes.Ldloca_S, 0); // 加载 Span<char> 地址
        il.Emit(OpCodes.Ldarg_0); // 加载 ReadOnlySpan<char>
        il.Emit(OpCodes.Ldfld, rosReferenceField); // 获取 _reference
        il.Emit(OpCodes.Stfld, spanReferenceField); // 设置 Span<char>._reference

        // 设置 _length 字段
        il.Emit(OpCodes.Ldloca_S, 0); // 加载 Span<char> 地址
        il.Emit(OpCodes.Ldarg_0); // 加载 ReadOnlySpan<char>
        il.Emit(OpCodes.Ldfld, rosLengthField); // 获取 _length
        il.Emit(OpCodes.Stfld, spanLengthField); // 设置 Span<char>._length

        // 返回 Span<char>
        il.Emit(OpCodes.Ldloc_0); // 加载 Span<char>
        il.Emit(OpCodes.Ret);

        // 创建委托
        return (Func<ReadOnlySpan<char>, Span<char>>)dynamicMethod.CreateDelegate(
            typeof(Func<ReadOnlySpan<char>, Span<char>>)
        );
    }

#else

    /// <summary>
    /// 将 ReadOnlySpan 强制作为 Span 类型返回.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    internal static Span<char> AsSpan(ReadOnlySpan<char> input)
        => MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(input), input.Length);

#endif
    
    #endregion
    
}