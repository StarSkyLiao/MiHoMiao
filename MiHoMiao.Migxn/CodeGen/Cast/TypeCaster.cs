using System.Diagnostics;
using System.Reflection;
using MiHoMiao.Migxn.CodeGen.Data.Load;
using MiHoMiao.Migxn.CodeGen.Flow;

namespace MiHoMiao.Migxn.CodeGen.Cast;

internal static class TypeCaster
{
    public static bool CastType(Type from, Type to, List<MigxnOpCode> codes, int index, bool softCast = false)
    {
        if (from == to) return true;
        Debug.Assert(index > 0);
        MigxnOpCode last = codes[index - 1];
        if (IsFloat(to) && last is OpLdcLong loadLong)
        {
            codes[index - 1] = new OpLdcFloat(loadLong.Value);
        }
        else if (IsInteger(to) && last is OpLdcFloat loadFloat)
        {
            codes[index - 1] = new OpLdcLong((long)loadFloat.Value);
        }
        else if (to == typeof(string) && !softCast)
        {
            if (last is OpLdc loadConst and not OpLdcStr)
            {
                codes[index - 1] = new OpLdcStr(loadConst.AsString());
            }
            else if (!to.IsValueType)
            {
                Delegate toString = new object().ToString;
                codes[index - 1] = new OpCallVirtual(toString.Method);
            }
            else
            {
                MethodInfo? method = to.GetMethod(nameof(ToString));
                if (method != null) codes.Insert(index, new OpCallVirtual(method));
                else
                {
                    Delegate toString = new object().ToString;
                    codes.Insert(index, new OpCallVirtual(toString.Method));
                }
            }
        }
        else if (to.IsAssignableFrom(from))
        {
            if (from.IsValueType) codes.Insert(index, new OpBox());
        }
        return false;
    }
    
    private static bool IsInteger(Type type) => type == typeof(char) || type == typeof(int) || type == typeof(long);

    private static bool IsFloat(Type type) => type == typeof(float) || type == typeof(double);
}