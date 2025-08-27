namespace MiHoMiao.Migxin.Compiler.Lexical.Keywords.Core;

internal record LetToken(int Index, (int Line, int Column) Position)
    : MigxinToken(Keyword.AsMemory(), Index, Position), IKeywordToken, ITokenMatcher
{
    public static string Keyword => "let";
    public static MigxinToken LoadToken(int start, (int Line, int Column) position) => new LetToken(start, position);

    public static HashSet<char> StartChars => [.."l"];
    public static uint Priority => 3;
    public static MigxinToken? TryMatch(MigxinLexer migxinLexer) => IKeywordToken.TryMatch<LetToken>(migxinLexer);
    
}