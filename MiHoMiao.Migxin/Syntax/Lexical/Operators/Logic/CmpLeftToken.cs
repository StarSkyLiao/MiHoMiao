namespace MiHoMiao.Migxin.Syntax.Lexical.Operators.Logic;

internal record CmpLeftToken(int Index, (int Line, int Column) Position)
    : MigxinToken(Keyword.AsMemory(), Index, Position), IKeywordToken, ITokenMatcher
{
    public static string Keyword => ">";
    public static MigxinToken LoadToken(int start, (int Line, int Column) position) => new CmpLeftToken(start, position);

    public static HashSet<char> StartChars => [..">"];
    public static uint Priority => 1;
    public static MigxinToken? TryMatch(MigxinLexer migxinLexer) => IKeywordToken.TryMatch<CmpLeftToken>(migxinLexer);
    
}