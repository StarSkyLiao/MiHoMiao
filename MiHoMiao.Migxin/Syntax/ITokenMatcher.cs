using MiHoMiao.Migxin.Syntax.Lexical;

namespace MiHoMiao.Migxin.Syntax;

internal interface ITokenMatcher
{
    public static abstract MigxinToken? TryMatch(MigxinLexer migxinLexer);
}