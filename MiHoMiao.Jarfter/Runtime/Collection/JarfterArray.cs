using System.Collections;
using System.Collections.Immutable;
using MiHoMiao.Core.Collections.Tool;
using MiHoMiao.Core.Numerics.Hash;
using MiHoMiao.Jarfter.Exception;
using MiHoMiao.Jarfter.Runtime.Core;

namespace MiHoMiao.Jarfter.Runtime.Collection;

public class JarfterArray<T>(T[] elements) : JarfterObject, IJarfterParsable<JarfterArray<T>>, IEquatable<JarfterArray<T>>, IStructuralEquatable
    where T : ISpanParsable<T>
{
    public T[] Content => elements;

    public override string ToString() => elements.GenericViewer();
    
    internal static JarfterArray<T> ParseInternal(ReadOnlySpan<char> input) => Parse(input, new JarfterContext(null!));
    
    public new static JarfterArray<T> Parse(ReadOnlySpan<char> input, IFormatProvider? provider)
    {
        if (provider is not JarfterContext context) throw new InvalidCallingTreeException();
        int index = context.ParsingIndex;
        while (index < input.Length && char.IsWhiteSpace(input[index])) ++index;
        if (input[index++] != '[') throw new UnBalancedArrayException("[", input[index..].ToString());

        List<T> elements = [];
        
        while (index < input.Length)
        {
            while (char.IsWhiteSpace(input[index]))
            {
                if (index < input.Length) ++index;
                else UnBalancedArrayException.ThrowAtEndOfLine("]");
            }
            if (input[index++] is ']') break;
            context.ParsingIndex = --index;
            elements.Add(ElementParse<T>(input, context));
            index = context.ParsingIndex;
            if (index >= input.Length) UnBalancedArrayException.ThrowAtEndOfLine("]");
            while (char.IsWhiteSpace(input[index]))
            {
                if (index < input.Length) ++index;
                else UnBalancedArrayException.ThrowAtEndOfLine(", or ]");
            }

            char token = input[index++];
            if (token is ']') break;
            if (token is ',') continue;
            throw new UnBalancedArrayException(",", input[index..].ToString());
        }
        while (index < input.Length && char.IsWhiteSpace(input[index])) ++index;
        context.ParsingIndex = index;
        return new JarfterArray<T>([..elements]);
    }

    private static bool IsPunctuation(char input) => char.IsPunctuation(input) && input is not '.' and not '_';
    
    public static bool operator ==(JarfterArray<T>? left, JarfterArray<T>? right) => Equals(left, right);

    public static bool operator !=(JarfterArray<T>? left, JarfterArray<T>? right) => !Equals(left, right);
    
    public override bool Equals(object? obj) => Equals(obj as JarfterArray<T>);
    
    public bool Equals(JarfterArray<T>? other)
    {
        if (other is not { } otherContainer) return false;

        if (ReferenceEquals(this, other)) return true;
        
        if (Content.Length != otherContainer.Content.Length) return false;
        
        return !Content.Where((t, i) => !Equals(t, otherContainer.Content[i])).Any();
    }
    
    public bool Equals(object? other, IEqualityComparer comparer)
    {
        if (other is not JarfterArray<T> otherContainer) return false;

        if (ReferenceEquals(this, otherContainer)) return true;

        if (Content.Length != otherContainer.Content.Length) return false;

        // 使用 comparer 比较数组元素
        return !Content.Where((t, i) => !comparer.Equals(t, otherContainer.Content[i])).Any();
    }
    
    public override int GetHashCode() => Content.Aggregate(
        17, (current, item) => current * 31 + HashCodes.Combine(item)
    );
    
    public int GetHashCode(IEqualityComparer comparer) => Content.Aggregate(
        17, (current, item) => current * 31 + comparer.GetHashCode(item)
    );
    
}