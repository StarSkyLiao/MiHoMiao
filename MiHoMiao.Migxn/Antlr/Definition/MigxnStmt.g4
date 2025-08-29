parser grammar MigxnStmt;

import MigxnExpr;

options {
    // 表示解析 token 的词法解析器使用 MigxnLiteral
    tokenVocab = MigxnLiteral;
}

statement
    : LCurly Children = statement* RCurly                                                              #BlockStmt
    | Var VarName = (Name | RawName)                               Assign Expression = expression      #VarStmt
    | Var VarName = (Name | RawName) Colon Type = (Name | RawName)                                     #VarStmt
    | Var VarName = (Name | RawName) Colon Type = (Name | RawName) Assign Expression = expression      #VarStmt
    | Val VarName = (Name | RawName)                               Assign Expression = expression      #ValStmt
    | Val VarName = (Name | RawName) Colon Type = (Name | RawName) Assign Expression = expression      #ValStmt
    | expression                                                                                       #ExprStmt
    ;