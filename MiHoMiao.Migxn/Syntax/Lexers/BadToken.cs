using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

namespace MiHoMiao.Migxn.Syntax.Lexers;

public record BadToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : LiteralToken(Text, Index, Position)
{
    public override IEnumerable<MigxnOpCode> AsOpCodes() => throw new NotSupportedException();
}