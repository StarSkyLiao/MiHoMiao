using JetBrains.Annotations;
using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Runtime.Collection;
using MiHoMiao.Jarfter.Runtime.Core;

namespace MiHoMiao.Jarfter.Runtime.Function.Template;

[UsedImplicitly]
public interface IJarfterFunc
{
    public abstract string[] JarfterFuncName { get; }

    public abstract void RunJarfterFunc(JarfterContext jarfterContext, ReadOnlySpan<char> input);
    
    public static T JarfterParse<T>(JarfterContext jarfterContext, ReadOnlySpan<char> input) where T : ISpanParsable<T>
    {
        ref int startIndex = ref jarfterContext.ParsingIndex;
        while (startIndex < input.Length && char.IsWhiteSpace(input[startIndex])) ++startIndex;
        if (input[startIndex] is '@')
        {
            ++startIndex;
            // 运行 @{} 方法并获取其中值
            if (input[startIndex] is '{') return MethodAtGrammar<T>(jarfterContext, input, ref startIndex);

            // load 变量的语法糖
            JarfterAccessor variable = JarfterAccessor.Parse(input, jarfterContext);
            return JarfterObject.Convert<T>(
                jarfterContext.JarfterSymbolTable.LoadVariable(variable.VarName), variable.VarName, jarfterContext
            );
        }
        if (input[startIndex] is '(') return MethodAtGrammar<T>(jarfterContext, input, ref startIndex, true);
        // 正常解析的逻辑
        return JarfterObject.ElementParse<T>(input, jarfterContext);
    }

    private static T MethodAtGrammar<T>(JarfterContext jarfterContext, ReadOnlySpan<char> input, ref int startIndex,
        bool isAtGrammar = false)
        where T : ISpanParsable<T>
    {
        JarfterFunc func = isAtGrammar
            ? JarfterFunc.BracketParse(input, jarfterContext, '(', ')')
            : JarfterFunc.Parse(input, jarfterContext);
        // 记录当前计算栈的长度
        int currStackDepth = jarfterContext.CalculationStack.Count;
        // 记录原先的解析位置
        int rawParsingIndex = startIndex;
        // 运行子代码段
        jarfterContext.JarfterInterpreter.Run(func.FuncCode.AsSpan());
        // 还原解析位置
        jarfterContext.ParsingIndex = rawParsingIndex;
        // 如果计算栈没有变深, 说明存在错误
        if (jarfterContext.CalculationStack.Count <= currStackDepth)
            throw new FuncReturnNullException(func.FuncCode);
        // 计算栈的栈顶为返回值
        object? funcReturn = jarfterContext.CalculationStack.Pop();
        // // 还原计算栈
        // while (jarfterContext.CalculationStack.Count > currStackDepth) jarfterContext.CalculationStack.Pop();
        return JarfterObject.Convert<T>(funcReturn, func.FuncCode, jarfterContext);
    }
    
}
