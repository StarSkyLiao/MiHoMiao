parser grammar MigxnStmt;

import MigxnExpr;

options {
    // 表示解析 token 的词法解析器使用 MigxnLexer
    tokenVocab = MigxnLexer;
}

statement
    : labeled_Statement
    | declarationStatement
    | embedded_statement
    ;

labeled_Statement: Label Identifier;

declarationStatement
    : local_variable_declaration
    ;

local_variable_declaration
    : Using? Var VarName = Identifier                          assignment Expression = expression     #VarStmt
    | Using? Var VarName = Identifier  Colon Type = fullType  (assignment Expression = expression)?   #VarStmt
    | Using? Val VarName = Identifier (Colon Type = fullType)? assignment Expression = expression     #ValStmt
    ;

assignment : AddAssign | SubAssign | MulAssign | DivAssign | RemAssign | AndAssign | OrAssign | XorAssign;

embedded_statement
    : block
    | simple_embedded_statement
    ;

block
    : '{' statement* '}'
    ;

simple_embedded_statement
    : expression          # expressionStatement

    // selection statements
    | If '(' expression ')' embedded_statement (Else embedded_statement)?    #IfStatement
    | When '(' expression ')' '{' when_case* '}'                             #WhenStatement

    // iteration statements
    | While '(' expression ')' embedded_statement                            #WhileStatement
    | Do embedded_statement While '(' expression ')'                         #DoStatement
    | Loop '(' expression ')' embedded_statement                             #LoopStatement
    
    // jump statements
    | Break                                                                  # BreakStatement
    | Pass                                                                   # ContinueStatement
    | Goto Identifier                                                        # GotoStatement
    | Return expression                                                      # ReturnStatement
    | Return                                                                 # ReturnEmptyStatement
    | Throw expression?                                                      # ThrowStatement
    | Yield (Return expression | Break)                                      # YieldStatement

    // unsafe statements Unsupport
    ;

when_case: expression Arrow embedded_statement;