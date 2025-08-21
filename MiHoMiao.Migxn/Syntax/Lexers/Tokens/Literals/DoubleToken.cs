using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Data.Load;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

internal record DoubleToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : LiteralToken(Text, Index, Position)
{
    public override Type LiteralType(MigxnContext context) => typeof(double);
    
    public override IEnumerable<MigxnOpCode> AsOpCodes(MigxnContext context)
    {
        double value = double.Parse(Text.ToString());
        yield return new OpLdcR8(value);
    }
}