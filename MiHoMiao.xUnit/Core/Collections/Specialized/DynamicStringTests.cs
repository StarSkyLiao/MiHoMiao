using MiHoMiao.Core.Collections.Specialized;

namespace MiHoMiao.xUnit.Core.Collections.Specialized;

public class DynamicStringTests
{
    #region 扩容

    [Fact]
    public void ExpandString_ExpandsCorrectly()
    {
        using DynamicString ds = new DynamicString(2);
        ds.Append("123456789");
        Assert.True(ds.RawStringLength >= 9);
        Assert.Equal("123456789", ds.Read());
    }

    #endregion

    #region 构造 & 释放

    [Fact]
    public void Ctor_Default_InitializesCorrectly()
    {
        using DynamicString ds = new DynamicString();
        Assert.Equal(1 << DynamicString.MinBitLengthOfStringLength, ds.RawStringLength);
        Assert.Equal(0, ds.CurrStringLength);
    }

    [Fact]
    public void Ctor_WithString_InitializesCorrectly()
    {
        const string Src = "hello";
        using DynamicString ds = new DynamicString(Src);
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
        using (DynamicString _ = new DynamicString("abc", s => captured = s))
        {
        }

        Assert.Equal("abc", captured);
    }

    #endregion

    #region Append

    [Fact]
    public void Append_Char_IncreasesLength()
    {
        using DynamicString ds = new DynamicString();
        ds.Append('a');
        Assert.Equal("a", ds.Read());
        Assert.Equal(1, ds.CurrStringLength);
    }

    [Fact]
    public void Append_String_IncreasesLength()
    {
        using DynamicString ds = new DynamicString();
        ds.Append("abc");
        Assert.Equal("abc", ds.Read());
        Assert.Equal(3, ds.CurrStringLength);
    }

    [Fact]
    public void Append_Object_CallsToString()
    {
        using DynamicString ds = new DynamicString();
        ds.Append(123);
        Assert.Equal("123", ds.Read());
    }

    [Fact]
    public void Append_Formattable_AvoidsAlloc()
    {
        using DynamicString ds = new DynamicString();
        ds.AppendFormattable(42);
        Assert.Equal("42", ds.Read());
    }

    [Fact]
    public void AppendLine_AddsNewline()
    {
        using DynamicString ds = new DynamicString();
        ds.AppendLine("hi");
        Assert.Equal("hi\n", ds.Read());
    }

    [Fact]
    public void AppendLine_Formattable_AddsNewline()
    {
        using DynamicString ds = new DynamicString();
        ds.AppendLineFormattable(3.14);
        Assert.True(ds.Read().StartsWith("3.14") && ds.Read().EndsWith("\n"));
    }

    [Fact]
    public void Append_DynamicString()
    {
        using DynamicString ds1 = new DynamicString("foo");
        using DynamicString ds2 = new DynamicString();
        ds2.Append(ds1);
        Assert.Equal("foo", ds2.Read());
    }

    #endregion

    #region Insert

    [Fact]
    public void Insert_Char_AtBeginning()
    {
        using DynamicString ds = new DynamicString("bcd");
        ds.Insert(0, 'a');
        Assert.Equal("abcd", ds.Read());
    }

    [Fact]
    public void Insert_String_InMiddle()
    {
        using DynamicString ds = new DynamicString("ace");
        ds.Insert(1, "bd");
        Assert.Equal("abdce", ds.Read());
    }

    [Fact]
    public void Insert_ReadOnlySpan_Empty_DoesNothing()
    {
        using DynamicString ds = new DynamicString("abc");
        ds.Insert(1, ReadOnlySpan<char>.Empty);
        Assert.Equal("abc", ds.Read());
    }

    [Fact]
    public void Insert_Object_CallsToString()
    {
        using DynamicString ds = new DynamicString("abc");
        ds.Insert(1, 123);
        Assert.Equal("a123bc", ds.Read());
    }

    [Fact]
    public void Insert_Formattable_UsesSpanPath()
    {
        using DynamicString ds = new DynamicString("abc");
        ds.InsertFormattable(1, 42);
        Assert.Equal("a42bc", ds.Read());
    }

    #endregion

    #region Clear & ToString

    [Fact]
    public void Clear_ResetsCurrentLength()
    {
        using DynamicString ds = new DynamicString("hello");
        ds.Clear();
        Assert.Equal(0, ds.CurrStringLength);
        Assert.Equal("", ds.Read());
    }

    [Fact]
    public void ToString_ReturnsCopy()
    {
        using DynamicString ds = new DynamicString("test");
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
        using DynamicString ds = new DynamicString("xyz");
        ReadOnlySpan<char> span = ds;
        Assert.Equal("xyz", span.ToString());
    }

    [Fact]
    public void ExplicitCast_ToString()
    {
        using DynamicString ds = new DynamicString("abc");
        string s = (string)ds;
        Assert.Equal("abc", s);
    }

    [Fact]
    public void ImplicitCast_FromString()
    {
        DynamicString ds = "hello";
        Assert.Equal("hello", ds.Read());
        ds.Dispose();
    }

    #endregion

    #region 边界 / 空值

    [Fact]
    public void Append_EmptyString_DoesNothing()
    {
        using DynamicString ds = new DynamicString("x");
        ds.Append("");
        Assert.Equal("x", ds.Read());
    }

    [Fact]
    public void Append_Formattable_Fallback_ToString()
    {
        // 构造一个 TryFormat 返回 false 的伪实现
        BadFormattable bad = new BadFormattable();
        using DynamicString ds = new DynamicString();
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