using System.Reflection.Emit;
using MiHoMiao.Core.Collections.Unsafe;
using MiHoMiao.Migxn.Runtime;

namespace MiHoMiao.Migxn.Syntax.Node;

public abstract record MigxnNode(ReadOnlyMemory<char> Text)
{
    protected abstract IEnumerable<MigxnNode> Children();

    internal MigxnNode? ParentNode { get; set; }

    internal abstract void EmitCode(MigxnContext context, ILGenerator generator);
    
    
    public string PrettyPrint(string indent = "", bool isLast = false)
    {
        using MutableString mutableString = new MutableString(16);

        string marker = isLast ? "└── " : "├── ";
        
        mutableString.Append(indent);
        mutableString.Append(marker);
        mutableString.Append(GetType().Name);
        mutableString.Append(':');

        mutableString.Append(Text);

        mutableString.Append('\n');

        indent += isLast ? "    " : "|   ";
        
        MigxnNode? last = Children().LastOrDefault();
        
        foreach (MigxnNode child in Children()) mutableString.Append(child.PrettyPrint(indent, child == last));

        return mutableString.ToString();
    }
    
}