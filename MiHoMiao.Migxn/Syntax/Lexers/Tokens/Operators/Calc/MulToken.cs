using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Algorithm;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Calc;

internal record MulToken(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, IBinaryToken
{
    public static string UniqueName => "*";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new MulToken(index, position);

    IEnumerable<MigxnOpCode> IBinaryToken.BinaryOp(MigxnExpr left, MigxnExpr right)
    {
        return left.AsOpCodes().Concat(right.AsOpCodes()).Concat([new OpMul()]);
    }
    
    int IBinaryToken.Priority => 4;
    
    MigxnNode ILeaderOpToken.MigxnNode => this;

}