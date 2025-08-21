using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Data.Load;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

internal record SymbolToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : LiteralToken(Text, Index, Position)
{
    public override Type LiteralType(MigxnContext context)
    {
        try
        {
            return context.Variables.LoadVariable(Text).Type;
        }
        catch (Exception ex)
        {
            context.MigxnParser.Exceptions.Add(ex);
            return typeof(object);
        }
    }

    public override IEnumerable<MigxnOpCode> AsOpCodes() => [new OpLdVar(Text)];
}