using System.Text.RegularExpressions;
using MiHoMiao.Core.Collections.Generic;
using MiHoMiao.Migxn.Analysis.Tokenize.Tokens;
using MiHoMiao.Migxn.Analysis.Tokenize.Tokens.Integers;
using MiHoMiao.Migxn.Analysis.Tokenize.Tokens.Reals;
using MiHoMiao.Migxn.Collections;

namespace MiHoMiao.Migxn.Analysis.Tokenize.Core;

public partial class RealNumberFactory : TokenFactory
{
    [GeneratedRegex(@"^\d+(?:\.\d+)?(?:[eE][+-]?\d+)?[fmdFMD]?", RegexOptions.NonBacktracking)]
    private static partial Regex RealNumberRegex();
    
    [GeneratedRegex(@"^\.\d+?", RegexOptions.NonBacktracking)]
    private static partial Regex HasDecimalRegex();
    
    [GeneratedRegex(@"^[eE][+-]?\d+", RegexOptions.NonBacktracking)]
    private static partial Regex HasExponentRegex();
    
    [GeneratedRegex(@"^[fmdFMD]", RegexOptions.NonBacktracking)]
    private static partial Regex HasSuffixRegex();
    
    public override MigxnToken? ParseToken(TokenizeContext context)
    {
        ReadOnlySpan<char> span = context.CharStream;
        Regex.ValueMatchEnumerator enumerator = RealNumberRegex().EnumerateMatches(span).GetEnumerator();
        if (!enumerator.MoveNext()) return null;
        int len = enumerator.Current.Length;
        TextSpan slice = new TextSpan(context.CurrSpan.Start, len);
        Token? token = span[len] switch
        {
            'f' or 'F' => Singleton<FloatReal>.Instance,
            'd' or 'D' => Singleton<DoubleReal>.Instance,
            'm' or 'M' => Singleton<DecimalReal>.Instance,
            _ => null,
        };
        if (token == null)
        {
            if (span[..len].ContainsAny('.', 'e', 'E')) token = Singleton<DoubleReal>.Instance;
            else token = Singleton<DecInteger>.Instance;
        }
        context.Accept(len);
        return new MigxnToken(context, slice, token);
    }
}