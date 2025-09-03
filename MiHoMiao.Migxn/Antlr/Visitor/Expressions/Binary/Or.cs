using Antlr4.Runtime;
using MiHoMiao.Migxn.CodeGen.Algorithm;
using MiHoMiao.Migxn.CodeGen.Cast;
using MiHoMiao.Migxn.CodeGen.Data.Load;
using MiHoMiao.Migxn.CodeGen.Flow;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    private Type EmitOr(Type leftType, Type rightType, int leftTail, IToken token)
    {
        if (leftType != typeof(bool) || rightType != typeof(bool)) return typeof(void);
        if (MigxnContext.MigxnMember.Codes[^1] is OpLoad)
        {
            MigxnContext.EmitCode(new OpOr());
        }
        else
        {
            string labelLeft = $"<label>.or_left.{(token.Line, token.Column)}";
            string labelRight = $"<label>.or_right.{(token.Line, token.Column)}";
            MigxnContext.InsertEmitCode(leftTail, new OpBrTrue(labelLeft));
            MigxnContext.EmitCode(new OpGoto(labelRight));
            
            MigxnContext.EmitCode(new OpLabel(labelLeft));
            MigxnContext.EmitCode(new OpLdcLong(1));
            MigxnContext.EmitCode(new OpLabel(labelRight));
        }
            
        return typeof(bool);
    }
}