using System.Reflection.Emit;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Tokens.Punctuations;

namespace MiHoMiao.Migxn.Syntax.Node.Expressions;

internal record UnaryExpr : MigxnExpr
{
    public UnaryExpr(IUnaryToken op, MigxnExpr operand) 
        : base($"{((MigxnNode)op).Text} {operand.Text}".AsMemory(), false)
    {
        Operator = op;
        Operand = operand;
        ((MigxnNode)op).ParentNode = operand.ParentNode = this;
    }

    public override Type ExpressionType(MigxnContext context) 
        => Operand.ExpressionType(context);
    
    protected override IEnumerable<MigxnNode> Children() => [(MigxnNode)Operator, Operand];

    internal override void EmitCode(MigxnContext context, ILGenerator generator)
    {
        Operand.EmitCode(context, generator);
        generator.Emit(Operator.UnaryOpCode);
    }

    public IUnaryToken Operator { get; }
    public MigxnExpr Operand { get; }
}