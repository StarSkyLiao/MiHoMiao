using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Intermediate.Data;
using MiHoMiao.Migxn.Syntax.Intermediate.Data.Load;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

public record StringToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : LiteralToken(Text, Index, Position)
{
    public override IEnumerable<MigxnOpCode> AsOpCodes()
    {
        yield return new OpLdcStr(Text.ToString());
    }
}