namespace MiHoMiao.Migxn.Syntax;

public abstract record MigxnNode(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
{
    public sealed override string ToString() => ToStringImpl(0);
    
    internal virtual string ToStringImpl(int level) => $"{GetType().Name} at \t{Index} \t({Position.Line},{Position.Column}:\t>>  \"{Text}\"  <<)";
    
}