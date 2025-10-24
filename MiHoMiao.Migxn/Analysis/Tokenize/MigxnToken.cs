using MiHoMiao.Migxn.Analysis.Tokenize.Tokens;
using MiHoMiao.Migxn.Collections;

namespace MiHoMiao.Migxn.Analysis.Tokenize;

public record MigxnToken(TokenizeContext Tokenizer, TextSpan TextSpan, Token Token)
{
    public ReadOnlySpan<char> Text => Tokenizer.RawText.AsSpan()[TextSpan.Start..TextSpan.End];
    
    public string Display() => $"{Token.GetType().Name}.{Text}";
}