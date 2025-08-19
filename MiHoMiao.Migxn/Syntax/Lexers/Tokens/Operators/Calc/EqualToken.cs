using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Calc;

public record EqualToken(int Index, (int Line, int Column) Position)
    : AbstractOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, IBinaryToken
{
    public static string UniqueName => "=";

    public static AbstractOperator Create(int index, (int Line, int Column) position) => new EqualToken(index, position);

    int IBinaryToken.Priority => 16;

    MigxnNode ILeaderOpToken.MigxnNode => this;
    
}