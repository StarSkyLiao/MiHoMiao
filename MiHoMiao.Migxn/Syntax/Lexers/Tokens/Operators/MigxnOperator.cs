using MiHoMiao.Migxn.Syntax.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

public record MigxnOperator(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : MigxnToken(Text, Index, Position)
{
    public override IEnumerable<MigxnOpCode> AsOpCodes() => throw new NotSupportedException();
}