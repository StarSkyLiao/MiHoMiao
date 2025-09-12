parser grammar MigxnExpr;

@header {
namespace MiHoMiao.Migxn.Antlr.Auto;
}

options {
    // 表示解析 token 的词法解析器使用 MigxnLexer
    tokenVocab = MigxnLexer;
}
    
expression
    : OpenParens expression CloseParens                             #ParenthesesExpr
    
    | lambdaExpr                                                    #LambdaExpr

    | tupleListExpr                                                 #TupleExpr

    | expression '[' expression (',' expression)* ']'               #IndexExpr
    
    | expression '.' Identifier                                     #FieldExpr
    
    | expression paramPassExpr                                      #CallExpr
    
    | expression '|>' expression                                    #CallExpr
    
    | Left = expression op = (Mul | Div | Rem) Right = expression   #BinaryExpr
      
    | <assoc=right> Left = expression op = Pow Right = expression   #BinaryExpr
      
    | Left = expression op = (Add | Sub) Right = expression         #BinaryExpr

    | Left = expression op = (Cgt | Cge | Clt | Cle) Right = expression   #BinaryExpr
      
    | Left = expression op = (Ceq | Cneq) Right = expression        #BinaryExpr
             
    | Left = expression 'and' Right = expression                    #AndExpr    

    | Left = expression 'or' Right = expression                     #OrExpr
    
    | Left = expression '??' Right = expression                     #NullTestExpr      
    
    | Left = expression '?' Then = expression ':' Else = expression       #NullTestExpr

    | (integer | float | string | namespace_or_typeName)            #SingleExpr
    ;

tupleListExpr: '{' expression (',' expression)+ '}';

paramPassExpr: '(' (expression (',' expression)*)? ')';

lambdaExpr
    : paramDefinition (Colon ReturnType = fullType)? Arrow (expression | statement)
    ;
    
integer
    : IntegerLiteral       #DecIntLiteral
    | HexIntegerLiteral    #HexIntLiteral
    | BinIntegerLiteral    #BinIntLiteral
    | CharLiteral          #CharLiteral
    ;

float
    : FloatNumberLiteal    #FloatLiteral
    | ExponentFloatLiteal  #ExponentLiteral
    ;

string
    : StringLiteral        #NormalStringLiteral
    | VerbatimString       #VerbatimStringLiteral
    ;

namespace_or_typeName
    : (Identifier type_argument_list?) ('.' Identifier type_argument_list?)*
    ;

type_argument_list
    : '<' fullType (',' fullType)* '>'
    ;
    
fullType : baseType ('?' | '['']' | '*')*;

baseType
    : namespace_or_typeName                                               #NamedType
    | '(' tupleElement (',' tupleElement)+ ')'                            #TupleType
    | Keyword = (Bool | Char | I32 | I64 | R32 | R64 | String | Any)      #KeywordType
    ;

tupleElement: fullType Identifier?;

paramDefinition: '(' (paramElement (',' paramElement)*)? ')';

paramElement: variableType Identifier ':' fullType;

variableType: (Val | Var | Ref);