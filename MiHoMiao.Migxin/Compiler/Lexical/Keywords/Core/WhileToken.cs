namespace MiHoMiao.Migxin.Compiler.Lexical.Keywords.Core;

internal record WhileToken(int Index, (int Line, int Column) Position)
    : MigxinToken(Keyword.AsMemory(), Index, Position), IKeywordToken, ITokenMatcher
{
    public static string Keyword => "while";
    public static MigxinToken LoadToken(int start, (int Line, int Column) position) => new WhileToken(start, position);

    public static HashSet<char> StartChars => [.."w"];
    public static uint Priority => 5;
    public static MigxinToken? TryMatch(MigxinLexer migxinLexer) => IKeywordToken.TryMatch<WhileToken>(migxinLexer);
    
}