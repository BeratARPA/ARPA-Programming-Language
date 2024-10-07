grammar ARPA;

program: statement*;

statement: 
      declaration
    | assignment
    | expressionStatement
    | ifStatement
    | printStatement
    | block;

declaration: 
      variableDeclaration 
    | functionDeclaration;

variableDeclaration: 
      (SAYI | METİN | ONDALIK | MANTIK) ID (ATAMA expression)? NOKTALIVİRGÜL;

functionDeclaration: 
      (SAYI | METİN | ONDALIK | MANTIK) ID SOLPARANTEZ (ID (',' ID)*)? SAĞPARANTEZ SOLSÜSLÜPARANTEZ statement* SAĞSÜSLÜPARANTEZ;

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

expression: 
      expression (ARTI | EKSİ | ÇARPIM | BÖLÜ | MOD) expression 
    | expression (EŞİT | EŞİTDEĞİL | BÜYÜK | KÜÇÜK | BÜYÜKEŞİT | KÜÇÜKEŞİT) expression
    | ID
    | NUMBER
    | STRING
    | MANTIK // Düzeltme yapıldı
    | SOLPARANTEZ expression SAĞPARANTEZ;

SOLPARANTEZ: '(';
SAĞPARANTEZ: ')';
SOLSÜSLÜPARANTEZ: '{';
SAĞSÜSLÜPARANTEZ: '}';
NOKTALIVİRGÜL: ';';

SAYI: 'sayı';
METİN: 'metin';
ONDALIK: 'ondalık';
MANTIK: 'mantık'; // MANTIK kuralı burada tanımlanıyor

ARTI: '+' ;
EKSİ: '-' ;
ÇARPIM: '*' ;
BÖLÜ: '/' ;
MOD: '%' ;

ATAMA: '=' ;
AND: 've' ;
OR: 'veya' ;
EŞİT: '==' ;
EŞİTDEĞİL: '!=' ;
BÜYÜK: '>' ;
KÜÇÜK: '<' ;
BÜYÜKEŞİT: '>=' ;
KÜÇÜKEŞİT: '<=' ;

EĞER: 'eğer' ;
DEĞİLSEEĞER: 'değilseeğer' ;
DEĞİLSE: 'değilse' ;
İÇİN: 'için' ;
İKEN: 'iken' ;
YAZDIR: 'yazdır' ;

ID: [a-zA-Z_][a-zA-Z0-9_]*;
NUMBER: '-'? [0-9]+('.'[0-9]+)? ;
STRING: '"' .*? '"' ;

WS: [ \t\r\n]+ -> skip;
COMMENT: '//' ~[\r\n]* -> skip;
