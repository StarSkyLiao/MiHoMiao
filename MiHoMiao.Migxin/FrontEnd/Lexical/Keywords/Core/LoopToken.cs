namespace MiHoMiao.Migxin.FrontEnd.Lexical.Keywords.Core;

internal record LoopToken(int Index, (int Line, int Column) Position)
    : MigxinToken(Keyword.AsMemory(), Index, Position), IKeywordToken, ITokenMatcher
{
    public static string Keyword => "loop";
    public static MigxinToken LoadToken(int start, (int Line, int Column) position) => new LoopToken(start, position);

    public static HashSet<char> StartChars => [.."l"];
    public static uint Priority => 4;
    public static MigxinToken? TryMatch(MigxinLexer migxinLexer) => IKeywordToken.TryMatch<LoopToken>(migxinLexer);
    
}