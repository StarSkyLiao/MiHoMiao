using System.Reflection.Emit;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Tokens.Literals;

namespace MiHoMiao.Migxn.Syntax.Node.Expressions;

public record LiteralExpr(LiteralToken LiteralToken) : MigxnExpr(LiteralToken.Text, false)
{
    
    protected override IEnumerable<MigxnNode> Children() => [LiteralToken];

    internal override void EmitCode(MigxnContext context, ILGenerator generator) => LiteralToken.EmitCode(context, generator);

    public override Type ExpressionType(MigxnContext context) 
        => LiteralToken.ExpressionType(context);
    
}