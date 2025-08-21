using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions.Prefix;

internal interface IPrefixToken : ILeaderOpToken
{
    /// <summary>
    /// 优先级越小, 实际优先级越高
    /// </summary>
    int Priority { get; }

    IEnumerable<MigxnOpCode> PrefixOp(MigxnExpr right) => [];
}