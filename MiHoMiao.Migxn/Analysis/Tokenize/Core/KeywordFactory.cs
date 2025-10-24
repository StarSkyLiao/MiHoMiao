using MiHoMiao.Core.Collections.Generic;
using MiHoMiao.Migxn.Analysis.Tokenize.Tokens;
using MiHoMiao.Migxn.Analysis.Tokenize.Tokens.Identifiers;
using MiHoMiao.Migxn.Analysis.Tokenize.Tokens.Keywords.Definition;
using MiHoMiao.Migxn.Collections;

namespace MiHoMiao.Migxn.Analysis.Tokenize.Core;

public class KeywordFactory : TokenFactory
{
    public static readonly TextTrie<Token> Keywords = new TextTrie<Token>
    {
        ["var"] = Singleton<Var>.Instance,
        ["val"] = Singleton<Val>.Instance,
    };
    
    public override MigxnToken? ParseToken(TokenizeContext context)
    {
        ReadOnlySpan<char> span = context.CharStream;
        int len = 0;
        if (!char.IsLetter(span[len]) && span[len] is not '_') return null;
        for (++len; len < span.Length; len++)
        {
            char c = span[len];
            if (!char.IsLetterOrDigit(c) && c is not '_') break;
        }
        ReadOnlySpan<char> ident = span[..len];
        TextSpan slice = new TextSpan(context.CurrSpan.Start, len);
        context.Accept(len);
        return new MigxnToken(context, slice, Keywords[ident] ?? Singleton<Identifier>.Instance);
    }
}