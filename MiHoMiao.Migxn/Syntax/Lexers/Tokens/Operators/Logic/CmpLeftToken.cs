using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Logic;

public record CmpLeftToken(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, IBinaryToken
{
    public static string UniqueName => ">";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new CmpLeftToken(index, position);

    int IBinaryToken.Priority => 7;

    MigxnNode ILeaderOpToken.MigxnNode => this;
    
}