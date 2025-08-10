using System.Reflection;
using System.Reflection.Emit;

namespace MiHoMiao.Migxn.Runtime;

public record MigxnVariable(string Name, Type Type)
{
    public int? ParamIndex = null;
    
    public FieldInfo? FieldInfo = null;
    
    public LocalBuilder? LocalBuilder = null;
    
    public void LoadVar(ILGenerator generator)
    {
        if (FieldInfo is not null)
        {
            generator.Emit(OpCodes.Ldfld, FieldInfo);
        }
        else if (ParamIndex != null)
        {
            switch (ParamIndex.Value)
            {
                case 0: generator.Emit(OpCodes.Ldarg_0);
                    break;
                case 1: generator.Emit(OpCodes.Ldarg_1);
                    break;
                case 2: generator.Emit(OpCodes.Ldarg_2);
                    break;
                case 3: generator.Emit(OpCodes.Ldarg_3);
                    break;
                case > byte.MinValue and < byte.MaxValue:
                    generator.Emit(OpCodes.Ldarg_S, (byte)ParamIndex.Value);
                    break;
                default: generator.Emit(OpCodes.Ldarg, (short)ParamIndex.Value);
                    break;
            }
        }
        else if (LocalBuilder != null)
        {
            switch (LocalBuilder.LocalIndex)
            {
                case 0: generator.Emit(OpCodes.Ldloc_0);
                    break;
                case 1: generator.Emit(OpCodes.Ldloc_1);
                    break;
                case 2: generator.Emit(OpCodes.Ldloc_2);
                    break;
                case 3: generator.Emit(OpCodes.Ldloc_3);
                    break;
                case > byte.MinValue and < byte.MaxValue:
                    generator.Emit(OpCodes.Ldloc_S, LocalBuilder);
                    break;
                default: generator.Emit(OpCodes.Ldloc, LocalBuilder);
                    break;
            }
        }
    }
    
    public void StoreVar(ILGenerator generator)
    {
        if (FieldInfo is not null)
        {
            generator.Emit(OpCodes.Stfld, FieldInfo);
        }
        else if (ParamIndex != null)
        {
            switch (ParamIndex.Value)
            {
                case > byte.MinValue and < byte.MaxValue:
                    generator.Emit(OpCodes.Starg_S, (byte)ParamIndex.Value);
                    break;
                default: generator.Emit(OpCodes.Starg, (short)ParamIndex.Value);
                    break;
            }
        }
        else if (LocalBuilder != null)
        {
            switch (LocalBuilder.LocalIndex)
            {
                case 0: generator.Emit(OpCodes.Stloc_0);
                    break;
                case 1: generator.Emit(OpCodes.Stloc_1);
                    break;
                case 2: generator.Emit(OpCodes.Stloc_2);
                    break;
                case 3: generator.Emit(OpCodes.Stloc_3);
                    break;
                case > byte.MinValue and < byte.MaxValue:
                    generator.Emit(OpCodes.Stloc_S, LocalBuilder);
                    break;
                default: generator.Emit(OpCodes.Stloc, LocalBuilder);
                    break;
            }
        }
    }
    
}