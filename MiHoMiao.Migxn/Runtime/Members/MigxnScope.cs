using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Runtime.Variable;

using ScopedTable = System.Collections.Generic.Dictionary<string, MiHoMiao.Migxn.Runtime.Variable.MigxnVariable>;
    
namespace MiHoMiao.Migxn.Runtime.Members;

/// <summary>
/// 表示一个基本的作用域.
/// 变量声明与加载都在其中进行.
/// </summary>
internal record MigxnScope
{
    private static readonly Queue<ScopedTable> s_ScopedTablePool = [];

    /// <summary>
    /// 该作用域中存储的变量
    /// </summary>
    public readonly Stack<ScopedTable> ScopedVariables = new Stack<ScopedTable>([[]]);

    /// <summary>
    /// 进入一个子作用域
    /// </summary>
    public void EnterScope() => ScopedVariables.Push(s_ScopedTablePool.TryDequeue(out ScopedTable? table) ? table : []);

    /// <summary>
    /// 离开当前作用域
    /// </summary>
    public void ExitScope() => s_ScopedTablePool.Enqueue(ScopedVariables.Pop());
    
    /// <summary>
    /// 检查在此处是否允许声明指定的变量
    /// </summary>
    public bool IsAbleToDeclareVariable(string name) => !ScopedVariables.Peek().ContainsKey(name);
    
    /// <summary>
    /// 声明一个变量
    /// </summary>
    public Exception? DeclareVariable(MigxnVariable variable)
    {
        if (variable.Name == "_" || ScopedVariables.Peek().TryAdd(variable.Name, variable)) return null;
        return new NotSupportedException($"Variable \"{variable.Name}\" is already declared!");
    }
    
    /// <summary>
    /// 加载一个变量
    /// </summary>
    public Result<MigxnVariable> LoadVariable(string name)
    {
        if (name == "_") return new NotSupportedException("Variable \"_\" is not declared!");
        foreach (ScopedTable table in ScopedVariables)
        {
            if (table.TryGetValue(name, out MigxnVariable? value)) return value;
        }
        return new NotSupportedException($"Variable \"{name}\" is not declared!");
    }
}