using MiHoMiao.Jarfter.Runtime.Collection;

namespace MiHoMiao.xUnit.Jarfter.Runtime.Collection;

public class JarfterWordTests
{
    [Fact]
    public void Test1_ValidIdentifier_LettersOnly() => 
        Assert.Equal("abc", JarfterWord.ParseInternal(" abc ").Content);

    [Fact]
    public void Test2_ValidIdentifier_UnderscoreStart() => 
        Assert.Equal("_abc", JarfterWord.ParseInternal(" _abc ").Content);

    [Fact]
    public void Test3_ValidIdentifier_UnderscoreAndNumbers() => 
        Assert.Equal("_abc123", JarfterWord.ParseInternal(" _abc123 ").Content);

    [Fact]
    public void Test4_ValidIdentifier_ChineseCharacters() => 
        Assert.Equal("变量", JarfterWord.ParseInternal(" 变量 ").Content);

    [Fact]
    public void Test5_ValidIdentifier_ChineseCharactersAndNumbers() => 
        Assert.Equal("变量123", JarfterWord.ParseInternal(" 变量123 ").Content);

    [Fact]
    public void Test6_InvalidIdentifier_EmptyString_ThrowsParseException() => 
        Assert.Equal("", JarfterWord.ParseInternal("    ").Content);

    [Fact]
    public void Test7_InvalidIdentifier_NumbersOnly_ThrowsParseException() => 
        Assert.Equal("123", JarfterWord.ParseInternal("  123  ").Content);

    [Fact]
    public void Test8_InvalidIdentifier_NumberStartWithBracket_ThrowsParseException() => 
        Assert.Equal("1", JarfterWord.ParseInternal(" 1[23abc ").Content);

    [Fact]
    public void Test9_InvalidIdentifier_NumberWithClosingBrace_ThrowsParseException() => 
        Assert.Equal("123", JarfterWord.ParseInternal("123}abc").Content);

    [Fact]
    public void Test10_InvalidIdentifier_NumberWithBraces_ThrowsParseException() => 
        Assert.Equal("123", JarfterWord.ParseInternal("123{}abc").Content);

    [Fact]
    public void Test11_InvalidIdentifier_BracketedContent_ThrowsParseException() => 
        Assert.Equal("", JarfterWord.ParseInternal("[123abc]").Content);

    [Fact]
    public void Test12_InvalidIdentifier_SpecialCharacterStart_ThrowsParseException() => 
        Assert.Equal("", JarfterWord.ParseInternal("@变量123").Content);
}