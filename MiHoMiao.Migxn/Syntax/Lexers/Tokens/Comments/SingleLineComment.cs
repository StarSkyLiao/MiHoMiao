using MiHoMiao.Migxn.Syntax.Parser.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Comments;

internal record SingleLineComment(int Index, ReadOnlyMemory<char> Text, (int Line, int Column) Position) : IgnoredToken(Text, Index, Position)
{
    public override IEnumerable<MigxnOpCode> AsOpCodes() => [];
}