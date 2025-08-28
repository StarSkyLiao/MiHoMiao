using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace MiHoMiao.Migxin.Compiler.Lexical;

public abstract record MigxinToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : MigxinNode(Text, Index, Position)
{
    public override int NextColumn => Position.Column + Text.Length;

    [field: AllowNull, MaybeNull]
    private static List<Type> TokenTypes => field ??=
    [
        ..
        from type in typeof(MigxinToken).Assembly.GetTypes()
        where type.IsAssignableTo(typeof(ITokenMatcher)) && !type.IsAbstract
        select type
    ];    
    
    [field: AllowNull, MaybeNull]
    private static SortedList<ulong, Func<MigxinLexer, MigxinToken?>> TokenParsers
    {
        get
        {
            if (field != null) return field;

            field = [];
            foreach (Type type in TokenTypes)
            {
                object? priority = type.GetProperty(nameof(ITokenMatcher.Priority))?.GetValue(null);
                if (priority is null) continue;
                var func = type.GetMethod(nameof(ITokenMatcher.TryMatch))?.CreateDelegate<Func<MigxinLexer, MigxinToken?>>();
                Debug.Assert(func != null);
                field.Add(((ulong)(uint)priority << 32) + (uint)type.GetHashCode(), func);
            }

            return field;
        }
    }

    [field: AllowNull, MaybeNull]
    internal static Func<MigxinLexer, MigxinToken?>[] TokenLists => field ??= TokenParsers.Values.Reverse().ToArray();
    
}