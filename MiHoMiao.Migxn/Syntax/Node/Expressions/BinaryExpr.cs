using System.Reflection.Emit;
using MiHoMiao.Migxn.Exceptions.Grammar;
using MiHoMiao.Migxn.Reflection;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Tokens;
using MiHoMiao.Migxn.Syntax.Tokens.Punctuations;

namespace MiHoMiao.Migxn.Syntax.Node.Expressions;

internal record BinaryExpr : MigxnExpr
{
    public BinaryExpr(MigxnExpr Left, IBinaryToken Operator, MigxnExpr Right, bool IsInCurly = false) 
        : base($"{Left.Text} {((MigxnNode)Operator).Text} {Right.Text}".AsMemory(), IsInCurly)
    {
        this.Left = Left;
        this.Operator = Operator;
        this.Right = Right;
        Left.ParentNode = Right.ParentNode = ((MigxnNode)Operator).ParentNode = this;
    }

    public override Type ExpressionType(MigxnContext context) => Operator.BinaryType(
        ((MigxnToken)Operator).Position,
        Left.ExpressionType(context),
        Right.ExpressionType(context)
    );
    
    protected override IEnumerable<MigxnNode> Children() => [Left, (MigxnNode)Operator, Right];

    internal override void EmitCode(MigxnContext context, ILGenerator generator)
    {
        Left.EmitCode(context, generator);
        Right.EmitCode(context, generator);
        generator.Emit(Operator.BinaryOpCode);
    }

    public MigxnExpr Left { get; init; }
    public IBinaryToken Operator { get; init; }
    public MigxnExpr Right { get; init; }


}