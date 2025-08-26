namespace MiHoMiao.Migxin.Syntax.Lexical.Keywords.Core;

internal record VarToken(int Index, (int Line, int Column) Position)
    : MigxinToken(Keyword.AsMemory(), Index, Position), IKeywordToken, ITokenMatcher
{
    public static string Keyword => "var";
    public static MigxinToken LoadToken(int start, (int Line, int Column) position) => new VarToken(start, position);

    public static HashSet<char> StartChars => [.."v"];
    public static uint Priority => 3;
    public static MigxinToken? TryMatch(MigxinLexer migxinLexer) => IKeywordToken.TryMatch<VarToken>(migxinLexer);
    
}