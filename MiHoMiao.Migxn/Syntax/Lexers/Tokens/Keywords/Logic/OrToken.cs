using MiHoMiao.Migxn.CodeAnalysis.Parser;
using MiHoMiao.Migxn.Runtime;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Algorithm;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Data.Load;
using MiHoMiao.Migxn.Syntax.Parser.Intermediate.Flow;

namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords.Logic;

internal record OrToken(int Index, (int Line, int Column) Position)
    : AbstractKeyword(UniqueName.AsMemory(), Index, Position), IKeywordToken, IBinaryToken
{
    public static string UniqueName => "or";

    public static AbstractKeyword Create(int index, (int Line, int Column) position) => new OrToken(index, position);

    public override IEnumerable<MigxnOpCode> AsOpCodes(MigxnContext context) => throw new NotSupportedException();

    public Type BinaryType(MigxnExpr left, MigxnExpr right, MigxnContext context)
    {
        Type typeLeft = left.ExprType(context);
        Type typeRight = right.ExprType(context);
        Type binaryType = typeof(bool);
        if (typeLeft == binaryType && typeRight == binaryType) return binaryType;
        ErrorTypeExpr exception = new ErrorTypeExpr(left.Children().First(), typeLeft, typeRight, UniqueName);
        context.MigxnParser.Exceptions.Add(exception);
        return typeof(void);
    }
    
    public IEnumerable<MigxnOpCode> BinaryOp(MigxnExpr left, MigxnExpr right, MigxnContext context)
    {
        if (right is TokenExpr rightToken) return [..left.AsOpCodes(context), ..rightToken.Token.AsOpCodes(context), new OpOr()];
        ReadOnlyMemory<char> labelLeft = $"<label>.or_left_{Position}".AsMemory();
        ReadOnlyMemory<char> labelRight = $"<label>.or_right_{Position}".AsMemory();
        return
        [
            ..left.AsOpCodes(context), new OpBrTrue(labelLeft),
            ..right.AsOpCodes(context), new OpGoto(labelRight),
            new OpLabel(labelLeft), new OpLdcI4S(1), new OpLabel(labelRight), 
        ];
    }

    int IBinaryToken.Priority => 13;
    
    MigxnNode ILeaderOpToken.MigxnNode => this;

}