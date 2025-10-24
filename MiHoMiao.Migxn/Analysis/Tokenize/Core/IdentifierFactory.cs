using MiHoMiao.Core.Collections.Generic;
using MiHoMiao.Migxn.Analysis.Tokenize.Tokens.Identifiers;
using MiHoMiao.Migxn.Collections;

namespace MiHoMiao.Migxn.Analysis.Tokenize.Core;

public class IdentifierFactory : TokenFactory
{
    public override MigxnToken? ParseToken(TokenizeContext context)
    {
        ReadOnlySpan<char> span = context.CharStream;
        int initialLength = (span[0] is '@') ? 1 : 0;
        int len = initialLength;
        if (!char.IsLetter(span[len]) && span[len] is not '_') return null;
        for (++len; len < span.Length; len++)
        {
            char c = span[len];
            if (!char.IsLetterOrDigit(c) && c is not '_') break;
        }
        TextSpan slice = new TextSpan(context.CurrSpan.Start, len);
        context.Accept(len);
        return new MigxnToken(context, slice, (span[0] is '@') ? Singleton<RawIdentifier>.Instance : Singleton<Identifier>.Instance);
    }
}