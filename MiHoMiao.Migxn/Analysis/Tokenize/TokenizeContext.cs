using MiHoMiao.Migxn.Collections;

namespace MiHoMiao.Migxn.Analysis.Tokenize;

/// <summary>
/// 分词器的上下文环境
/// </summary>
public class TokenizeContext(string rawText)
{
    public readonly string RawText = rawText;
    public TextSpan CurrSpan { get; private set; } = new TextSpan(0, rawText.Length);
    public ReadOnlySpan<char> CharStream => RawText.AsSpan()[CurrSpan.Start..CurrSpan.End];

    public char CurrentChar => CurrSpan.Start < RawText.Length ? RawText[CurrSpan.Start] : '\0';
    
    public char Peek(int offset) => RawText[CurrSpan.Start + offset];

    public ReadOnlySpan<char> Slice(int offset) => CharStream[..offset];

    public void TrimHead()
    {
        if (!char.IsWhiteSpace(CurrentChar)) return;
        int index = CurrSpan.Start + 1;
        ReadOnlySpan<char> span = RawText.AsSpan();
        while (index < span.Length && char.IsWhiteSpace(span[index])) ++index;
        CurrSpan = CurrSpan[(index - CurrSpan.Start)..];
    }

    public void Accept(int length = 1) => CurrSpan = CurrSpan[length..];
    
    public void AcceptWord()
    {
        if (!char.IsLetterOrDigit(CurrentChar))
        {
            Accept();
            return;
        }
        int index = CurrSpan.Start + 1;
        ReadOnlySpan<char> span = RawText.AsSpan();
        while (index < span.Length && char.IsLetterOrDigit(span[index])) ++index;
        CurrSpan = CurrSpan[(index - CurrSpan.Start)..];
    }
    
}