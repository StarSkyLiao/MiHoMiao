using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Algorithm;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Data.Load;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Flow;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords.Logic;

internal record AndToken(int Index, (int Line, int Column) Position)
    : AbstractKeyword(UniqueName.AsMemory(), Index, Position), IKeywordToken, IBinaryToken
{
    public static string UniqueName => "and";

    public static AbstractKeyword Create(int index, (int Line, int Column) position) => new AndToken(index, position);

    public override IEnumerable<MigxnOpCode> AsOpCodes(MigxnContext context) => throw new NotSupportedException();
    
    public IEnumerable<MigxnOpCode> BinaryOp(MigxnExpr left, MigxnExpr right, MigxnContext context)
    {
        if (right is TokenExpr rightToken) return [..left.AsOpCodes(context), ..rightToken.Token.AsOpCodes(context), new OpAnd()];
        ReadOnlyMemory<char> labelLeft = $"<label>.or_left_{Position}".AsMemory();
        ReadOnlyMemory<char> labelRight = $"<label>.or_right_{Position}".AsMemory();
        return
        [
            ..left.AsOpCodes(context), new OpBrFalse(labelLeft),
            ..right.AsOpCodes(context), new OpGoto(labelRight),
            new OpLabel(labelLeft), new OpLdcI4S(0), new OpLabel(labelRight), 
        ];
    }
    
    int IBinaryToken.Priority => 12;
    
    MigxnNode ILeaderOpToken.MigxnNode => this;

}