using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions.Suffix;

internal interface ISuffixToken : ILeaderOpToken
{
    /// <summary>
    /// 优先级越小, 实际优先级越高
    /// </summary>
    int Priority { get; }
}