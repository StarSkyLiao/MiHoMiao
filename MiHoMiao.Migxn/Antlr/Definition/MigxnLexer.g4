lexer grammar MigxnLexer;

@header {
namespace MiHoMiao.Migxn.Antlr.Generated;
}

SingleLintComment           : '#' InputCharacter*      -> channel(HIDDEN);
MultiLineComment            : '###' .*? '###'          -> channel(HIDDEN);
WhiteSpace                  : (Whitespace | NewLine)+  -> channel(HIDDEN);

Import:    'import';
Export:    'export';
From:      'from';
With:      'with';

Public:    'public';
Global:    'global';
Asmbly:    'asmbly';
Family:    'family';
Intern:    'intern';
Native:    'native';

ValType:   'valtype';
Secured:   'secured';
Virtual:   'virtual';
Concept:   'concept';
Toolset:   'toolset';
Feature:   'feature';

Var:       'var';
Val:       'val';
Let:       'let';
Fun:       'fun';
Get:       'get';
Set:       'set';

If:        'if';
Else:      'else';

And:       'and';
Or:        'or';
Not:       'not';
Is:        'is';
As:        'as';

Goto:      'goto';
Label:     'label::';
Loop:      'loop';
While:     'while';
Pass:      'pass';
Break:     'break';
Return:    'ret';

Bool:      'bool';
Char:      'char';
I32:       'i32';
I64:       'i64';
R32:       'r32';
R64:       'r64';
String:    'string';
Any:       'any';

Identifier: '@'? IdentifierOrKeyword;

// Literals
// 0.Equals() would be parsed as an invalid real (1. branch) causing a lexer error
LITERAL_ACCESS      : [0-9] ('_'* [0-9])* 'L'? '.' '@'? IdentifierOrKeyword;
IntegerLiteral      : [0-9] ('_'* [0-9])* 'L'?;
HexIntegerLiteral   : '0' [xX] ('_'* HexDigit)+ 'L'?;
BinIntegerLiteral   : '0' [bB] ('_'* [01])+ 'L'?;
RealNumberLiteal:
    ([0-9] ('_'* [0-9])*)? '.' [0-9] ('_'* [0-9])* ExponentPart? [FfDdMm]?
    | [0-9] ('_'* [0-9])* ([FfDdMm] | ExponentPart [FfDdMm]?)
;

CharLiteral       : '\'' (~['\\\r\n\u0085\u2028\u2029] | CommonCharacter) '\'';
StringLiteral     : '"' (~["\\\r\n\u0085\u2028\u2029] | CommonCharacter)* '"';
VerbatimString    : '@"' (~'"' | '""')* '"';

// Operators And Punctuators
OpenBrace       : '{';
CloseBrace      : '}';
OpenBracket     : '[';
CloseBracket    : ']';
OpenParens      : '(';
CloseParens     : ')';
Dot             : '.';
Comma           : ',';
Colon           : ':';
SemiColon       : ';';
Add             : '+';
Sub             : '-';
Mul             : '*';
Div             : '/';
Rem             : '%';
LogicAnd        : '&';
LogicOr         : '|';
LogicXor        : '^';
Bang            : '!';
Tilde           : '~';
Assign          : '=';
Interr          : '?';
DoubleColon     : '::';
Inc             : '++';
Dec             : '--';
Arrow           : '->';
Ceq             : '==';
Cneq            : '!=';
Cle             : '<=';
Cge             : '>=';
Clt             : '<';
Cgt             : '>';
AddAssign       : '+=';
SubAssign       : '-=';
MulAssign       : '*=';
DivAssign       : '/=';
RemAssign       : '%=';
AndAssign       : '&=';
OrAssign        : '|=';
XorAssign       : '^=';
Shl             : '<<';
Shr             : '>>';
NullAssign      : '??=';

// Fragments

fragment InputCharacter: ~[\r\n\u0085\u2028\u2029];

fragment ExponentPart      : [eE] ('+' | '-')? [0-9] ('_'* [0-9])*;

fragment CommonCharacter: SimpleEscapeSequence | HexEscapeSequence | UnicodeEscapeSequence;

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
    | '\\$'
;

fragment HexEscapeSequence:
    '\\x' HexDigit
    | '\\x' HexDigit HexDigit
    | '\\x' HexDigit HexDigit HexDigit
    | '\\x' HexDigit HexDigit HexDigit HexDigit
;

fragment NewLine:
    '\r\n'
    | '\r'
    | '\n'
    | '\u0085'   // <Next Line CHARACTER (U+0085)>'
    | '\u2028'   //'<Line Separator CHARACTER (U+2028)>'
    | '\u2029'   //'<Paragraph Separator CHARACTER (U+2029)>'
;

fragment Whitespace
    : '\u0020'   // SPACE
    | '\u0009'   //'<Horizontal Tab Character (U+0009)>'
    | '\u000B'   //'<Vertical Tab Character (U+000B)>'
    | '\u000C'   //'<Form Feed Character (U+000C)>'
;

fragment IdentifierOrKeyword: IdentifierStartCharacter IdentifierPartCharacter*;

fragment IdentifierStartCharacter: '_' | [\p{L}] | UnicodeEscapeSequence;

fragment IdentifierPartCharacter: IdentifierStartCharacter | [0-9];

fragment UnicodeEscapeSequence:
    '\\u' HexDigit HexDigit HexDigit HexDigit
    | '\\U' HexDigit HexDigit HexDigit HexDigit HexDigit HexDigit HexDigit HexDigit
;

fragment HexDigit: [0-9] | [A-F] | [a-f];
