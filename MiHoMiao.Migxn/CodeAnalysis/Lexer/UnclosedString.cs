namespace MiHoMiao.Migxn.CodeAnalysis.Lexer;

/// <summary>
/// 字符串 text 为 position 开始的一个 token.
/// </summary>
public sealed class UnclosedString((int Line, int Column) position, ReadOnlyMemory<char> text) : Exception
{
    public override string Message => $"UnclosedString at line {position.Line} column {position.Column}. (:{text}).";
}