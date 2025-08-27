namespace MiHoMiao.Migxin.Compiler.Lexical;

internal interface ITokenMatcher
{
    public static abstract HashSet<char> StartChars { get; }
    public static abstract uint Priority { get; }
    public static abstract MigxinToken? TryMatch(MigxinLexer migxinLexer);
}