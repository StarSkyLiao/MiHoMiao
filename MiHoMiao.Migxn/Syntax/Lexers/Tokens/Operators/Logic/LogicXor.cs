using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Algorithm;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Logic;

internal record LogicXor(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, IIntLogicBinary<LogicXor>
{
    public static string UniqueName => "^";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new LogicXor(index, position);

    static MigxnOpCode IIntLogicBinary<LogicXor>.Operator { get; } = new OpOr();
    
    int IBinaryToken.Priority => 10;
    
    MigxnNode ILeaderOpToken.MigxnNode => this;

}