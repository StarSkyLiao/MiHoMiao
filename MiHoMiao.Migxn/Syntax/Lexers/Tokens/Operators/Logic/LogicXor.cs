using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Intermediate.Algorithm;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Logic;

public record LogicXor(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, IBinaryToken
{
    public static string UniqueName => "^";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new LogicXor(index, position);

    IEnumerable<MigxnOpCode> IBinaryToken.BinaryOp(MigxnExpr left, MigxnExpr right)
    {
        return left.AsOpCodes().Concat(right.AsOpCodes()).Concat([new OpXor()]);
    }
    
    int IBinaryToken.Priority => 10;
    
    MigxnNode ILeaderOpToken.MigxnNode => this;

}