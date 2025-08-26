using System.Diagnostics.CodeAnalysis;

namespace MiHoMiao.Migxin.Syntax.Lexical;

public abstract record MigxinToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : MigxinNode(Text, Index, Position)
{
    public override int NextColumn => Position.Column + Text.Length;

    [field: AllowNull, MaybeNull]
    internal static List<Func<MigxinLexer, MigxinToken?>> TokenParsers => field ??=
    [
        ..
        from type in typeof(MigxinToken).Assembly.GetTypes()
        where type.IsAssignableTo(typeof(ITokenMatcher)) && !type.IsAbstract
        select type.GetMethod(nameof(ITokenMatcher.TryMatch)).CreateDelegate<Func<MigxinLexer, MigxinToken?>>()
    ];
}