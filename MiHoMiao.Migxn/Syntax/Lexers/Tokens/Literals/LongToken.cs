using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Intermediate.Data;
using MiHoMiao.Migxn.Syntax.Intermediate.Data.Load;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

public record LongToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : LiteralToken(Text, Index, Position)
{
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