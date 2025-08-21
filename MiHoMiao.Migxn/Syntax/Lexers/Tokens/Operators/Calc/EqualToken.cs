using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Data.Store;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Calc;

internal record EqualToken(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, IBinaryToken
{
    public static string UniqueName => "=";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new EqualToken(index, position);

    IEnumerable<MigxnOpCode> IBinaryToken.BinaryOp(MigxnExpr left, MigxnExpr right)
    {
        if (left is not TokenExpr { Token: SymbolToken symbol }) throw new NotSupportedException();
        return right.AsOpCodes().Concat([new OpStVar(symbol.Text)]);
    }
    
    int IBinaryToken.Priority => 16;

    MigxnNode ILeaderOpToken.MigxnNode => this;
    
}