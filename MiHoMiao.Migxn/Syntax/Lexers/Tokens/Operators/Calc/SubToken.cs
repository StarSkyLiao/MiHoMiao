using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Prefix;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Algorithm;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Calc;

internal record SubToken(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, ICalculationBinary<SubToken>, IPrefixToken
{
    public static string UniqueName => "-";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new SubToken(index, position);
    
    public IEnumerable<MigxnOpCode> PrefixOp(MigxnExpr right, MigxnContext context) => right.AsOpCodes(context).Concat([new OpNeg()]);
    
    static MigxnOpCode ICalculationBinary<SubToken>.Operator { get; } = new OpSub();
    
    int IPrefixToken.Priority => 1;
    
    int IBinaryToken.Priority => 5;

    MigxnNode ILeaderOpToken.MigxnNode => this;
    
}