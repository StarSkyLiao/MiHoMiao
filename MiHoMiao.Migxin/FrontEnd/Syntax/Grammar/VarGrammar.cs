using System.Diagnostics;
using MiHoMiao.Migxin.CodeAnalysis;
using MiHoMiao.Migxin.CodeAnalysis.Grammar;
using MiHoMiao.Migxin.FrontEnd.Lexical.Keywords.Core;
using MiHoMiao.Migxin.FrontEnd.Lexical.Names;
using MiHoMiao.Migxin.FrontEnd.Lexical.Operators.Advance;
using MiHoMiao.Migxin.FrontEnd.Lexical.Operators.Assign;
using MiHoMiao.Migxin.FrontEnd.Syntax.Expr;
using MiHoMiao.Migxin.FrontEnd.Syntax.Stmt;

namespace MiHoMiao.Migxin.FrontEnd.Syntax.Grammar;

internal class VarGrammar: ITokenGrammar
{
    public static Type TokenType => typeof(VarToken);
    public static MigxinResult<MigxinStmt> TryMatchStmt(MigxinGrammar migxinGrammar)
    {
        migxinGrammar.CreateFrame();
        VarToken? varToken = migxinGrammar.MoveNext() as VarToken;
        Debug.Assert(varToken is not null);

        NameToken? varName = migxinGrammar.TryMatchToken<NameToken>();
        // error: var
        if (varName is null) return new DiagnosticBag(new ShouldBe(migxinGrammar.Position, nameof(varName)));

        NameToken? varType = null;
        ColonToken? colon = migxinGrammar.TryMatchToken<ColonToken>();
        if (colon != null)
        {
            varType = migxinGrammar.TryMatchToken<NameToken>();
            // error: var item :
            if (varType is null) return new DiagnosticBag(new ShouldBe(migxinGrammar.Position, nameof(varType)));
        }
        
        // var item = expr
        AssignToken? equal = migxinGrammar.TryMatchToken<AssignToken>();
        if (equal == null)
        {
            // error: var item
            if (varType == null) return new DiagnosticBag(new TypeUnclear(migxinGrammar.Position, varName.Text.ToString()));
            // var item : type
            return new VarDeclareStmt(varToken, varName, varType);
        }

        MigxinResult<MigxinExpr> initialExpr = MigxinExpr.TryParse(migxinGrammar);
        // var item : type = expr
        if (initialExpr.IsSuccess)
        {
            if (varType is null) return new VarAutoInitStmt(varToken, varName, initialExpr.Result!);
            return new VarInitializeStmt(varToken, varName, varType, initialExpr.Result!);
        }
        // error: var item : type = 
        initialExpr.Exception!.DiagnosticsContained.Add(new ShouldBe(migxinGrammar.Position, nameof(MigxinExpr)));
        return initialExpr.Exception;

    }
}