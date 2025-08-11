namespace MiHoMiao.Migxn.Syntax.Lexers.Exceptions;

/// <summary>
/// 字符串 text 为 position 开始的一个 token.
/// </summary>
public class UnrecognisedTokenException((int Line, int Column) position, ReadOnlyMemory<char> text) : Exception
{
    public override string Message => $"Unrecognised token at line {position.Line} column {position.Column}. (:{text}).";
}