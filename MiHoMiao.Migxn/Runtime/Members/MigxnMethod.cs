using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;
using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Migxn.CodeGen.Flow;

namespace MiHoMiao.Migxn.Runtime.Members;

internal record MigxnMethod(MigxnContext Context, string Name, Type ReturnType, params Type[] MethodParams) : MigxnMember
{
    public override string ToString() =>
        $".method {ReturnType.Name} {Name}" +
        $"{MethodParams.GenericViewer(type => type.Name, "(", ")")} cil managed\n" +
        $"{Codes.GenericViewer("", "\n", "\n")}";

    private string? m_ReturnLabel = null;

    public string ReturnLabel => m_ReturnLabel ??= $"<ret>.{Name}";
    
    public void SetReturn()
    {
        if (m_ReturnLabel != null) EmitCode(new OpLabel(m_ReturnLabel));
        EmitCode(new OpRet());
    }

    /// <summary>
    /// 对应的动态方法
    /// </summary>
    [field: AllowNull, MaybeNull]
    internal DynamicMethod DynamicMethod => field ??= new DynamicMethod(Name, ReturnType, MethodParams);
    //
    // internal ILGenerator Generator => DynamicMethod.GetILGenerator();

}