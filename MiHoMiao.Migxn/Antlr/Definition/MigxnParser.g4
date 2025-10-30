parser grammar MigxnParser;

@header {
namespace MiHoMiao.Migxn.Antlr.Generated;
}

options {
    // 表示解析 token 的词法解析器使用 MigxnLiteral
    tokenVocab = MigxnLexer;
}

root : simpleStatement;

// ============================================================Type============================================================//
typeDefinition : accessAbility typeKeywords? TypeName = genericName              // 类型定义
                 ('in' Namespace = namespace)?                                   // 命名空间
                 (From BaseType = fullType)?                                     // 基类型
                 (With featureList)?                                             // 实现的特性
                 memberDeclaration*                                              // 成员
               ;
typeKeywords: ValType | Secured | Virtual | Concept | Toolset | Feature;
featureList : fullType (',' fullType)*;
// ------------------------------------------------------------Type------------------------------------------------------------//

// ============================================================Member============================================================//
memberDeclaration : memberAttribute? accessAbility memberKeyword MemberName = Identifier memberBody;
memberBody : ':' fullType ('=' expression)?                                   #FieldMember
           | ':' fullType '->' (statement | expression)                       #GetOrSetMember
           | ':' fullType '{' accessAbility? 'set''}' ('=' expression)?       #PropertyMember
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
memberAttribute : As (Identifier | Virtual | Concept | Secured)+ ':';
// ------------------------------------------------------------Attribute------------------------------------------------------------//

// ============================================================Statement============================================================//
statement : tuple '|>' expression #PipStatement
          | expression arguments  #CallStatement
          | '{' statement* '}'    #BlockStmt
          
          | simpleStatement       #SingleStmt
          | declaration           #DeclStmt
          | assignment            #AssignStmt
          | Label Identifier      #LabelStmt
          ;
declaration : Using? Var VarName = Identifier                          assignOp Expression = expression     #VarStmt
            | Using? Var VarName = Identifier  Colon Type = fullType  (assignOp Expression = expression)?   #VarStmt
            | Using? Val VarName = Identifier (Colon Type = fullType)? assignOp Expression = expression     #ValStmt
            | Using? Get VarName = Identifier (Colon Type = fullType)?  Arrow   Expression = expression     #GetStmt
            ;
assignment  : (tuple | VarName = Identifier)  (Colon Type = fullType)?  assignOp  Expression = expression;
assignOp : Assign | AddAssign | SubAssign | MulAssign | DivAssign | RemAssign | AndAssign | OrAssign | XorAssign;
simpleStatement : If '(' expression ')' statement ('else' statement)?            #IfStatement
                | When '(' expression ')' whenStmtCase* End                      #WhenStatement

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
whenStmtCase: '::' basePattern (Arrow statement)?;
// ------------------------------------------------------------Statement------------------------------------------------------------//

// ============================================================Expression============================================================//
expression : OpenParens expression CloseParens                                        #ParenthesesExpr
           | expression '[' expression (',' expression)* ']'                          #IndexExpr
           | expression op = ( '?.' |'.' ) Identifier                                 #FieldExpr
           | Identifier '::' Identifier                                               #MetaExpr
           | expression When whenExprCase* End                                        #WhenExpr
           | Not expression                                                           #NotExpr
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
           | Left = expression '?' Then = expression ':' Else = expression            #ConditionalExpr
           | tuple                                                                    #TupleExpr
           | (number | string | genericName | lambdaBody)                             #SingleExpr
           ;
whenExprCase: '::' basePattern (Arrow expression)?;
arguments : '(' (expression (',' expression)*)? ')';
tuple : '{' (expression (',' expression)*)? '}';
number : integer | float;
integer : IntegerLiteral | HexIntegerLiteral | BinIntegerLiteral | CharLiteral;
float : FloatNumberLiteal | ExponentFloatLiteal;
string : StringLiteral | VerbatimString;
// ------------------------------------------------------------Expression------------------------------------------------------------//

// ============================================================Pattern============================================================//
basePattern : fullType
            | '(' basePattern ')'
            | 'not' basePattern
            | basePattern 'and' basePattern
            | basePattern 'or' basePattern
            | basePattern 'xor' basePattern
            | number
            | string
            | In range
            | op = (Cgt | Cge | Clt | Cle) basePattern
            ;
range: ('(' Infinity | ('[' | '(')  expression) ',' (Infinity ')' | expression (']' | ')')) ;
// ------------------------------------------------------------Pattern------------------------------------------------------------//


// ============================================================NameExpr============================================================//
//namespace_or_typeName : genericName ('.' genericName)*;
namespace : Identifier ('.' Identifier)*;
fullType : baseType ('?' | '['']' | '*')*;
genericName : Identifier ('<' Generic = fullType (',' Generic = fullType)* '>')?;
baseType : Keyword = (Bool | Char | I32 | I64 | R32 | R64 | String | Object)   #KeywordType
         | genericName                                                         #NamedType
         ;
// ------------------------------------------------------------NameExpr------------------------------------------------------------//
