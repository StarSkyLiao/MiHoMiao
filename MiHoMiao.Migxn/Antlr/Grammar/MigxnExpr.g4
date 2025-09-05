parser grammar MigxnExpr;

@header {
namespace MiHoMiao.Migxn.Antlr.Auto;
}

options {
    // 表示解析 token 的词法解析器使用 MigxnLexer
    tokenVocab = MigxnLexer;
}

expression
    : OpenParens expression CloseParens                    #ParenthesesExpr
    
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
          op = (Cgt | Cge | Clt | Cle)
      Right = expression                                   #BinaryExpr
      
    | Left = expression
          op = (Ceq | Cneq)
      Right = expression                                   #BinaryExpr
             
    | Left = expression
          op = (And | Or)
      Right = expression                                   #AndOrExpr      
     
    | tupleListExpr                                        #TupleExpr
     
    | (integer | float | string | namespace_or_typeName)   #SingleExpr
    ;

tupleListExpr: '(' expression (',' expression)+ ')';

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
