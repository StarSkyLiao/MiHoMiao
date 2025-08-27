namespace MiHoMiao.Migxin.Compiler.Lexical.Keywords.Core;

internal record BreakToken(int Index, (int Line, int Column) Position)
    : MigxinToken(Keyword.AsMemory(), Index, Position), IKeywordToken, ITokenMatcher
{
    public static string Keyword => "break";
    public static MigxinToken LoadToken(int start, (int Line, int Column) position) => new BreakToken(start, position);

    public static HashSet<char> StartChars => [.."b"];
    public static uint Priority => 5;
    public static MigxinToken? TryMatch(MigxinLexer migxinLexer) => IKeywordToken.TryMatch<BreakToken>(migxinLexer);
    
}