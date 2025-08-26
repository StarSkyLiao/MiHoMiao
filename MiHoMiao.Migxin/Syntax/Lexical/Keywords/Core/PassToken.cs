namespace MiHoMiao.Migxin.Syntax.Lexical.Keywords.Core;

internal record PassToken(int Index, (int Line, int Column) Position)
    : MigxinToken(Keyword.AsMemory(), Index, Position), IKeywordToken, ITokenMatcher
{
    public static string Keyword => "pass";
    public static MigxinToken LoadToken(int start, (int Line, int Column) position) => new PassToken(start, position);

    public static HashSet<char> StartChars => [.."p"];
    public static uint Priority => 4;
    public static MigxinToken? TryMatch(MigxinLexer migxinLexer) => IKeywordToken.TryMatch<PassToken>(migxinLexer);
    
}