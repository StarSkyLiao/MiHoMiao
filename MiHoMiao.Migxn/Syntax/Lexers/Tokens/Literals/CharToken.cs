using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Data.Load;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

internal record CharToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : LiteralToken(Text, Index, Position)
{
    public override Type LiteralType(MigxnContext context) => typeof(char);
    
    public override IEnumerable<MigxnOpCode> AsOpCodes()
    {
        yield return new OpLdcI4(Text.Span[0]);
    }
}