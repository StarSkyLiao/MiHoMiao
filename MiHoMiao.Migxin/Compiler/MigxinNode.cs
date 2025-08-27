namespace MiHoMiao.Migxin.Compiler;

public abstract record MigxinNode(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
{
    public abstract int NextColumn { get; }

    public sealed override string ToString() => ToStringImpl(0);

    internal virtual string ToStringImpl(int level) => $"{GetType().Name, -20} at {Position, -10}: >>\"{Text}\"<<\n";
    
}