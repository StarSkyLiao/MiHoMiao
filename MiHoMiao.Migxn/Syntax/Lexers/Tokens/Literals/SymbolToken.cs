using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Intermediate.Data.Load;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

public record SymbolToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : LiteralToken(Text, Index, Position)
{
    public override IEnumerable<MigxnOpCode> AsOpCodes() => [new OpLdVar(Text)];
}