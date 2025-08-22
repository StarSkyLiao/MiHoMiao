using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Algorithm;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Logic;

internal record ShrToken(int Index, (int Line, int Column) Position)
    : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, IIntShiftBinary<ShrToken>
{
    public static string UniqueName => ">>";

    public static MigxnOperator Create(int index, (int Line, int Column) position) => new ShrToken(index, position);

    static MigxnOpCode IIntShiftBinary<ShrToken>.Operator { get; } = new OpShr();
    
    int IBinaryToken.Priority => 6;
    
    MigxnNode ILeaderOpToken.MigxnNode => this;

}