using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords.Logic;

internal record AndToken(int Index, (int Line, int Column) Position)
    : AbstractKeyword(UniqueName.AsMemory(), Index, Position), IKeywordToken, IBinaryToken
{
    public static string UniqueName => "and";

    public static AbstractKeyword Create(int index, (int Line, int Column) position) => new AndToken(index, position);

    public override IEnumerable<MigxnOpCode> AsOpCodes() => throw new NotImplementedException();
    
    int IBinaryToken.Priority => 12;
    
    MigxnNode ILeaderOpToken.MigxnNode => this;

}