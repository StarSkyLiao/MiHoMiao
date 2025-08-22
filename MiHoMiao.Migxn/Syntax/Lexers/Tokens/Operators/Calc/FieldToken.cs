// using MiHoMiao.Migxn.Runtime;
// using MiHoMiao.Migxn.Syntax.Grammars.Expressions;
// using MiHoMiao.Migxn.Syntax.Grammars.Expressions.Binary;
// using MiHoMiao.Migxn.Syntax.Lexers.Tokens.Keywords;
//
// namespace MiHoMiao.Migxn.Syntax.Lexers.Tokens.Operators.Calc;
//
// internal record FieldToken(int Index, (int Line, int Column) Position)
//     : MigxnOperator(UniqueName.AsMemory(), Index, Position), IOperatorToken, IClosedBinaryToken
// {
//     public static string UniqueName => ".";
//
//     public static MigxnOperator Create(int index, (int Line, int Column) position) => new FieldToken(index, position);
//
//     int IBinaryToken.Priority => 0;
//     
//     public Type BinaryType(MigxnExpr left, MigxnExpr right, MigxnContext context)
//     {
//         throw new NotImplementedException();
//     }
//
//     MigxnNode ILeaderOpToken.MigxnNode => this;
//
// }