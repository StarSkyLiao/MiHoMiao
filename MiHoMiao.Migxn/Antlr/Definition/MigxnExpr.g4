parser grammar MigxnExpr;

@header {
namespace MiHoMiao.Migxn.Antlr.Generated;
}

options {
    // 表示解析 token 的词法解析器使用 MigxnLiteral
    tokenVocab = MigxnLiteral;
}

expression
    : LRound expression RRound                             #ParenthesesExpr
    
    | Left = expression
      op = (Mul | Div | Rem)   
      Right = expression                                   #BinaryExpr
      
    | <assoc=right> Left = expression
      op = Pow
      Right = expression                                   #BinaryExpr
      
    | Left = expression
      op = (Add | Sub)
      Right = expression                                   #BinaryExpr

    | Left = expression
      op = (Eql | Ueql | Cgt | Cge | Clt | Cle)
      Right = expression                                   #CompareExpr
      
    | Value = (Integer | Float | String | Char | Name)     #SingleExpr
    ;
