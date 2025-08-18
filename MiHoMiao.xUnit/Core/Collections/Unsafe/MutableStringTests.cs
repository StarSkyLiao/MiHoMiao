using MiHoMiao.Core.Collections.Unsafe;

namespace MiHoMiao.xUnit.Core.Collections.Unsafe;

public class MutableStringTests
{
    #region 扩容

    [Fact]
    public void ExpandString_ExpandsCorrectly()
    {
        using MutableString ds = new MutableString(2);
        ds.Append("123456789");
        Assert.True(ds.RawStringLength >= 9);
        Assert.Equal("123456789", ds.Read());
    }

    #endregion

    #region 构造 & 释放

    [Fact]
    public void Ctor_Default_InitializesCorrectly()
    {
        using MutableString ds = new MutableString();
        Assert.Equal(1 << MutableString.MinBitLengthOfStringLength, ds.RawStringLength);
        Assert.Equal(0, ds.CurrStringLength);
    }

    [Fact]
    public void Ctor_WithString_InitializesCorrectly()
    {
        const string Src = "hello";
        using MutableString ds = new MutableString(Src);
        int expect = Src.Length;
        int actual = ds.RawStringLength;
        Assert.Equal(expect, actual);
        Assert.Equal(expect, actual);
        Assert.Equal(Src, ds.Read());
    }

    [Fact]
    public void Dispose_CallsReleaseAction()
    {
        string? captured = null;
        using (MutableString _ = new MutableString("abc", s => captured = s))
        {
        }

        Assert.Equal("abc", captured);
    }

    #endregion

    #region Append

    [Fact]
    public void Append_Char_IncreasesLength()
    {
        using MutableString ds = new MutableString();
        ds.Append('a');
        Assert.Equal("a", ds.Read());
        Assert.Equal(1, ds.CurrStringLength);
    }

    [Fact]
    public void Append_String_IncreasesLength()
    {
        using MutableString ds = new MutableString();
        ds.Append("abc");
        Assert.Equal("abc", ds.Read());
        Assert.Equal(3, ds.CurrStringLength);
    }

    [Fact]
    public void Append_Object_CallsToString()
    {
        using MutableString ds = new MutableString();
        ds.Append(123);
        Assert.Equal("123", ds.Read());
    }

    [Fact]
    public void Append_Formattable_AvoidsAlloc()
    {
        using MutableString ds = new MutableString();
        ds.AppendFormattable(42);
        Assert.Equal("42", ds.Read());
    }

    [Fact]
    public void AppendLine_AddsNewline()
    {
        using MutableString ds = new MutableString();
        ds.AppendLine("hi");
        Assert.Equal("hi\n", ds.Read());
    }

    [Fact]
    public void AppendLine_Formattable_AddsNewline()
    {
        using MutableString ds = new MutableString();
        ds.AppendLineFormattable(3.14);
        Assert.True(ds.Read().StartsWith("3.14") && ds.Read().EndsWith("\n"));
    }

    [Fact]
    public void Append_DynamicString()
    {
        using MutableString ds1 = new MutableString("foo");
        using MutableString ds2 = new MutableString();
        ds2.Append(ds1);
        Assert.Equal("foo", ds2.Read());
    }

    #endregion

    #region Insert

    [Fact]
    public void Insert_Char_AtBeginning()
    {
        using MutableString ds = new MutableString("bcd");
        ds.Insert(0, 'a');
        Assert.Equal("abcd", ds.Read());
    }

    [Fact]
    public void Insert_String_InMiddle()
    {
        using MutableString ds = new MutableString("ace");
        ds.Insert(1, "bd");
        Assert.Equal("abdce", ds.Read());
    }

    [Fact]
    public void Insert_ReadOnlySpan_Empty_DoesNothing()
    {
        using MutableString ds = new MutableString("abc");
        ds.Insert(1, ReadOnlySpan<char>.Empty);
        Assert.Equal("abc", ds.Read());
    }

    [Fact]
    public void Insert_Object_CallsToString()
    {
        using MutableString ds = new MutableString("abc");
        ds.Insert(1, 123);
        Assert.Equal("a123bc", ds.Read());
    }

    [Fact]
    public void Insert_Formattable_UsesSpanPath()
    {
        using MutableString ds = new MutableString("abc");
        ds.InsertFormattable(1, 42);
        Assert.Equal("a42bc", ds.Read());
    }

    #endregion

    #region Clear & ToString

    [Fact]
    public void Clear_ResetsCurrentLength()
    {
        using MutableString ds = new MutableString("hello");
        ds.Clear();
        Assert.Equal(0, ds.CurrStringLength);
        Assert.Equal("", ds.Read());
    }

    [Fact]
    public void ToString_ReturnsCopy()
    {
        using MutableString ds = new MutableString("test");
        string copy = ds.ToString();
        ds.Append('!');
        Assert.Equal("test", copy);
        Assert.Equal("test!", ds.Read());
    }

    #endregion

    #region Operators

    [Fact]
    public void ImplicitCast_ToSpan()
    {
        using MutableString ds = new MutableString("xyz");
        ReadOnlySpan<char> span = ds;
        Assert.Equal("xyz", span.ToString());
    }

    [Fact]
    public void ExplicitCast_ToString()
    {
        using MutableString ds = new MutableString("abc");
        string s = (string)ds;
        Assert.Equal("abc", s);
    }

    [Fact]
    public void ImplicitCast_FromString()
    {
        MutableString ds = "hello";
        Assert.Equal("hello", ds.Read());
        ds.Dispose();
    }

    #endregion

    #region 边界 / 空值

    [Fact]
    public void Append_EmptyString_DoesNothing()
    {
        using MutableString ds = new MutableString("x");
        ds.Append("");
        Assert.Equal("x", ds.Read());
    }

    [Fact]
    public void Append_Formattable_Fallback_ToString()
    {
        // 构造一个 TryFormat 返回 false 的伪实现
        BadFormattable bad = new BadFormattable();
        using MutableString ds = new MutableString();
        ds.AppendFormattable(bad);
        Assert.Equal("Bad", ds.Read());
    }

    private sealed class BadFormattable : ISpanFormattable
    {
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format,
            IFormatProvider? provider)
        {
            charsWritten = 0;
            return false;
        }

        public string ToString(string? format, IFormatProvider? formatProvider) => ToString();
        public override string ToString() => "Bad";
    }

    #endregion
}