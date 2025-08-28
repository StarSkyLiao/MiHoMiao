namespace MiHoMiao.Migxin.Compiler.Semantic;

public struct MigxinType(Type type)
{
    /// <summary>
    /// 实际的运行时类型
    /// </summary>
    public Type RuntimeType { get; set; } = type;
    
    /// <summary>
    /// 是否是可空类型
    /// </summary>
    public bool Nullable { get; set; } = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

    public static implicit operator MigxinType(Type type) => new MigxinType(type);

}