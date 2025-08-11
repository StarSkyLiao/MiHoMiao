namespace MiHoMiao.Migxn.Syntax.Grammars.Statements;

public abstract record MigxnStmt(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position) 
    : MigxnTree(Text, Index, Position)
{
    
}