using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

internal record OrToken(int Index, (int Line, int Column) Position)
    : AbstractKeyword(UniqueName.AsMemory(), Index, Position), IKeywordToken, IBinaryToken
{
    public static string UniqueName => "or";

    public static AbstractKeyword Create(int index, (int Line, int Column) position) => new OrToken(index, position);

    int IBinaryToken.Priority => 13;
    
    MigxnNode ILeaderOpToken.MigxnNode => this;

}