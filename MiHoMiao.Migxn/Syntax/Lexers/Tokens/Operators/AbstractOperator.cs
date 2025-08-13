namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

public record AbstractOperator(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : MigxnToken(Text, Index, Position)
{
    
}