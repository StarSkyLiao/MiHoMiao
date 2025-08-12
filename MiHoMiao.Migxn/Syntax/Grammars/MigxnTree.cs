using System.Text;

namespace MiHoMiao.Migxn.Syntax.Grammars;

public abstract record MigxnTree(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position) 
    : MigxnNode(Text, Index, Position)
{
    internal abstract IEnumerable<MigxnNode?> Children();
    
    internal override string ToStringImpl(int level)
    {
        // 使用缩进表示层级
        string indent = new string(' ', level * 2);
        // 构建当前节点的信息
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"{GetType().Name} (Line: {Position.Line}, Column: {Position.Column}): {Text.ToString()}");

        // 处理子节点
        List<MigxnNode?> children = Children().ToList();
        if (children.Count == 0) return sb.ToString();
        for (int i = 0; i < children.Count; i++)
        {
            MigxnNode? child = children[i];
            bool isLast = i == children.Count - 1;
            string childPrefix = isLast ? "└── " : "├── ";
            // 递归调用子节点的 ToStringImpl
            sb.Append($"{indent}  {childPrefix}");
            string childString = child?.ToStringImpl(level + 2) ?? "null";
            if (isLast) sb.Append(childString);
            else sb.AppendLine(childString);
        }

        return sb.ToString();
    }
    
}