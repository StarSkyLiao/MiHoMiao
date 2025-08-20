using MiHoMiao.Migxn.Syntax.Intermediate;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Comments;

public record MultiLineComment(int Index, ReadOnlyMemory<char> Text, (int Line, int Column) Position) : IgnoredToken(Text, Index, Position)
{
    public override IEnumerable<MigxnOpCode> AsOpCodes() => [];
}