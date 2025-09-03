using Antlr4.Runtime;
using MiHoMiao.Migxn.CodeGen.Algorithm;
using MiHoMiao.Migxn.CodeGen.Cast;
using MiHoMiao.Migxn.CodeGen.Data.Load;
using MiHoMiao.Migxn.CodeGen.Flow;

namespace MiHoMiao.Migxn.Antlr.Visitor;

internal partial class MigxnMethodParser
{
    private Type EmitAnd(Type leftType, Type rightType, int leftTail, IToken token)
    {
        if (leftType != typeof(bool) || rightType != typeof(bool)) return typeof(void);
        if (MigxnContext.MigxnMember.Codes[^1] is OpLoad)
        {
            MigxnContext.EmitCode(new OpAnd());
        }
        else
        {
            string labelLeft = $"<label>.and_left.{(token.Line, token.Column)}";
            string labelRight = $"<label>.and_right.{(token.Line, token.Column)}";
            MigxnContext.InsertEmitCode(leftTail, new OpBrFalse(labelLeft));
            MigxnContext.EmitCode(new OpGoto(labelRight));
            
            MigxnContext.EmitCode(new OpLabel(labelLeft));
            MigxnContext.EmitCode(new OpLdcLong(0));
            MigxnContext.EmitCode(new OpLabel(labelRight));
        }
            
        return typeof(bool);
    }
}