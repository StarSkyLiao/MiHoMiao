using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Prefix;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

internal record AddToken(int Index, (int Line, int Column) Position)
    : AbstractOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, IBinaryToken, IPrefixToken
{
    public static string UniqueName => "+";

    public static AbstractOperator Create(int index, (int Line, int Column) position) => new AddToken(index, position);

    int IPrefixToken.Priority => 1;
    
    int IBinaryToken.Priority => 5;

    MigxnNode ILeaderOpToken.MigxnNode => this;
    
}