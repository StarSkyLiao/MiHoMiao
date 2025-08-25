using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("MiHoMiao.Program")]
namespace MiHoMiao.Migxin.Syntax;

public abstract record MigxinNode(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
{
    public abstract int NextColumn { get; }

    public sealed override string ToString() => ToStringImpl(0);

    internal virtual string ToStringImpl(int level) => $"{GetType().Name} at \t{Index} \t({Position.Line},{Position.Column}:\t >>\"{Text}\"<<)";
    
}