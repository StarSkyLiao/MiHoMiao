using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Algorithm;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Calc;

internal record MulToken(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, ICalculationBinary<MulToken>
{
    public static string UniqueName => "*";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new MulToken(index, position);

    static MigxnOpCode ICalculationBinary<MulToken>.Operator { get; } = new OpMul();
    
    int IBinaryToken.Priority => 4;
    
    MigxnNode ILeaderOpToken.MigxnNode => this;

}