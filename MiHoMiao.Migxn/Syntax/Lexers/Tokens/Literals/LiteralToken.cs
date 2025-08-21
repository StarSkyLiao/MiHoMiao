using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

internal abstract record LiteralToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : MigxnToken(Text, Index, Position)
{
    
    public abstract Type LiteralType(MigxnContext context);
    
    public abstract override IEnumerable<MigxnOpCode> AsOpCodes();
    
}