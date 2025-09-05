parser grammar MigxnParser;

import MigxnStmt;

options {
    // 表示解析 token 的词法解析器使用 MigxnLexer
    tokenVocab = MigxnLexer;
}

memberDeclarations
    : (Export namespace_or_typeName)?                                 // 命名空间
      accessAbilityToken typeDeinition? fullType                      // 类型定义
      (':' namespace_or_typeName)?                                    // 基类型
      (With namespace_or_typeName (',' namespace_or_typeName)*)?      // 实现的特性
      memberDeclaration+                                              // 成员
    ;

accessAbilityToken: Public | Global | Asmbly | Family | Intern | Native;

typeDeinition: ValType | Secured | Virtual | Concept | Toolset | Feature;

memberDeclaration
    : fieldMember
    ;

fieldMember: attributes? accessAbilityToken Type = (Val | Var) Identifier ':' fullType ('=' expression);

attributes: '[' attribute_list (',' attribute_list)* ','? ']';
    
attribute_list: attribute (',' attribute)*;
    
attribute: namespace_or_typeName (OpenParens (attribute_argument (',' attribute_argument)*)? CloseParens)?;

attribute_argument: (Identifier ':')? expression;