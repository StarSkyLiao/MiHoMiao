using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Algorithm;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Logic;

internal record LogicOr(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, IIntLogicBinary<LogicOr>
{
    public static string UniqueName => "|";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new LogicOr(index, position);

    static MigxnOpCode IIntLogicBinary<LogicOr>.Operator { get; } = new OpOr();
    
    int IBinaryToken.Priority => 11;
    
    MigxnNode ILeaderOpToken.MigxnNode => this;

}