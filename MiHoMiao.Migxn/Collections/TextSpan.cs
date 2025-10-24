using MiHoMiao.Core.Numerics.Hash;

namespace MiHoMiao.Migxn.Collections;

public readonly struct TextSpan : IEquatable<TextSpan>
{
    public TextSpan(int start, int length)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(start);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(start, start + length);
        Start = start;
        Length = length;
    }

    public int Start { get; }
    
    public int End => Start + Length;
    
    public int Length { get; }
    
    public bool IsEmpty => Length == 0;
    
    public bool Contains(int position) => (uint)(position - Start) < (uint)Length;
    
    public bool Contains(TextSpan span) => span.Start >= Start && span.End <= End;
    
    public static TextSpan FromBounds(int start, int end) => new TextSpan(start, end - start);
    
    public TextSpan this[Range range]
    {
        get
        {
            int start = range.Start.IsFromEnd ? End - range.Start.Value : Start + range.Start.Value;
            int end   = range.End.IsFromEnd   ? End - range.End.Value   : Start + range.End.Value;

            // 允许 end == start，对应空子区间
            if ((uint)start > (uint)end || start < Start || end > End)
                throw new ArgumentOutOfRangeException(nameof(range));

            return new TextSpan(start, end - start);
        }
    }
    
    public static bool operator ==(TextSpan left, TextSpan right) => left.Equals(right);
    
    public static bool operator !=(TextSpan left, TextSpan right) => !left.Equals(right);
    
    public bool Equals(TextSpan other) => Start == other.Start && Length == other.Length;
    
    public override bool Equals(object? obj) => obj is TextSpan span && Equals(span);

    public override int GetHashCode() => HashCodes.Combine(Start, Length);
    
    public override string ToString() => $"[{Start}..{End})";
    
}