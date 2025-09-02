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

Integer:   Digit+;
Float:     Digit* Dot Digit+;
Char :     '\'' (~['\\\r\n\u0085\u2028\u2029] | CommonCharacter) '\'';
String:    '"' (~["\\\r\n\u0085\u2028\u2029] | CommonCharacter)* '"';

Var:       'var';
Val:       'val';
Let:       'let';
Fun:       'fun';

If:        'if';
Else:      'else';

Loop:      'loop';
While:     'while';
Return:    'ret';

Name:      '@'? (UnicodeChar | '_') (UnicodeChar | UnicodeNumber)*;

Pow:       '**';

Dot:       '.';
Comma:     ',';
Colon:     ':';
SemiColon: ';';
LRound:    '(';
RRound:    ')';
LCurly:    '{';
RCurly:    '}';

Arrow:     '->';

Eql:       '==';
Ueql:      '!=';
Cgt:       '>';
Cge:       '>=';
Clt:       '<';
Cle:       '<=';

Assign:    '=';
Add:       '+';
Sub:       '-';
Mul:       '*';
Div:       '/';
Rem:       '%';

LBRACKET: '[';
RBRACKET: ']';

fragment Digit: [0-9];

fragment CommonCharacter: SimpleEscapeSequence | UnicodeChar;

fragment UnicodeChar: [\p{L}];

fragment UnicodeNumber: [\p{N}];

fragment SimpleEscapeSequence:
    '\\\''
    | '\\"'
    | '\\\\'
    | '\\0'
    | '\\a'
    | '\\b'
    | '\\f'
    | '\\n'
    | '\\r'
    | '\\t'
    | '\\v'
;
