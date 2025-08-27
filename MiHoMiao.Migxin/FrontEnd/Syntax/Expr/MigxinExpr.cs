using MiHoMiao.Migxin.CodeAnalysis;
using MiHoMiao.Migxin.CodeAnalysis.Grammar;
using MiHoMiao.Migxin.FrontEnd.Lexical;
using MiHoMiao.Migxin.FrontEnd.Lexical.Literals;
using MiHoMiao.Migxin.FrontEnd.Lexical.Names;
using MiHoMiao.Migxin.FrontEnd.Syntax.Expr.Binary;

namespace MiHoMiao.Migxin.FrontEnd.Syntax.Expr;

internal abstract record MigxinExpr(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position) 
    : MigxinTree(Text, Index, Position)
{
    public static MigxinResult<MigxinExpr> TryParse(MigxinGrammar migxinGrammar)
    {
        MigxinExpr? curr = null;
        start:
        MigxinToken? token = migxinGrammar.Current;
        if (token == null) goto end;
        if (token is LiteralToken literal)
        {
            if (curr != null) goto end; 
            migxinGrammar.MoveNext();
            curr = new LiteralExpr(literal);
            goto start;
        }
        
        if (token is NameToken name)
        {
            if (curr != null) goto end; 
            migxinGrammar.MoveNext();
            curr = new NameExpr(name);
            goto start;
        }

        if (BinaryExpr.BinaryParsers.TryGetValue(token.GetType(), out IOperatorSymbol.BinaryParser? parser))
        {
            migxinGrammar.MoveNext();
            curr = parser(curr, migxinGrammar);
            goto start;
        }
            
        end:
        return curr != null ? curr : new DiagnosticBag(new ShouldBe(migxinGrammar.Position, nameof(MigxinExpr)));
    }
    
}