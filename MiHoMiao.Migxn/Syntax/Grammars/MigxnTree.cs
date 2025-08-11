namespace MiHoMiao.Migxn.Syntax.Grammars;

public abstract record MigxnTree(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position) 
    : MigxnNode(Text, Index, Position)
{
    internal abstract IEnumerable<MigxnNode> Children();
}