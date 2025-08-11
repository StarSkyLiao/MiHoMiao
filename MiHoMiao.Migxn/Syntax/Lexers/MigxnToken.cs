namespace MiHoMiao.Migxn.Syntax.Lexers;

public abstract record MigxnToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
{
    
}