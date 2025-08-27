namespace MiHoMiao.Migxin.Compiler.Lexical.Operators.Calc;

internal record ShrToken(int Index, (int Line, int Column) Position)
    : MigxinToken(Keyword.AsMemory(), Index, Position), IKeywordToken, ITokenMatcher
{
    public static string Keyword => ">>";
    public static MigxinToken LoadToken(int start, (int Line, int Column) position) => new ShrToken(start, position);

    public static HashSet<char> StartChars => [..">"];
    public static uint Priority => 2;
    public static MigxinToken? TryMatch(MigxinLexer migxinLexer) => IKeywordToken.TryMatch<ShrToken>(migxinLexer);
    
}