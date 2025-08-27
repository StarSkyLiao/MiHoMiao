using MiHoMiao.Core.Collections.Unsafe;

namespace MiHoMiao.Migxin.FrontEnd.Syntax;

public abstract record MigxinTree(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : MigxinNode(Text, Index, Position)
{
    public override int NextColumn => Children().Last().NextColumn;
    
    internal abstract IEnumerable<MigxinNode> Children();
    
    internal override string ToStringImpl(int level)
    {
        // 使用缩进表示层级
        string indent = new string(' ', level * 2);
        // 构建当前节点的信息
        InterpolatedString sb = new InterpolatedString();
        sb.AppendLine(SelfString());

        // 处理子节点
        List<MigxinNode> children = Children().ToList();
        if (children.Count == 0) return sb.ToString();
        for (int i = 0; i < children.Count; i++)
        {
            MigxinNode child = children[i];
            bool isLast = i == children.Count - 1;
            string childPrefix = isLast ? "└── " : "├── ";
            // 递归调用子节点的 ToStringImpl
            sb.Append($"{indent}  {childPrefix}");
            string childString = child.ToStringImpl(level + 2);
            sb.Append(childString);
        }

        return sb.ToString();
    }

    protected virtual string SelfString() => $"{GetType().Name} (Line: {Position.Line}, Column: {Position.Column}): {Text.ToString()}";

}