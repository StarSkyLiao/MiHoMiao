using System.Diagnostics;
using MiHoMiao.Core.Diagnostics;
using MiHoMiao.Migxn.Syntax.Grammars;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Literals;

public record SymbolToken(ReadOnlyMemory<char> Text, int Index, (int Line, int Column) Position)
    : LiteralToken(Text, Index, Position), ILeadToken
{
    public Result<MigxnTree> TryCollectToken(ReadOnlySpan<MigxnToken> tokens, out int movedStep)
    {
        movedStep = 0;
        Debug.Assert(tokens[movedStep] is SymbolToken);
        // switch (tokens[++movedStep])
        // {
        //     case VarToken varToken:
        //     
        //     
        // }
        
        
        throw new NotImplementedException();
    }
}