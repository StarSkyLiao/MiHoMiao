using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Intermediate;
using MiHoMiao.Migxn.Syntax.Intermediate.Algorithm;
using MiHoMiao.Migxn.Syntax.Intermediate.Data.Load;
using MiHoMiao.Migxn.Syntax.Intermediate.Flow;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords.Logic;

internal record AndToken(int Index, (int Line, int Column) Position)
    : AbstractKeyword(UniqueName.AsMemory(), Index, Position), IKeywordToken, IBinaryToken
{
    public static string UniqueName => "and";

    public static AbstractKeyword Create(int index, (int Line, int Column) position) => new AndToken(index, position);

    public override IEnumerable<MigxnOpCode> AsOpCodes() => throw new NotSupportedException();
    
    public IEnumerable<MigxnOpCode> BinaryOp(MigxnExpr left, MigxnExpr right)
    {
        if (right is TokenExpr rightToken) return [..left.AsOpCodes(), new OpAnd(), ..rightToken.Token.AsOpCodes()];
        ReadOnlyMemory<char> labelLeft = $"<label>.or_left_{Position}".AsMemory();
        ReadOnlyMemory<char> labelRight = $"<label>.or_right_{Position}".AsMemory();
        return
        [
            ..left.AsOpCodes(), new OpBrFalse(labelLeft),
            ..right.AsOpCodes(), new OpGoto(labelRight),
            new OpLabel(labelLeft), new OpLdcI4S(0), new OpLabel(labelRight), 
        ];
    }
    
    int IBinaryToken.Priority => 12;
    
    MigxnNode ILeaderOpToken.MigxnNode => this;

}