using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Lexers;

internal record BadToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : LiteralToken(Text, Index, Position)
{
    public override Type LiteralType(MigxnContext context) => typeof(void);

    public override IEnumerable<MigxnOpCode> AsOpCodes() => throw new NotSupportedException();

}