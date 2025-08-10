using System.Reflection.Emit;

namespace MiHoMiao.Migxn.Runtime;

public class MigxnContext
{
    internal MigxnSymbols<MigxnVariable> Variables = new MigxnSymbols<MigxnVariable>();
    
    internal MigxnSymbols<Label> Labels = new MigxnSymbols<Label>();
    
    internal readonly Stack<MigxnFrame> CallingTree = [];
    
    internal void PushStack(MigxnFrame frame)
    {
        Variables = new MigxnSymbols<MigxnVariable>(Variables);
        Labels = new MigxnSymbols<Label>(Labels);
        CallingTree.Push(frame);
    }
    
    internal void PopStack()
    {
        if (!CallingTree.TryPop(out _)) throw new NotSupportedException("Calling tree is invalid!");
        Variables = Variables.ParentTable!;
        Labels = Labels.ParentTable!;
    }
}