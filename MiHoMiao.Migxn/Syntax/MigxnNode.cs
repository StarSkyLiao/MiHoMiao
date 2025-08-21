using System.Runtime.CompilerServices;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
[assembly:InternalsVisibleTo("MiHoMiao.Program")]
namespace MiHoMiao.Migxn.Syntax;

internal abstract record MigxnNode(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
{
    public abstract int NextColumn { get; }

    public sealed override string ToString() => ToStringImpl(0);

    public abstract IEnumerable<MigxnOpCode> AsOpCodes(MigxnContext context);

    internal virtual string ToStringImpl(int level) => $"{GetType().Name} at \t{Index} \t({Position.Line},{Position.Column}:\t>>  \"{Text}\"  <<)";
    
}