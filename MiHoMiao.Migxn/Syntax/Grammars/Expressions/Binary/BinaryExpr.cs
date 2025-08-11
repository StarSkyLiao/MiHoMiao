namespace MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;

public record BinaryExpr(MigxnExpr Left, IBinaryToken BinaryToken, MigxnExpr Right)
    : MigxnExpr($"{Left.Text} {(BinaryToken as MigxnNode)!.Text} {Right.Text}".AsMemory(), Left.Index, Left.Position)
{
    
    internal readonly MigxnNode BinaryNode = (BinaryToken as MigxnNode)!;

    internal override IEnumerable<MigxnNode> Children() => [Left, BinaryNode, Right];
    
}