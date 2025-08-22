using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Algorithm;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Logic;

internal record LogicAnd(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, IIntLogicBinary<LogicAnd>
{
    public static string UniqueName => "&";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new LogicAnd(index, position);

    static MigxnOpCode IIntLogicBinary<LogicAnd>.Operator { get; } = new OpAnd();
    
    int IBinaryToken.Priority => 9;
    
    MigxnNode ILeaderOpToken.MigxnNode => this;

}