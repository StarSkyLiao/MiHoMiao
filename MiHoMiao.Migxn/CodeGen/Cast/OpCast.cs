using System.Reflection;
using MiHoMiao.Migxn.CodeGen.Data.Load;
using MiHoMiao.Migxn.CodeGen.Flow;

namespace MiHoMiao.Migxn.CodeGen.Cast;

internal class OpCast(Type from, Type to, bool softCast = false): MigxnOpCode
{
    public override string ToString() => $"{"cast",-12}{from} -> {to}";

    public override void OnEmitting(List<MigxnOpCode> codes, int index)
    {
        if (from == to) return;
        if (index == 0)
        {
            base.OnEmitting(codes, index);
            return;
        }
        MigxnOpCode last = codes[index - 1];
        if (IsFloat(to) && last is OpLdcLong loadLong)
        {
            codes[index - 1] = new OpLdcFloat(loadLong.Value);
            return;
        }

        if (IsInteger(to) && last is OpLdcFloat loadFloat)
        {
            codes[index - 1] = new OpLdcLong((long)loadFloat.Value);
            return;
        }
        if (!softCast && to == typeof(string))
        {
            if (last is OpLdc loadConst and not OpLdcStr)
            {
                codes[index - 1] = new OpLdcStr(loadConst.AsString());
                return;
            }
            if (!to.IsValueType)
            {
                Delegate toString = ToString;
                codes[index - 1] = new OpCallVirtual(toString.Method);
                return;
            }
            MethodInfo? method = to.GetMethod(nameof(ToString));
            if (method != null) codes.Insert(index, new OpCallVirtual(method));
            else
            {
                Delegate toString = ToString;
                codes.Insert(index, new OpCallVirtual(toString.Method));
            }
            return;
        }

        if (to.IsAssignableFrom(from) && from.IsValueType)
        {
            codes.Insert(index, new OpBox());
            return;
        }
        base.OnEmitting(codes, index);
        // throw new InvalidCastException($"OpCast: from {from} to {to}");
    }

    private static bool IsInteger(Type type) => type == typeof(char) || type == typeof(int) || type == typeof(long);

    private static bool IsFloat(Type type) => type == typeof(float) || type == typeof(double);
}