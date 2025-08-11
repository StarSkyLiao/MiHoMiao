namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions;

public abstract record MigxnExpr(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position) 
    : MigxnTree(Text, Index, Position)
{
    
}