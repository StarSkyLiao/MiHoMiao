using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Compare;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Logic;

internal record CmpRightEqlToken(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, ICompareBinary<CmpRightEqlToken>
{
    public static string UniqueName => "<=";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new CmpRightEqlToken(index, position);

    static MigxnOpCode ICompareBinary<CmpRightEqlToken>.Operator { get; } = new OpCle();  
    
    int IBinaryToken.Priority => 7;

    MigxnNode ILeaderOpToken.MigxnNode => this;
    
}