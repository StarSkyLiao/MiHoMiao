using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Data.Load;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

internal record DefaultToken(int Index, (int Line, int Column) Position) : LiteralToken("default".AsMemory(), Index, Position)
{
    public override Type LiteralType(MigxnContext context) => typeof(object);
    
    public override IEnumerable<MigxnOpCode> AsOpCodes(MigxnContext context)
    {
        yield return new OpLdNull();
    }
}