using MiHoMiao.Migxn.Analysis.Tokenize;
using MiHoMiao.Migxn.Analysis.Tokenize.Core;

namespace MiHoMiao.xUnit.Migxn.Analysis.Tokenize;

public class TokenFactoryTest
{
    private const string RawText =
        """
        可空 item val vart a1 
        _ __ _1 _a1 1 哈
        @item @val @_
        """;
    
    [Fact]
    public void KeywordFactoryTest()
    {
        TokenizeContext context = new TokenizeContext(RawText);
        KeywordFactory factory = new KeywordFactory();
        List<string> result = [];
        Lexer(context, factory, result);
        Assert.Equal(["可空", "item", "val", "vart", "a1", "_", "__", "_1", "_a1", "哈", "item", "val", "_"], result);
    }
    
    [Fact]
    public void IdentifierFactoryTest()
    {
        TokenizeContext context = new TokenizeContext(RawText);
        IdentifierFactory factory = new IdentifierFactory();
        List<string> result = [];
        Lexer(context, factory, result);
        Assert.Equal(["可空", "item", "val", "vart", "a1", "_", "__", "_1", "_a1", "哈", "@item", "@val", "@_"], result);
    }
    
    #region 整数测试

    [Theory]
    // ====== 合法二进制 ======
    [InlineData("0b0",        "BinInteger.0b0")]
    [InlineData("0B1",        "BinInteger.0B1")]
    [InlineData("0b10_11L",   "BinInteger.0b10_11L")]
    [InlineData("0b_0",       "BinInteger.0b_0")]
    [InlineData("0b1___0L",   "BinInteger.0b1___0L")]
    [InlineData("0b01Lx",     "BinInteger.0b01L")]
    // ====== 合法十进制 ======
    [InlineData("0",          "DecInteger.0")]
    [InlineData("123",        "DecInteger.123")]
    [InlineData("1_000_000L", "DecInteger.1_000_000L")]
    [InlineData("9_",         "DecInteger.9")]
    [InlineData("0L",         "DecInteger.0L")]
    // ====== 合法十六进制 ======
    [InlineData("0x0",        "HexInteger.0x0")]
    [InlineData("0Xdead_BEFL","HexInteger.0Xdead_BEFL")]
    [InlineData("0x_0F_",     "HexInteger.0x_0F")]
    [InlineData("0x1L",       "HexInteger.0x1L")]
    // ====== 回退到十进制（以 0 开头但非 b/B/x/X） ======
    [InlineData("05",         "DecInteger.05")]
    [InlineData("0_7",        "DecInteger.0_7")]
    [InlineData("0123L",      "DecInteger.0123L")]
    // ====== 非法/边界 ======
    [InlineData("x",          "")]
    [InlineData("0b",         "")]
    [InlineData("0x",         "")]
    [InlineData("0b2",        "")]
    [InlineData("0xG",        "")]
    [InlineData("1__L",       "DecInteger.1")]
    [InlineData("0b_L",       "")]
    [InlineData("0x_L",       "")]
    [InlineData("1L_",        "DecInteger.1L")]
    [InlineData("123abc",     "DecInteger.123")]
    public void IntegerFactoryTest(string text, string expected)
    {
        TokenizeContext context = new TokenizeContext(text);
        IntegerFactory factory = new IntegerFactory();
        MigxnToken? migxnToken = factory.ParseToken(context);
        Assert.Equal(expected, migxnToken?.Display() ?? "");
    }

    #endregion 整数测试
    
    #region 实数测试

    [Theory]
    [InlineData("3.14", "3.14")]
    [InlineData("0.0", "0.0")]
    [InlineData("123.456", "123.456")]
    [InlineData("1.23e10", "1.23e10")]
    [InlineData("1.23E+10", "1.23E+10")]
    [InlineData("1.23e-10", "1.23e-10")]
    [InlineData("3.14f", "3.14f")]
    [InlineData("2.5F", "2.5F")]
    [InlineData("0.0d", "0.0d")]
    [InlineData("1.2m", "1.2m")]
    [InlineData("1.2M", "1.2M")]
    [InlineData("42","")] // 无效：没有小数点
    [InlineData("abc", "")] // 无效：非数字
    [InlineData("1.2.3", "1.2")] // 无效：多个小数点
    [InlineData(".5", "")] // 无效：缺少整数部分
    [InlineData("5.", "")] // 无效：缺少小数部分
    [InlineData("1e10", null)] // 无效：缺少小数点（除非你想支持整数，但正则要求小数点）
    [InlineData("1.2e", "1.2")] // 无效：指数部分不完整
    [InlineData("1.2e+", "1.2")] // 无效：指数部分不完整
    [InlineData("1.2e-", "1.2")] // 无效：指数部分不完整
    public void RealNumberFactoryTest(string text, string expected)
    {
        TokenizeContext context = new TokenizeContext(text);
        IntegerFactory factory = new IntegerFactory();
        MigxnToken? migxnToken = factory.ParseToken(context);
        Assert.Equal(expected, migxnToken?.Display() ?? "");
    }

    #endregion 实数测试
    
    [Fact]
    public void RealFactoryTest()
    {
        TokenizeContext context = new TokenizeContext("1.2 1.2f 1. .1 1e5f");
        RealNumberFactory factory = new RealNumberFactory();
        List<string> result = [];
        Lexer(context, factory, result);
        Assert.Equal(["1.2", "1.2f", "1", "1", "1e5f"], result);
    }
        
    private static void Lexer(TokenizeContext context, TokenFactory factory, List<string> result)
    {
        while (context.CurrentChar is not '\0')
        {
            context.TrimHead();
            MigxnToken? migxnToken = factory.ParseToken(context);
            if (migxnToken != null) result.Add(migxnToken.Text.ToString());
            else context.AcceptWord();
        }
    }
}