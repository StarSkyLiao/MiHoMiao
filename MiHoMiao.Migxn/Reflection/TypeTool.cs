using System.Reflection.Emit;
using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Exceptions.Grammar;

namespace MiHoMiao.Migxn.Reflection;

public static class TypeTool
{
    private static readonly Dictionary<(Type, Type), Action<ILGenerator>> s_ConvertTable =
        new Dictionary<(Type, Type), Action<ILGenerator>>
        {
            [(typeof(long), typeof(char))] = iLGenerator => iLGenerator.Emit(OpCodes.Conv_I8),
            [(typeof(double), typeof(char))] = iLGenerator => iLGenerator.Emit(OpCodes.Conv_R8),
            [(typeof(double), typeof(long))] = iLGenerator => iLGenerator.Emit(OpCodes.Conv_R8),
        };

    /// <summary>
    /// 返回 target 是否可以通过 source 转化而来
    /// </summary>
    public static Result<Action<ILGenerator>?> IsTypeConvertible(int position, Type target, Type source)
    {
        if (target == source) return new Result<Action<ILGenerator>?>(value: null);
        if (s_ConvertTable.TryGetValue((target, source), out var action)) return action;
        if (target.IsAssignableFrom(source)) return new Result<Action<ILGenerator>?>(value: null);
        return new Result<Action<ILGenerator>?>(exception: new TypeNotFitException(position, target, source));
    }
}