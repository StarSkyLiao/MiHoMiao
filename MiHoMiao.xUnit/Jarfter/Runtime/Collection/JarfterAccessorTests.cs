using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;

namespace MiHoMiao.xUnit.Jarfter.Runtime.Collection;

public class JarfterAccessorTests
{
    [Fact]
    public void ParseInternal_ValidIdentifier_LettersOnly()
    {
        Assert.Equal("abc", JarfterAccessor.ParseInternal(" @abc ").VarName);
    }

    [Fact]
    public void ParseInternal_ValidIdentifier_UnderscoreStart()
    {
        Assert.Equal("_abc", JarfterAccessor.ParseInternal(" @_abc ").VarName);
    }

    [Fact]
    public void ParseInternal_ValidIdentifier_UnderscoreAndNumbers()
    {
        Assert.Equal("_abc123", JarfterAccessor.ParseInternal(" @_abc123 ").VarName);
    }

    [Fact]
    public void ParseInternal_LeadingWhitespace_IgnoresWhitespace()
    {
        Assert.Equal("xyz", JarfterAccessor.ParseInternal("    @xyz ").VarName);
    }

    [Fact]
    public void ParseInternal_TrailingWhitespace_IgnoresWhitespace()
    {
        Assert.Equal("xyz", JarfterAccessor.ParseInternal(" @xyz    ").VarName);
    }

    
    [Fact]
    public void ParseInternal_MultiAt()
    {
        Assert.Equal("xy", JarfterAccessor.ParseInternal(" @xy@z    ").VarName);
    }

    [Fact]
    public void ParseInternal_MissingAtSymbol_ThrowsInvalidTypeException()
    {
        string input = " abc ";
        Assert.Throws<InvalidTypeException<JarfterAccessor>>(() => JarfterAccessor.ParseInternal(input));
    }

    [Fact]
    public void ParseInternal_PunctuationAfterAtSymbol_ThrowsInvalidTypeException()
    {
        string input = " @!abc ";
        Assert.Throws<InvalidTypeException<JarfterAccessor>>(() => JarfterAccessor.ParseInternal(input));
    }

    [Fact]
    public void ParseInternal_WhitespaceAfterAtSymbol_ThrowsInvalidTypeException()
    {
        string input = " @ abc ";
        Assert.Throws<InvalidTypeException<JarfterAccessor>>(() => JarfterAccessor.ParseInternal(input));
    }

    [Fact]
    public void ParseInternal_EmptyInput_ThrowsInvalidTypeException()
    {
        string input = "";
        Assert.Throws<InvalidTypeException<JarfterAccessor>>(() => JarfterAccessor.ParseInternal(input));
    }

    [Fact]
    public void ParseInternal_ValidInput_ReturnsCorrectContent()
    {
        JarfterContext context = new JarfterContext(null!);
        context.JarfterSymbolTable.DeclareVariable("testVar", new JarfterWord("2"));
        JarfterAccessor result = JarfterAccessor.Parse(" @testVar ", context);
        Assert.Equal("testVar", result.VarName);
        Assert.NotNull(result.Content);
    }
}