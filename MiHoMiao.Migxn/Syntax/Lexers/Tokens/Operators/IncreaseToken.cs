using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Prefix;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Suffix;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

internal record IncreaseToken(int Index, (int Line, int Column) Position)
    : AbstractOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, ISuffixToken, IPrefixToken
{
    public static string UniqueName => "++";

    public static AbstractOperator Create(int index, (int Line, int Column) position) => new IncreaseToken(index, position);
    
    MigxnNode ILeaderOpToken.MigxnNode => this;

}