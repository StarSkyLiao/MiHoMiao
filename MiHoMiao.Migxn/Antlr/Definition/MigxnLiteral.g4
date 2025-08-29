lexer grammar MigxnLiteral;

@header {
namespace MiHoMiao.Migxn.Antlr.Generated;
}

// SKIP 当 Antlr 解析到下面的代码时，会选择跳过
Space: [ \t\r\n]+ -> channel(HIDDEN);

// 遇到 ### ### 会当作注释跳过
MultiLineComment: '###' .*? '###' -> channel(HIDDEN);

// 遇到 # 会当作注释跳过
SingleLineComment:  '#' ~[\r\n]* ('\r'? '\n' | EOF) -> channel(HIDDEN);

fragment Digit: [0-9]; 

Integer: Digit+;
Float: Digit* Dot Digit+;

Var:       'var';
Val:       'val';
Let:       'let';

RawName: '@'[\p{L}][\p{L}\p{N}]*;
Name: [\p{L}][\p{L}\p{N}]*;

Pow:       '**';

Dot:       '.';
Comma:     ',';
Colon:     ':';
SemiColon: ';';
LRound:    '(';
RRound:    ')';
LCurly:    '{';
RCurly:    '}';

Eql:       '==';
Ueql:      '!=';

Assign:    '=';
Add:       '+';
Sub:       '-';
Mul:       '*';
Div:       '/';
Rem:       '%';


LBRACKET: '[';
RBRACKET: ']';

GT: '>';
