using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

internal record ElseToken(int Index, (int Line, int Column) Position)
    : AbstractKeyword(UniqueName.AsMemory(), Index, Position), IKeywordToken
{
    public static string UniqueName => "else";

    public static AbstractKeyword Create(int index, (int Line, int Column) position) => new ElseToken(index, position);

    public override IEnumerable<MigxnOpCode> AsOpCodes(MigxnContext context) => throw new NotSupportedException();
    
}