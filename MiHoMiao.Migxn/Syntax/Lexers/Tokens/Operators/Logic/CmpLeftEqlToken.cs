using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Compare;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Logic;

internal record CmpLeftEqlToken(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, ICompareBinary<CmpLeftEqlToken>
{
    public static string UniqueName => ">=";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new CmpLeftEqlToken(index, position);

    static MigxnOpCode ICompareBinary<CmpLeftEqlToken>.Operator { get; } = new OpCge();  
    
    int IBinaryToken.Priority => 7;

    MigxnNode ILeaderOpToken.MigxnNode => this;
    
}