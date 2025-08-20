using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Intermediate.Algorithm;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Logic;

public record LogicAnd(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, IBinaryToken
{
    public static string UniqueName => "&";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new LogicAnd(index, position);

    IEnumerable<MigxnOpCode> IBinaryToken.BinaryOp(MigxnExpr left, MigxnExpr right)
    {
        return left.AsOpCodes().Concat(right.AsOpCodes()).Concat([new OpAnd()]);
    }
    
    int IBinaryToken.Priority => 9;
    
    MigxnNode ILeaderOpToken.MigxnNode => this;

}