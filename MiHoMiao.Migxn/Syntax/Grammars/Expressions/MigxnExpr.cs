using System.Diagnostics;
using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Syntax.Grammars.Exceptions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Param;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Prefix;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Suffix;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions;

public abstract record MigxnExpr(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position) 
    : MigxnTree(Text, Index, Position), IExprParser<MigxnExpr>
{
    static IResult<MigxnExpr> IExprParser<MigxnExpr>.TryParse(MigxnGrammar grammar)
    {
        if (grammar.Current is IPrefixToken) return PrefixExpr.ParseForward(grammar);
        IResult<MigxnExpr> current = ParseUnitExpr(grammar);
        if (!current.IsSuccess) return current;
        Debug.Assert(current.Result != null);

        switch (grammar.Current)
        {
            case ISuffixToken:
                current = SuffixExpr.ParseForward(current.Result, grammar);
                if (!current.IsSuccess) return current;
                break;
            case RoundOpenToken:
                current = ParamExpr.ParseForward(current.Result, grammar);
                if (!current.IsSuccess) return current;
                break;
        }

        if (grammar.Current is IBinaryToken) current = BinaryExpr.ParseForward(current.Result, grammar);
        return current;
    }

    internal static IResult<MigxnExpr> ParseUnitExpr(MigxnGrammar grammar)
    {
        switch (grammar.Current)
        {
            case RoundOpenToken: return grammar.TryParse<ParenthesizedExpr>();
            case LiteralToken: return grammar.TryParse<TokenExpr>();
            case null: return TokenMissingException.Create<MigxnExpr>([], nameof(MigxnExpr));
        }
        return new ActionResult<MigxnExpr>(new UnexpectedTokenException<MigxnExpr>(grammar.MoveNext()!.Text.ToString()));
    }
    
}