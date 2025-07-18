using JetBrains.Annotations;
using MiHoMiao.Jarfter.Exception;

namespace MiHoMiao.Jarfter.Core.Func;

[UsedImplicitly]
public interface IJarfterFunc
{
    public abstract string[] JarfterFuncName { get; }

    public abstract void RunJarfterFunc(JarfterContext jarfterContext, params Span<string> argSpan);
    
    public static T JarfterParse<T>(JarfterContext jarfterContext, string input) where T : ISpanParsable<T>
    {
        bool isFunc = input.Length > 3 && input[0] is '@' && input[1] is '{' && input[^1] is '}';
        if (!isFunc && input[0] is '@')
        {
            string varName = input[1..];
            object? variable = jarfterContext.JarfterSymbolTable.LoadVariable(varName);
            if (variable is T item) return item;
            if (variable is null) throw new InvalidTypeException<T>(varName);
            return T.Parse(variable.ToString(), jarfterContext.Provider);
        }
        if (!isFunc) return T.Parse(input, jarfterContext.Provider);
        // 记录当前计算栈的长度
        int currStackDepth = jarfterContext.CalculationStack.Count;
        // 运行子代码段
        jarfterContext.JarfterInterpreter.Run(input.AsSpan()[2..^1]);
        // 如果计算栈没有变深, 说明存在错误
        if (jarfterContext.CalculationStack.Count <= currStackDepth)
            throw new FuncReturnNullException(input[2..^1]);
        // 计算栈的栈顶为返回值
        object? funcReturn = jarfterContext.CalculationStack.Pop();
        // 还原计算栈
        while (jarfterContext.CalculationStack.Count > currStackDepth) jarfterContext.CalculationStack.Pop();
        if (funcReturn is T result) return result;
        if (funcReturn is null) throw new InvalidTypeException<T>(input);
        try
        {
            return T.Parse(funcReturn.ToString().AsSpan(), null);
        }
        catch (System.Exception)
        {
            throw new InvalidTypeException<T>(input);
        }
    }
}
