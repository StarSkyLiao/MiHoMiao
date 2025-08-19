using System.Diagnostics;
using MiHoMiao.Migxn.CodeAnalysis;
using MiHoMiao.Migxn.CodeAnalysis.Grammar;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Param;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Prefix;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Suffix;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Pair;

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
                current = FuncCallExpr.ParseForward(current.Result, grammar);
                if (!current.IsSuccess) return current;
                break;
        }
        Debug.Assert(current.Result != null);
        if (grammar.Current is IBinaryToken) current = BinaryExpr.ParseForward(current.Result, grammar);
        return current;
    }

    internal static IResult<MigxnExpr> ParseUnitExpr(MigxnGrammar grammar)
    {
        switch (grammar.Current)
        {
            case RoundOpenToken: return grammar.TryParse<ParenthesizedExpr>();
            case LiteralToken: return grammar.TryParse<TokenExpr>();
            case null: return SpecifiedTokenMissing.Create<MigxnExpr>([], nameof(MigxnExpr));
        }
        return new Diagnostic<MigxnExpr>(new UnexpectedToken<MigxnExpr>(grammar.MoveNext()!));
    }
    
}