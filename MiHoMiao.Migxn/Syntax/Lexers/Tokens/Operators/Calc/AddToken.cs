using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Prefix;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Algorithm;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Calc;

internal record AddToken(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, IBinaryToken, IPrefixToken
{
    public static string UniqueName => "+";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new AddToken(index, position);

    public IEnumerable<MigxnOpCode> PrefixOp(MigxnExpr right) => right.AsOpCodes();
    
    IEnumerable<MigxnOpCode> IBinaryToken.BinaryOp(MigxnExpr left, MigxnExpr right)
    {
        return left.AsOpCodes().Concat(right.AsOpCodes()).Concat([new OpAdd()]);
    }

    int IPrefixToken.Priority => 1;

    int IBinaryToken.Priority => 5;

    MigxnNode ILeaderOpToken.MigxnNode => this;
    
}