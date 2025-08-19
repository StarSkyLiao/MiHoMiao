using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Logic;

public record LogicAnd(int Index, (int Line, int Column) Position)
    : AbstractOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, IBinaryToken
{
    public static string UniqueName => "&";

    public static AbstractOperator Create(int index, (int Line, int Column) position) => new LogicAnd(index, position);

    int IBinaryToken.Priority => 9;
    
    MigxnNode ILeaderOpToken.MigxnNode => this;

}