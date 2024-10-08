grammar ARPA;

program: statement*;

statement: 
      declaration
    | assignment
    | expressionStatement
    | ifStatement
    | printStatement
    | block
    | functionCall
    | returnStatement; // Return (döndür) ifadesi eklendi

declaration: 
      variableDeclaration 
    | functionDeclaration;

variableDeclaration: 
      (SAYI | METİN | ONDALIK | MANTIK) ID (ATAMA expression)? NOKTALIVİRGÜL;

functionDeclaration: 
      (SAYI | METİN | ONDALIK | MANTIK | BOŞ) ID SOLPARANTEZ (paramList)? SAĞPARANTEZ block;

paramList: ID (VİRGÜL ID)*;

assignment: 
      ID ATAMA expression NOKTALIVİRGÜL;

expressionStatement: 
      expression NOKTALIVİRGÜL;

ifStatement: 
      EĞER SOLPARANTEZ expression SAĞPARANTEZ block (DEĞİLSEEĞER SOLPARANTEZ expression SAĞPARANTEZ block)? (DEĞİLSE block)?;

printStatement: 
      YAZDIR SOLPARANTEZ expression SAĞPARANTEZ NOKTALIVİRGÜL;

block: 
      SOLSÜSLÜPARANTEZ statement* SAĞSÜSLÜPARANTEZ;

returnStatement: 
      DÖNDÜR expression NOKTALIVİRGÜL; // Döndür ifadesi tanımlandı

expression: 
      expression (ARTI | EKSİ | ÇARPIM | BÖLÜ | MOD) expression 
    | expression (EŞİT | EŞİTDEĞİL | BÜYÜK | KÜÇÜK | BÜYÜKEŞİT | KÜÇÜKEŞİT) expression
    | ID
    | NUMBER
    | STRING
    | MANTIK
    | functionCall
    | SOLPARANTEZ expression SAĞPARANTEZ;

functionCall: 
      ID SOLPARANTEZ (argList)? SAĞPARANTEZ;

argList: expression (VİRGÜL expression)*;

SOLPARANTEZ: '(';
SAĞPARANTEZ: ')';
SOLSÜSLÜPARANTEZ: '{';
SAĞSÜSLÜPARANTEZ: '}';

NOKTALIVİRGÜL: ';';
VİRGÜL: ',';

SAYI: 'sayı';
METİN: 'metin';
ONDALIK: 'ondalık';
MANTIK: 'mantık';
BOŞ: 'boş';

ARTI: '+';
EKSİ: '-';
ÇARPIM: '*';
BÖLÜ: '/';
MOD: '%';

ATAMA: '=';
AND: 've';
OR: 'veya';
EŞİT: '==';
EŞİTDEĞİL: '!=';
BÜYÜK: '>';
KÜÇÜK: '<';
BÜYÜKEŞİT: '>=';
KÜÇÜKEŞİT: '<=';

EĞER: 'eğer';
DEĞİLSEEĞER: 'değilseeğer';
DEĞİLSE: 'değilse';
İÇİN: 'için';
İKEN: 'iken';
YAZDIR: 'yazdır';
DÖNDÜR: 'döndür'; // Döndür anahtar kelimesi

ID: [a-zA-Z_][a-zA-Z0-9_]*;
NUMBER: '-'? [0-9]+('.'[0-9]+)?;
STRING: '"' .*? '"';

WS: [ \t\r\n]+ -> skip;
COMMENT: '//' ~[\r\n]* -> skip;
