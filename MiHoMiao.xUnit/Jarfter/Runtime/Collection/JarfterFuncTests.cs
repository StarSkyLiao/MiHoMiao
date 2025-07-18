using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Runtime.Collection;

namespace MiHoMiao.xUnit.Jarfter.Runtime.Collection;

public class JarfterFuncTests
{
    [Fact]
    public void Test1_BasicFunc_SimpleCode()
    {
        JarfterFunc result = JarfterFunc.ParseInternal("{x => x + 1}");
        Assert.Equal("x => x + 1", result.FuncCode);
    }

    [Fact]
    public void Test2_EmptyFunc()
    {
        JarfterFunc result = JarfterFunc.ParseInternal("{}");
        Assert.Equal("", result.FuncCode);
    }

    [Fact]
    public void Test3_NestedBraces()
    {
        JarfterFunc result = JarfterFunc.ParseInternal("{{x => { return x + 1; }}}");
        Assert.Equal("{x => { return x + 1; }}", result.FuncCode);
    }
    
    [Fact]
    public void Test3_1_NestedBracesAndArray()
    {
        JarfterFunc result = JarfterFunc.ParseInternal("{{x => { [][return x + 1; }}}");
        Assert.Equal("{x => { [][return x + 1; }}", result.FuncCode);
    }
    
    [Fact]
    public void Test3_2_AtName()
    {
        JarfterFunc result = JarfterFunc.ParseInternal("{{@x => { [][return x + 1; }}}");
        Assert.Equal("{@x => { [][return x + 1; }}", result.FuncCode);
    }

    [Fact]
    public void Test4_WhitespaceBeforeAndAfter()
    {
        JarfterFunc result = JarfterFunc.ParseInternal("   {x => x * 2}   ");
        Assert.Equal("x => x * 2", result.FuncCode);
    }

    [Fact]
    public void Test5_InvalidInput_MissingOpeningBrace_ThrowsException()
    {
        Assert.Throws<UnBalancedArrayException>(() => JarfterFunc.ParseInternal("x => x + 1}"));
    }

    [Fact]
    public void Test6_InvalidInput_MissingClosingBrace_ThrowsException()
    {
        Assert.Throws<UnBalancedArrayException>(() => JarfterFunc.ParseInternal("{x => x + 1"));
    }

    [Fact]
    public void Test7_InvalidInput_UnbalancedBraces_ThrowsException()
    {
        Assert.Throws<UnBalancedArrayException>(() => JarfterFunc.ParseInternal("{{x => x + 1}"));
    }

    [Fact]
    public void Test8_InvalidContext_ThrowsInvalidCallingTreeException()
    {
        Assert.Throws<InvalidCallingTreeException>(() => JarfterFunc.Parse("{}", null));
    }
}