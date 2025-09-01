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
    | If LRound Condition = expression RRound TrueBody = statement                                     #IfStmt
    | If LRound Condition = expression RRound TrueBody = statement Else FalseBody = statement          #IfElseStmt
    | While LRound Condition = expression RRound WhileBody = statement                                 #WhileStmt
    | Loop LRound LoopTimes = expression RRound LoopBody = statement                                  #LoopStmt
    | expression                                                                                       #ExprStmt
    ;