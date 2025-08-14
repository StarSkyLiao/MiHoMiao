using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

internal record AndToken(int Index, (int Line, int Column) Position)
    : AbstractKeyword(UniqueName.AsMemory(), Index, Position), IKeywordToken, IBinaryToken
{
    public static string UniqueName => "and";

    public static AbstractKeyword Create(int index, (int Line, int Column) position) => new AndToken(index, position);

    int IBinaryToken.Priority => 12;
    
    MigxnNode IBinaryToken.MigxnNode => this;

}