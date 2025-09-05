parser grammar MigxnParser;

@header {
namespace MiHoMiao.Migxn.Antlr.Generated;
}

options {
    // 表示解析 token 的词法解析器使用 MigxnLiteral
    tokenVocab = MigxnLexer;
}

compilationUnit
    : importDirectives memberDeclarations+
    ;

namespace_or_typeName
    : (Identifier type_argument_list?) ('.' Identifier type_argument_list?)*
    ;
    
type_argument_list
    : '<' fullType (',' fullType)* '>'
    ;
    
fullType : baseType ('?' | arraySuffix | '*')*;

baseType
    : namespace_or_typeName                                               #NamedType
    | '(' tupleElement (',' tupleElement)+ ')'                            #TupleType
    | Keyword = (Bool | Char | I32 | I64 | R32 | R64 | String | Any)      #KeywordType
    ;

tupleElement: fullType Identifier?;

arraySuffix: '[' ','* ']';

attributes: '[' attribute_list (',' attribute_list)* ','? ']';
    
attribute_list: attribute (',' attribute)*;
    
attribute: namespace_or_typeName (OpenParens (attribute_argument (',' attribute_argument)*)? CloseParens)?;

attribute_argument: (Identifier ':')? expression;

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
      op = (Cgt | Cge | Clt | Cle)
      Right = expression                                   #BinaryExpr
      
    | Left = expression
          op = (Eql | Ueql)
      Right = expression                                   #BinaryExpr
             
    | Left = expression
          op = (And | Or)
      Right = expression                                   #AndOrExpr      
     
    | Value = (Integer | Float | String | Char | Name)     #SingleExpr
    ;

assignment_operator : AddAssign | SubAssign | MulAssign | DivAssign | RemAssign | AndAssign | OrAssign | XorAssign;

importDirectives: importDirective+;
    
importDirective
    : Import namespace_or_typeName ';'
    | Import Identifier From namespace_or_typeName ';'
    ;

memberDeclarations
    : (Export namespace_or_typeName)?                                 // 命名空间
      accessAbilityToken typeDeinition? Identifier                    // 类型定义
      (':' namespace_or_typeName)?                                    // 基类型
      (With namespace_or_typeName (',' namespace_or_typeName)*)?      // 实现的特性
      memberDeclaration+                                              // 成员
    ;

accessAbilityToken: Public | Global | Asmbly | Family | Intern | Native;

typeDeinition: ValType | Secured | Virtual | Concept | Toolset | Feature;

memberDeclaration
    : fieldMember
    ;

fieldMember: attributes? accessAbilityToken Type = (Val | Var) Identifier ':' namespace_or_typeName ('=' expression);