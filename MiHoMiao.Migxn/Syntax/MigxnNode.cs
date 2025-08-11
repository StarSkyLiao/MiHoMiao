namespace MiHoMiao.Migxn.Syntax;

public abstract record MigxnNode(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
{
    public override string ToString() => $"{GetType()} at {Index} ({Position.Line},{Position.Column}:>>\"{Text}\"<<)";
}