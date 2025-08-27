namespace MiHoMiao.Migxin.FrontEnd.Lexical.Keywords.Core;

internal record ValToken(int Index, (int Line, int Column) Position)
    : MigxinToken(Keyword.AsMemory(), Index, Position), IKeywordToken, ITokenMatcher
{
    public static string Keyword => "val";
    public static MigxinToken LoadToken(int start, (int Line, int Column) position) => new ValToken(start, position);

    public static HashSet<char> StartChars => [.."v"];
    public static uint Priority => 3;
    public static MigxinToken? TryMatch(MigxinLexer migxinLexer) => IKeywordToken.TryMatch<ValToken>(migxinLexer);
    
}