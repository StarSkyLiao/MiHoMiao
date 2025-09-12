parser grammar MigxnStmt;

import MigxnExpr;

options {
    // 表示解析 token 的词法解析器使用 MigxnLexer
    tokenVocab = MigxnLexer;
}

statement
    : labeledStatement
    | declarationStatement
    | embeddedStatement
    ;

block
    : '{' statement* '}'
    ;

embeddedStatement
    : block
    | simpleStatement
    ;

labeledStatement: Label Identifier;

declarationStatement
    : variableDeclaration
    | variableAssignment
    | tupleAssignment
    ;

variableDeclaration
    : Using? Var VarName = Identifier                          assignment Expression = expression     #VarStmt
    | Using? Var VarName = Identifier  Colon Type = fullType  (assignment Expression = expression)?   #VarStmt
    | Using? Val VarName = Identifier (Colon Type = fullType)? assignment Expression = expression     #ValStmt
    | Using? Get VarName = Identifier (Colon Type = fullType)?    Arrow   Expression = expression     #GetStmt
    ;

variableAssignment: VarName = Identifier (Colon Type = fullType)? assignment Expression = expression;

tupleAssignment: tupleListExpr (Colon Type = fullType)? assignment Expression = expression;

assignment : Assign | AddAssign | SubAssign | MulAssign | DivAssign | RemAssign | AndAssign | OrAssign | XorAssign;

simpleStatement
    : If '(' expression ')' embeddedStatement ('else' embeddedStatement)?    #IfStatement
    | When '(' expression ')' '{' when_case* '}'                             #WhenStatement

    | While '(' expression ')' embeddedStatement                             #WhileStatement
    | Do embeddedStatement While '(' expression ')'                          #DoStatement
    | Loop '(' expression ')' embeddedStatement                              #LoopStatement

    | Break                                                                  #BreakStatement
    | Pass                                                                   #ContinueStatement
    | Goto Identifier                                                        #GotoStatement
    | Return expression                                                      #ReturnStatement
    | Return                                                                 #ReturnEmptyStatement
    | Throw expression?                                                      #ThrowStatement
    | Yield (Return expression | Break)                                      #YieldStatement

    | expression '|>' expression                                             #CallStatement
    | expression paramPassExpr                                               #CallStatement
    ;

when_case: expression Arrow embeddedStatement;
