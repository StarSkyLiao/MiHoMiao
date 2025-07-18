using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Runtime.Collection;

namespace MiHoMiao.xUnit.Jarfter.Runtime.Collection;

public class SplitByCommaWithBracketsTests
{
    [Fact]
    public void OriginalTestCase_UnmatchedClosingBracket()
    {
        string input = "(a,b),c,[d,e],{f,g}),h,,i";
        ReadOnlySpan<char> span = input.AsSpan();
        string[] result = JarfterFunc.SplitByCommaWithBrackets(span, 0, out _).ToArray();
        Assert.Equal(new[] { "(a,b)", "c", "[d,e]", "{f,g}" }, result);
    }

    [Fact]
    public void SimpleCommaSeparated_NoBrackets()
    {
        string input = "a,b,c";
        ReadOnlySpan<char> span = input.AsSpan();
        string[] result = JarfterFunc.SplitByCommaWithBrackets(span, 0, out _).ToArray();
        Assert.Equal(new[] { "a", "b", "c" }, result);
    }

    [Fact]
    public void NestedBrackets_Valid()
    {
        string input = "(a,(b,c)),d,[e,{f,g}]";
        ReadOnlySpan<char> span = input.AsSpan();
        string[] result = JarfterFunc.SplitByCommaWithBrackets(span, 0, out _).ToArray();
        Assert.Equal(new[] { "(a,(b,c))", "d", "[e,{f,g}]" }, result);
    }

    [Fact]
    public void EmptyInput()
    {
        string input = "";
        ReadOnlySpan<char> span = input.AsSpan();
        string[] result = JarfterFunc.SplitByCommaWithBrackets(span, 0, out _).ToArray();
        Assert.Empty(result);
    }

    [Fact]
    public void WhitespaceOnlyInput()
    {
        string input = "   , ,  ";
        ReadOnlySpan<char> span = input.AsSpan();
        string[] result = JarfterFunc.SplitByCommaWithBrackets(span, 0, out _).ToArray();
        Assert.Empty(result);
    }

    [Fact]
    public void UnmatchedOpeningBracket()
    {
        string input = "(a,b,[c,d";
        Assert.Throws<UnBalancedArrayException>(
            () => JarfterFunc.SplitByCommaWithBrackets(input.AsSpan(), 0, out _)
        );
    }

    [Fact]
    public void UnmatchedClosingBracket_EarlyReturn()
    {
        string input = "a,b],c";
        ReadOnlySpan<char> span = input.AsSpan();
        string[] result = JarfterFunc.SplitByCommaWithBrackets(span, 0, out _).ToArray();
        Assert.Equal(new[] { "a", "b" }, result);
    }

    [Fact]
    public void MultipleCommas()
    {
        string input = "a,,b,  ,c";
        ReadOnlySpan<char> span = input.AsSpan();
        string[] result = JarfterFunc.SplitByCommaWithBrackets(span, 0, out _).ToArray();
        Assert.Equal(new[] { "a", "b", "c" }, result);
    }

    [Fact]
    public void DeeplyNestedBrackets()
    {
        string input = "{a,[b,(c,d)],e},f";
        ReadOnlySpan<char> span = input.AsSpan();
        string[] result = JarfterFunc.SplitByCommaWithBrackets(span, 0, out _).ToArray();
        Assert.Equal(new[] { "{a,[b,(c,d)],e}", "f" }, result);
    }

    [Fact]
    public void SingleSegment_WithBrackets()
    {
        string input = "(a,b)";
        ReadOnlySpan<char> span = input.AsSpan();
        string[] result = JarfterFunc.SplitByCommaWithBrackets(span, 0, out _).ToArray();
        Assert.Equal(new[] { "(a,b)" }, result);
    }

    [Fact]
    public void UnmatchedClosingBracket_Multiple()
    {
        string input = "{a,b)),c";
        ReadOnlySpan<char> span = input.AsSpan();
        string[] result = JarfterFunc.SplitByCommaWithBrackets(span, 0, out _).ToArray();
        Assert.Equal(new[] { "{a,b)" }, result);
    }

    [Fact]
    public void MixedBrackets_WithWhitespace()
    {
        string input = " (a,b) , [c,d] , {e,f} ";
        ReadOnlySpan<char> span = input.AsSpan();
        string[] result = JarfterFunc.SplitByCommaWithBrackets(span, 0, out _).ToArray();
        Assert.Equal(new[] { "(a,b)", "[c,d]", "{e,f}" }, result);
    }

    [Fact]
    public void InvalidDepth_NegativeEarly()
    {
        string input = "a,),b";
        ReadOnlySpan<char> span = input.AsSpan();
        string[] result = JarfterFunc.SplitByCommaWithBrackets(span, 0, out _).ToArray();
        Assert.Equal(new[] { "a" }, result);
    }
}