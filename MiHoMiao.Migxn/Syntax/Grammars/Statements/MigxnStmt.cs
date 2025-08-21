namespace MiHoMiao.Migxn.Syntax.Grammars.Statements;

internal abstract record MigxnStmt(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position) 
    : MigxnTree(Text, Index, Position)
{
    
}