using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Data.Load;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

internal record LongToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : LiteralToken(Text, Index, Position)
{
    public override Type LiteralType(MigxnContext context) => typeof(long);
    
    public override IEnumerable<MigxnOpCode> AsOpCodes()
    {
        long value = long.Parse(Text.ToString());
        yield return value switch
        {
            >= sbyte.MinValue and <= sbyte.MaxValue => new OpLdcI4S((sbyte)value),
            >= int.MinValue and <= int.MaxValue => new OpLdcI4((int)value),
            _ => new OpLdcI8(value),
        };
    }
}