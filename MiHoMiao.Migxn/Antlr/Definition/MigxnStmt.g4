parser grammar MigxnStmt;

import MigxnExpr;

options {
    // 表示解析 token 的词法解析器使用 MigxnLiteral
    tokenVocab = MigxnLiteral;
}

statement
    : Var name = (Name | RawName) Assign expression                                    #VarStmt
    | Var name = (Name | RawName) Colon type = (Name | RawName)                        #VarStmt
    | Var name = (Name | RawName) Colon type = (Name | RawName) Assign expression      #VarStmt
    | Val name = (Name | RawName) Assign expression                                    #ValStmt
    | Val name = (Name | RawName) Colon type = (Name | RawName) Assign expression      #ValStmt
    ;