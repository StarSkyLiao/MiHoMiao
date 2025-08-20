using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Intermediate.Data;
using MiHoMiao.Migxn.Syntax.Intermediate.Data.Load;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

public record DefaultToken(int Index, (int Line, int Column) Position) : LiteralToken("default".AsMemory(), Index, Position)
{
    public override IEnumerable<MigxnOpCode> AsOpCodes()
    {
        yield return new OpLdNull();
    }
}