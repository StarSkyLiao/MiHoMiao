using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Data.Load;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

internal record StringToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : LiteralToken(Text, Index, Position)
{
    public override Type LiteralType(MigxnContext context) => typeof(string);
    
    public override IEnumerable<MigxnOpCode> AsOpCodes()
    {
        yield return new OpLdcStr(Text.ToString());
    }
}