using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Prefix;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Suffix;
using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Intermediate.Algorithm;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Calc;

internal record DecreaseToken(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, ISuffixToken, IPrefixToken
{
    public static string UniqueName => "--";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new DecreaseToken(index, position);
    
    int IPrefixToken.Priority => 1;

    int ISuffixToken.Priority => 0;
    
    MigxnNode ILeaderOpToken.MigxnNode => this;
    
}