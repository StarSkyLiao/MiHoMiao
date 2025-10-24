using System.Text.RegularExpressions;
using MiHoMiao.Core.Collections.Generic;
using MiHoMiao.Migxn.Analysis.Tokenize.Tokens.Integers;

namespace MiHoMiao.Migxn.Analysis.Tokenize.Core;

public partial class IntegerFactory : TokenFactory
{
    [GeneratedRegex(@"^0[b|B][01_]*[01]L?")]
    private static partial Regex BinIntegerRegex();
    
    [GeneratedRegex(@"^\d(?:_*\d+)*L?")]
    private static partial Regex DecIntegerRegex();
    
    [GeneratedRegex(@"^0[x|X][\da-fA-F_]*[\da-fA-F]L?")]
    private static partial Regex HexIntegerRegex();
    
    public override MigxnToken? ParseToken(TokenizeContext context)
    {
        ReadOnlySpan<char> span = context.CharStream;
        if (!char.IsDigit(span[0])) return null;
        if (span[0] is not '0' || span.Length is 1) return ParseByRegex(DecIntegerRegex(), context, Singleton<DecInteger>.Instance);
        return span[1] switch
        {
            'b' or 'B' => ParseByRegex(BinIntegerRegex(), context, Singleton<BinInteger>.Instance),
            'x' or 'X' => ParseByRegex(HexIntegerRegex(), context, Singleton<HexInteger>.Instance),
            _ => ParseByRegex(DecIntegerRegex(), context, Singleton<DecInteger>.Instance)
        };
    }
    
}