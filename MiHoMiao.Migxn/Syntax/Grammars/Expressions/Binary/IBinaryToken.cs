using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;

internal interface IBinaryToken : ILeaderOpToken
{
    /// <summary>
    /// 优先级越小, 实际优先级越高
    /// </summary>
    int Priority { get; }

    IEnumerable<MigxnOpCode> BinaryOp(MigxnExpr left, MigxnExpr right, MigxnContext context) => [];
    
    Type BinaryType(MigxnExpr left, MigxnExpr right, MigxnContext context);
}