using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Logic;

internal record ShrToken(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, IBinaryToken
{
    public static string UniqueName => ">>";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new ShrToken(index, position);

    int IBinaryToken.Priority => 6;
    
    MigxnNode ILeaderOpToken.MigxnNode => this;

}