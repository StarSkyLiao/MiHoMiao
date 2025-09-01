parser grammar MigxnStmt;

import MigxnExpr;

options {
    // 表示解析 token 的词法解析器使用 MigxnLiteral
    tokenVocab = MigxnLiteral;
}

statement
    : LCurly Children = statement* RCurly                                                              #BlockStmt
    | Var VarName = Name                               Assign Expression = expression                  #VarStmt
    | Var VarName = Name Colon Type = Name                                                             #VarStmt
    | Var VarName = Name Colon Type = Name Assign Expression = expression                              #VarStmt
    | Val VarName = Name                               Assign Expression = expression                  #ValStmt
    | Val VarName = Name Colon Type = Name Assign Expression = expression                              #ValStmt
    | If LRound Condition = expression RRound TrueBody = statement                                     #IfStmt
    | If LRound Condition = expression RRound TrueBody = statement Else FalseBody = statement          #IfElseStmt
    | While LRound Condition = expression RRound WhileBody = statement                                 #WhileStmt
    | Loop LRound LoopTimes = expression RRound LoopBody = statement                                   #LoopStmt
    | Return Result = expression?                                                                      #ReturnStmt
    | <assoc=right> Left = expression op = (Assign | Assign) Right = expression                        #AssignStmt
    ;