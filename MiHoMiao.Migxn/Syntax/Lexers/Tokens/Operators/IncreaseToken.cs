using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Prefix;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Suffix;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

internal record IncreaseToken(int Index, (int Line, int Column) Position)
    : AbstractOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, IPrefixToken, ISuffixToken
{
    public static string UniqueName => "++";

    public static AbstractOperator Create(int index, (int Line, int Column) position) => new IncreaseToken(index, position);

    int IPrefixToken.Priority => 1;

    int ISuffixToken.Priority => 0;

    MigxnNode ILeaderOpToken.MigxnNode => this;

}