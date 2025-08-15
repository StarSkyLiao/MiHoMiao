namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

internal record VarToken(int Index, (int Line, int Column) Position)
    : AbstractKeyword(UniqueName.AsMemory(), Index, Position), IKeywordToken //, ILeadToken
{
    public static string UniqueName => "var";

    public static AbstractKeyword Create(int index, (int Line, int Column) position) => new VarToken(index, position);

    // public Result<MigxnTree> TryCollectToken(MigxnGrammar migxnGrammar)
    // {
    //     MigxnToken? token = migxnGrammar.MoveNext();
    //     Debug.Assert(token is VarToken);
    //     
    //     if (!migxnGrammar.TryMatchToken(out SymbolToken? identifier))
    //         return new SpecifiedTokenMissing(new BadTree([this]), nameof(identifier));
    //     Debug.Assert(identifier != null);
    //     
    //     // 情况 1：var item : Type [= expr]
    //     if (migxnGrammar.TryMatchToken(out ColonToken? colon))
    //     {
    //         Debug.Assert(colon != null);
    //         if (!migxnGrammar.TryMatchToken(out SymbolToken? varType))
    //             return new SpecifiedTokenMissing(new BadTree([this, identifier, colon]), nameof(varType));
    //         Debug.Assert(varType != null);
    //
    //         if (!migxnGrammar.TryMatchToken(out EqualToken? equal))
    //             return new VarStmt(this, identifier, colon, varType, null, null);
    //         Debug.Assert(equal != null);
    //
    //         if (migxnGrammar.TryParseTree(out MigxnExpr? initialExpr))
    //             return new VarStmt(this, identifier, colon, varType, equal, initialExpr);
    //         return new SpecifiedTokenMissing(
    //             new BadTree([this, identifier, colon, varType, equal]), nameof(initialExpr)
    //         );
    //     }
    //     // 情况 2：var item = expr
    //     if (migxnGrammar.TryMatchToken(out EqualToken? equalToken))
    //     {
    //         Debug.Assert(equalToken != null);
    //         if (migxnGrammar.TryParseTree(out MigxnExpr? initialExpr))
    //             return new VarStmt(this, identifier, null, null, equalToken, initialExpr);
    //         return new SpecifiedTokenMissing(
    //             new BadTree([this, identifier, equalToken]), nameof(initialExpr)
    //         );
    //     }
    //
    //     return new SpecifiedTokenMissing(
    //         new BadTree([this, identifier]), "varType or initialExpr"
    //     );
    // }
    
}