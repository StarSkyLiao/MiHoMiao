using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Syntax.Grammars;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

internal record VarToken(int Index, (int Line, int Column) Position)
    : AbstractKeyword(UniqueName.AsMemory(), Index, Position), IKeywordToken//, ILeadToken
{
    public static string UniqueName => "var";

    public static AbstractKeyword Create(int index, (int Line, int Column) position) => new VarToken(index, position);

    // Result<MigxnTree> ILeadToken.TryCollectToken(ReadOnlySpan<MigxnToken> tokens, out int movedStep)
    // {
    //     
    //     
    //     
    //     
    // }
    
}