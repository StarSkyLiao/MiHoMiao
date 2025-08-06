
using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Runtime.Collection;

namespace MiHoMiao.Jarfter.Runtime.Core;

public class JarfterContext(JarfterInterpreter jarfterInterpreter) : IFormatProvider
{
    public JarfterInterpreter JarfterInterpreter => jarfterInterpreter;

    public readonly Stack<object?> CalculationStack = [];
    
    public JarfterSymbolTable<JarfterObject> JarfterSymbolTable = new JarfterSymbolTable<JarfterObject>();
    
    public JarfterSymbolTable<(JarfterFrame, int)> JarfterLabelTable = new JarfterSymbolTable<(JarfterFrame, int)>();

    public readonly Stack<JarfterFrame> CallingTree = [];

    public readonly Dictionary<string, object> Environment = [];

    public int ParsingIndex = 0;
    
    public void PushStack(JarfterFrame jarfterFrame)
    {
        JarfterSymbolTable = new JarfterSymbolTable<JarfterObject>(JarfterSymbolTable);
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