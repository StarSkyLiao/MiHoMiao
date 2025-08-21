namespace MiHoMiao.Migxn.Syntax.Lexers;

internal abstract record MigxnToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : MigxnNode(Text, Index, Position)
{
    public override int NextColumn => Position.Column + Text.Length;
}