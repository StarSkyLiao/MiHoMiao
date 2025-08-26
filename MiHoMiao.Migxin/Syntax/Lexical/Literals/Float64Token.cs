using System.Globalization;

namespace MiHoMiao.Migxin.Syntax.Lexical.Literals;

internal record Float64Token(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : LiteralToken(Text, Index, Position), ITokenMatcher
{
    public required double Value { get; init; }
    public static HashSet<char> StartChars => [..".0123456789"];
    public static uint Priority => 2;
    public static MigxinToken? TryMatch(MigxinLexer migxinLexer)
    {
        if (!StartChars.Contains(migxinLexer.Current)) return null;
        (int start, (int Line, int Column) position) = migxinLexer.CreateFrame();
    
        // Parse integer part
        while (migxinLexer.Current is >= '0' and <= '9') migxinLexer.MoveNext();
    
        // Check for decimal point and fractional part
        bool isFloat = false;
        if (migxinLexer.Current == '.')
        {
            isFloat = true;
            migxinLexer.MoveNext();
            while (migxinLexer.Current is >= '0' and <= '9') migxinLexer.MoveNext();
        }
    
        // Check for exponent
        if (migxinLexer.Current is 'e' or 'E')
        {
            isFloat = true;
            migxinLexer.MoveNext();
            if (migxinLexer.Current is '+' or '-') migxinLexer.MoveNext();
            while (migxinLexer.Current is >= '0' and <= '9') migxinLexer.MoveNext();
        }
        
        ReadOnlyMemory<char> read = migxinLexer.AsMemory(start, migxinLexer.CharIndex);
        if (char.IsLetter(migxinLexer.Current))
        {
            if (migxinLexer.Current is 'd' or 'D')
            {
                isFloat = true;
                migxinLexer.MoveNext();
            }
            else
            {
                migxinLexer.RestoreFrame();
                return null;
            }
        }

        if (!isFloat)
        {
            migxinLexer.RestoreFrame();
            return null;
        }
        bool success = double.TryParse(read.Span, NumberStyles.Float, CultureInfo.InvariantCulture, out double result);
        if (success) return new Float64Token(result.ToString(CultureInfo.CurrentCulture).AsMemory(), start, position) { Value = result };
        migxinLexer.RestoreFrame();
        return null;
    }
}