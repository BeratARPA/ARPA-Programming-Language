grammar ARPA;

program: statement*;

statement: 
      declaration
    | assignment
    | expressionStatement
    | ifStatement
    | whileLoopStatement
    | forLoopStatement
    | printStatement
    | block
    | functionCall
    | returnStatement;

declaration: 
      variableDeclaration 
    | functionDeclaration;

variableDeclaration: 
      (SAYI | METIN | ONDALIK | MANTIK) ID (ATAMA expression)? NOKTALIVIRGUL;

functionDeclaration: 
      (SAYI | METIN | ONDALIK | MANTIK | BOS) ID SOLPARANTEZ (paramList)? SAGPARANTEZ block;

paramList: 
      ID (VIRGUL ID)*;

assignment: 
      ID ATAMA expression NOKTALIVIRGUL;

expressionStatement: 
      expression NOKTALIVIRGUL;

ifStatement: 
      EGER SOLPARANTEZ expression SAGPARANTEZ block (DEGILSEEGER SOLPARANTEZ expression SAGPARANTEZ block)? (DEGILSE block)?;

whileLoopStatement: 
      IKEN SOLPARANTEZ expression SAGPARANTEZ block;

forLoopStatement: 
      ICIN SOLPARANTEZ variableDeclaration? expression? NOKTALIVIRGUL assignment? SAGPARANTEZ block;

printStatement: 
      YAZDIR SOLPARANTEZ expression SAGPARANTEZ NOKTALIVIRGUL;

block: 
      SOLSUSLUPARANTEZ statement* SAGSUSLUPARANTEZ;

returnStatement: 
      DONDUR expression? NOKTALIVIRGUL;

expression: 
      expression VEYA expression
    | expression VE expression
    | expression (ESIT | ESITDEGIL | BUYUK | KUCUK | BUYUKESIT | KUCUKESIT) expression
    | expression (ARTI | EKSI) expression
    | expression (CARPIM | BOLU | MOD) expression
    | ID
    | NUMBER
    | STRING
    | TRUE
    | FALSE
    | functionCall
    | SOLPARANTEZ expression SAGPARANTEZ;

functionCall: 
      ID SOLPARANTEZ (argList)? SAGPARANTEZ;

argList: 
      expression (VIRGUL expression)*;

SOLPARANTEZ: '(';
SAGPARANTEZ: ')';
SOLSUSLUPARANTEZ: '{';
SAGSUSLUPARANTEZ: '}';

NOKTALIVIRGUL: ';';
VIRGUL: ',';

SAYI: 'sayi';
METIN: 'metin';
ONDALIK: 'ondalik';
MANTIK: 'mantik';
BOS: 'bos';
TRUE : 'dogru';
FALSE : 'yanlis';

ARTI: '+';
EKSI: '-';
CARPIM: '*';
BOLU: '/';
MOD: '%';

ATAMA: '=';
VE: 've';
VEYA: 'veya';
ESIT: '==';
ESITDEGIL: '!=';
BUYUK: '>';
KUCUK: '<';
BUYUKESIT: '>=';
KUCUKESIT: '<=';

EGER: 'eger';
DEGILSEEGER: 'degilseeger';
DEGILSE: 'degilse';
ICIN: 'icin';
IKEN: 'iken';
YAZDIR: 'yazdir';
DONDUR: 'dondur';

ID: [a-zA-Z_][a-zA-Z0-9_]*;
NUMBER: '-'? [0-9]+('.'[0-9]+)?;
STRING: '"' .*? '"';

WS: [ \t\r\n]+ -> skip;
COMMENT: '//' ~[\r\n]* -> skip;