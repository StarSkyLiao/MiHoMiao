using MiHoMiao.Jarfter.Exception;

namespace MiHoMiao.Jarfter.Core;

public class JarfterContext(JarfterInterpreter jarfterInterpreter, IFormatProvider? provider = null) : IFormatProvider
{
    public IFormatProvider Provider => provider ?? this;
    public JarfterInterpreter JarfterInterpreter => jarfterInterpreter;

    public readonly Stack<object?> CalculationStack = [];
    
    public JarfterSymbolTable<object> JarfterSymbolTable = new JarfterSymbolTable<object>();
    
    public JarfterSymbolTable<(JarfterFrame, int)> JarfterLabelTable = new JarfterSymbolTable<(JarfterFrame, int)>();

    public readonly Stack<JarfterFrame> CallingTree = [];
    
    public void PushStack(JarfterFrame jarfterFrame)
    {
        JarfterSymbolTable = new JarfterSymbolTable<object>(JarfterSymbolTable);
        JarfterLabelTable = new JarfterSymbolTable<(JarfterFrame, int)>(JarfterLabelTable);
        CallingTree.Push(jarfterFrame);
    }
    
    public void PopStack()
    {
        if (CallingTree.Count == 0) throw new InvalidCallingTreeException();
        CallingTree.Pop();
        JarfterLabelTable = JarfterLabelTable.ParentTable!;
        JarfterSymbolTable = JarfterSymbolTable.ParentTable!;
    }

    public object? GetFormat(Type? formatType) => this;

}