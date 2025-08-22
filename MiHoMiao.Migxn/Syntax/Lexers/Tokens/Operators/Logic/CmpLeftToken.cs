using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Compare;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Logic;

internal record CmpLeftToken(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, ICompareBinary<CmpLeftToken>
{
    public static string UniqueName => ">";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new CmpLeftToken(index, position);

    static MigxnOpCode ICompareBinary<CmpLeftToken>.Operator { get; } = new OpCgt();  
    
    int IBinaryToken.Priority => 7;

    MigxnNode ILeaderOpToken.MigxnNode => this;
    
}