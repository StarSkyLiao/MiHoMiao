using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

internal record MigxnOperator(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : MigxnToken(Text, Index, Position)
{
    public override IEnumerable<MigxnOpCode> AsOpCodes(MigxnContext context) => throw new NotSupportedException();
}