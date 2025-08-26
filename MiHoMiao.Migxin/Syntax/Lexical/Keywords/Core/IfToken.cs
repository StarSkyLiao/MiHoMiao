namespace MiHoMiao.Migxin.Syntax.Lexical.Keywords.Core;

internal record IfToken(int Index, (int Line, int Column) Position)
    : MigxinToken(Keyword.AsMemory(), Index, Position), IKeywordToken, ITokenMatcher
{
    public static string Keyword => "if";
    public static MigxinToken LoadToken(int start, (int Line, int Column) position) => new IfToken(start, position);

    public static HashSet<char> StartChars => [.."i"];
    public static uint Priority => 2;
    public static MigxinToken? TryMatch(MigxinLexer migxinLexer) => IKeywordToken.TryMatch<IfToken>(migxinLexer);
    
}