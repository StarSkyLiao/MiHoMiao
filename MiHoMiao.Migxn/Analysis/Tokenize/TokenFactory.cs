using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using MiHoMiao.Core.Collections.Generic;
using MiHoMiao.Core.Reflection;
using MiHoMiao.Migxn.Analysis.Tokenize.Tokens;
using MiHoMiao.Migxn.Analysis.Tokenize.Tokens.Integers;
using MiHoMiao.Migxn.Collections;

namespace MiHoMiao.Migxn.Analysis.Tokenize;

public abstract class TokenFactory : ICollectable
{
    /// <summary>
    /// 从当前的字符流中提取 MigxnToken.
    /// </summary>
    public abstract MigxnToken? ParseToken(TokenizeContext context);

    protected static MigxnToken? ParseByRegex(Regex regex, TokenizeContext context, Token token)
    {
        ReadOnlySpan<char> span = context.CharStream;
        Regex.ValueMatchEnumerator enumerator = regex.EnumerateMatches(span).GetEnumerator();
        if (!enumerator.MoveNext()) return null;
        int len = enumerator.Current.Length;
        TextSpan slice = new TextSpan(context.CurrSpan.Start, len);
        context.Accept(len);
        return new MigxnToken(context, slice, token);
    }

    [field: AllowNull, MaybeNull] string ICollectable.UniqueName => field ??= GetType().Name;
    
    [field: AllowNull, MaybeNull] string[][] ICollectable.NameGroups => field ??= [[GetType().Name]];

}