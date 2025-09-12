parser grammar MigxnParser;

@header {
namespace MiHoMiao.Migxn.Antlr.Auto;
}

options {
    // 表示解析 token 的词法解析器使用 MigxnLexer
    tokenVocab = MigxnLexer;
}

// ============================================================Type============================================================//
typeDefinition : accessAbility typeDeinition? TypeName = fullType                // 类型定义
                 (':' BaseType = namespace_or_typeName)?                         // 基类型
                 (With featureList)?                                             // 实现的特性
                 memberDeclaration*                                              // 成员
               ;
typeDeinition: ValType | Secured | Virtual | Concept | Toolset | Feature;
featureList : namespace_or_typeName (',' namespace_or_typeName)*;
// ------------------------------------------------------------Type------------------------------------------------------------//

// ============================================================Member============================================================//
memberDeclaration : memberAttribute? accessAbility memberKeyword MemberName = Identifier memberBody;
memberBody : ':' fullType ('=' expression)?                                   #FieldMember
           | ':' fullType '->' (statement | expression)                       #GetOrSetMember
           | ':' fullType '{' accessAbility? 'set''}' ('=' expression)?   #PropertyMember
           | lambdaBody                                                       #MethodMember
           ;
accessAbility : Public | Global | Asmbly | Family | Intern | Native;
memberKeyword : Val | Var | Fun | Get | Set | Ref | Let;
// ------------------------------------------------------------Member------------------------------------------------------------//

// ============================================================Lambda============================================================//
lambdaBody : '(' paramList? ')' (':' ReturnType = fullType)? '->' (statement | expression);
paramList : param (',' param)*;
param : (Val | Var)? ParamName = Identifier ':' ParamType = fullType;
// ------------------------------------------------------------Member------------------------------------------------------------//

// ============================================================Attribute============================================================//
memberAttribute : '[' attributeParam (',' attributeParam)* ','? ']';
attributeParam : namespace_or_typeName ('(' (attribute (',' attribute)*)? ')')?;
attribute : (Identifier ':')? expression;
// ------------------------------------------------------------Attribute------------------------------------------------------------//

// ============================================================Statement============================================================//
statement : tuple '|>' expression #CallStatement
          | expression arguments  #CallStatement
          | '{' statement* '}'    #BlockStmt
          | Label Identifier      #LabelStmt
          | simpleStatement       #SingleStmt
          | declaration           #DeclStmt
          | assignment            #AssignStmt
          ;
declaration : Using? Var VarName = Identifier                          assignment Expression = expression     #VarStmt
            | Using? Var VarName = Identifier  Colon Type = fullType  (assignment Expression = expression)?   #VarStmt
            | Using? Val VarName = Identifier (Colon Type = fullType)? assignment Expression = expression     #ValStmt
            | Using? Get VarName = Identifier (Colon Type = fullType)?    Arrow   Expression = expression     #GetStmt
            ;
assignment : (tuple | VarName = Identifier) (Colon Type = fullType)? assignOp Expression = expression;
assignOp : Assign | AddAssign | SubAssign | MulAssign | DivAssign | RemAssign | AndAssign | OrAssign | XorAssign;
simpleStatement : If '(' expression ')' statement ('else' statement)?            #IfStatement
                | When '(' expression ')' '{' when_case* '}'                     #WhenStatement

                | While '(' expression ')' statement                             #WhileStatement
                | Do statement While '(' expression ')'                          #DoStatement
                | Loop '(' expression ')' statement                              #LoopStatement

                | Break                                                          #BreakStatement
                | Pass                                                           #ContinueStatement
                | Goto Identifier                                                #GotoStatement
                | Return expression                                              #ReturnStatement
                | Return                                                         #ReturnEmptyStatement
                | Throw expression?                                              #ThrowStatement
                | Yield (Return expression | Break)                              #YieldStatement
                ;
when_case: expression Arrow statement;
// ------------------------------------------------------------Statement------------------------------------------------------------//

// ============================================================Expression============================================================//
expression : OpenParens expression CloseParens                                        #ParenthesesExpr
           | expression '[' expression (',' expression)* ']'                          #IndexExpr
           | expression op = ( '?.' |'.' ) Identifier                                 #FieldExpr
           | expression arguments                                                     #CallExpr
           | tuple '|>' expression                                                    #CallExpr
           | Left = expression op = (Mul | Div | Rem) Right = expression              #BinaryExpr
           | <assoc=right> Left = expression op = Pow Right = expression              #BinaryExpr
           | Left = expression op = (Add | Sub) Right = expression                    #BinaryExpr
           | Left = expression op = (Cgt | Cge | Clt | Cle) Right = expression        #BinaryExpr
           | Left = expression op = (Ceq | Cneq) Right = expression                   #BinaryExpr      
           | Left = expression 'and' Right = expression                               #AndExpr    
           | Left = expression 'or' Right = expression                                #OrExpr
           | Left = expression '??' Right = expression                                #NullTestExpr      
           | Left = expression '?' Then = expression ':' Else = expression            #NullTestExpr
           | tuple                                                                    #TupleExpr
           | (integer | float | string | namespace_or_typeName | lambdaBody)          #SingleExpr
           ;
arguments : '(' (expression (',' expression)*)? ')';
tuple : '{' (expression (',' expression)*)? '}';
integer : IntegerLiteral | HexIntegerLiteral | BinIntegerLiteral | CharLiteral;
float : FloatNumberLiteal | ExponentFloatLiteal;
string : StringLiteral | VerbatimString;
// ------------------------------------------------------------Expression------------------------------------------------------------//

// ============================================================NameExpr============================================================//
namespace_or_typeName : genericName ('.' genericName)*;
genericName : Identifier genericList?;
genericList : '<' fullType (',' fullType)* '>';
fullType : baseType ('?' | '['']' | '*')*;
baseType : namespace_or_typeName                                               #NamedType
         | Keyword = (Bool | Char | I32 | I64 | R32 | R64 | String | Any)      #KeywordType
         ;
// ------------------------------------------------------------NameExpr------------------------------------------------------------//