using MiHoMiao.Core.Collections.Unsafe;

namespace MiHoMiao.xUnit.Core.Collections.Unsafe;

public class InterpolatedStringTests
{

    #region Append

    [Fact]
    public void Append_Char_IncreasesLength()
    {
        using InterpolatedString ds = new InterpolatedString();
        ds.Append('a');
        Assert.Equal("a", ds.ToString());
        Assert.Equal(1, ds.Length);
    }

    [Fact]
    public void Append_String_IncreasesLength()
    {
        using InterpolatedString ds = new InterpolatedString();
        ds.Append("abc");
        Assert.Equal("abc", ds.ToString());
        Assert.Equal(3, ds.Length);
    }

    [Fact]
    public void Append_Object_CallsToString()
    {
        using InterpolatedString ds = new InterpolatedString();
        ds.Append(123);
        Assert.Equal("123", ds.ToString());
    }

    [Fact]
    public void Append_Formattable_AvoidsAlloc()
    {
        using InterpolatedString ds = new InterpolatedString();
        ds.AppendFormatted(42);
        Assert.Equal("42", ds.ToString());
    }

    #endregion

    #region Insert

    [Fact]
    public void Insert_Char_AtBeginning()
    {
        using InterpolatedString ds = new InterpolatedString("bcd");
        ds.Insert(0, 'a');
        Assert.Equal("abcd", ds.ToString());
    }

    [Fact]
    public void Insert_String_InMiddle()
    {
        using InterpolatedString ds = new InterpolatedString("ace");
        ds.Insert(1, "bd");
        Assert.Equal("abdce", ds.ToString());
    }

    [Fact]
    public void Insert_ReadOnlySpan_Empty_DoesNothing()
    {
        using InterpolatedString ds = new InterpolatedString("abc");
        ds.Insert(1, ReadOnlySpan<char>.Empty);
        Assert.Equal("abc", ds.ToString());
    }

    [Fact]
    public void Insert_Object_CallsToString()
    {
        using InterpolatedString ds = new InterpolatedString("abc");
        ds.Insert(1, 123);
        Assert.Equal("a123bc", ds.ToString());
    }

    [Fact]
    public void Insert_Formattable_UsesSpanPath()
    {
        using InterpolatedString ds = new InterpolatedString("abc");
        ds.InsertFormattable(1, 42);
        Assert.Equal("a42bc", ds.ToString());
    }

    #endregion

    #region Clear & ToString

    [Fact]
    public void Clear_ResetsCurrentLength()
    {
        using InterpolatedString ds = new InterpolatedString();
        ds.Clear();
        Assert.Equal(0, ds.Length);
        Assert.Equal("", ds.ToString());
    }

    [Fact]
    public void ToString_ReturnsCopy()
    {
        using InterpolatedString ds = new InterpolatedString("test");
        string copy = ds.ToString();
        ds.Append('!');
        Assert.Equal("test", copy);
        Assert.Equal("test!", ds.ToString());
    }

    #endregion

    #region 边界 / 空值

    [Fact]
    public void Append_EmptyString_DoesNothing()
    {
        using InterpolatedString ds = new InterpolatedString("x");
        ds.Append("");
        Assert.Equal("x", ds.ToString());
    }

    #endregion
}