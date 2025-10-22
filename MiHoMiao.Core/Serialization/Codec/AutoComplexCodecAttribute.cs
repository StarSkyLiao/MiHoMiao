namespace MiHoMiao.Core.Serialization.Codec;

/// <summary>
/// 需要使用 MiHoMiao.xGenerator.Codec 程序集来进行 Codec 接口自动实现.
/// 仅支持 record 类型的值.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
public sealed class AutoComplexCodecAttribute : Attribute;