namespace MiHoMiao.Migxin.Syntax.Lexical;

public abstract record MigxinToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : MigxinNode(Text, Index, Position)
{
    public override int NextColumn => Position.Column + Text.Length;

    public abstract MigxinToken? TryMatch(MigxinLexer migxinLexer);
}