using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Compare;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Logic;

internal record CmpEqlToken(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, ICompareBinary<CmpEqlToken>
{
    public static string UniqueName => "==";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new CmpEqlToken(index, position);

    static MigxnOpCode ICompareBinary<CmpEqlToken>.Operator { get; } = new OpCeq();  
    
    int IBinaryToken.Priority => 8;

    MigxnNode ILeaderOpToken.MigxnNode => this;
    
}