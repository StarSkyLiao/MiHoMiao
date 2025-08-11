using System.Text;

namespace MiHoMiao.Migxn.Syntax;

public abstract record MigxnNode(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
{
    public sealed override string ToString() =>
        $"{GetType().Name} at \t{Index} \t({Position.Line},{Position.Column}:\t>>    \"{Text}\"    <<)";
    
}