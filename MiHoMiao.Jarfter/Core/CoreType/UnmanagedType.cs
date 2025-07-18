using System.Diagnostics.CodeAnalysis;
using MiHoMiao.Jarfter.Exception;

namespace MiHoMiao.Jarfter.Core.CoreType;

/// <summary>
/// 解析 .Net 原生的非托管类型
/// </summary>
public class UnmanagedType(TypeCode containedType) : ISpanParsable<UnmanagedType>
{
    public TypeCode ContainedType => containedType;

    public static UnmanagedType Parse(string s, IFormatProvider? provider) => new UnmanagedType(LoadUnmanagedType(s.AsSpan())!);
    
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider,
        [MaybeNullWhen(false)] out UnmanagedType result)
    {
        TypeCode resultType = LoadUnmanagedType(s.AsSpan());
        if (resultType is TypeCode.DBNull)
        {
            result = null;
            return false;
        }
        result = new UnmanagedType(resultType);
        return true;
    }

    public static UnmanagedType Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => new UnmanagedType(LoadUnmanagedType(s)!);

    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider,
        [MaybeNullWhen(false)] out UnmanagedType result)
    {
        TypeCode resultType = LoadUnmanagedType(s);
        if (resultType is TypeCode.DBNull)
        {
            result = null;
            return false;
        }
        result = new UnmanagedType(resultType);
        return true;
    }

    private static TypeCode LoadUnmanagedType(ReadOnlySpan<char> s)
    {
        return s switch
        {
            "bool" or "boolean" or "System.Boolean" => TypeCode.Boolean,
            "byte" or "int8" or "i8" or "System.Byte" => TypeCode.Byte,
            "short" or "int16" or "i16" or "System.Int16" => TypeCode.Int16,
            "int" or "int32" or "i32" or "System.Int32" => TypeCode.Int32,
            "long" or "int64" or "i64" or "System.Int64" => TypeCode.Int64,
            "float" or "float32" or "r32" or "System.Single" => TypeCode.Single,
            "double" or "float64" or "r64" or "System.Double" => TypeCode.Double,
            "decimal" or "float128" or "r128" or "System.Decimal" => TypeCode.Decimal,
        
            "char" or "System.Char" => TypeCode.Char,
            "string" or "System.String" => TypeCode.String,
            _ => throw new InvalidTypeException<UnmanagedType>(s.ToString())
        };
    }
    
}