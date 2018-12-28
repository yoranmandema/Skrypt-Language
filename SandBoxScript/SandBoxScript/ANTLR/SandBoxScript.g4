grammar SandBoxScript;

block				: expression ';'											#expressionExp
					| importStatement											#importStat
					;

importStatement		: IMPORT expression	
					;

expression          : '(' expression ')'										#parenthesisExp
					| expression DOT NAME										#memberAccessExp
					| expression '[' expression ']'								#computedMemberAccessExp
                    | expression '(' expression* (',' expression)* ')'			#functionCallExp
					
					| <assoc=right>		Left=expression Operation=EXPONENT			Right=expression		#binaryOperationExp
                    |					Left=expression Operation=(ASTERISK|SLASH)	Right=expression		#binaryOperationExp
                    |					Left=expression Operation=(PLUS|MINUS)		Right=expression		#binaryOperationExp

					| NAME														#nameExp
                    | NUMBER													#numericAtomExp
					| STRING													#stringExp
                    ;

fragment LETTER			: [a-zA-Z] ;
fragment DIGIT			: [0-9] ;
fragment ESCAPED_QUOTE	: '\\"';

STRING :   '"' ( ESCAPED_QUOTE | ~('\n'|'\r') )*? '"';

ASSIGN	            : '=' ;

ASTERISK            : '*' ;
SLASH               : '/' ;
PLUS                : '+' ;
MINUS               : '-' ;
EXPONENT            : '**';


INCREMENT			: '++';
DECREMENT			: '--';

DOT		            : '.' ;

NAME				: LETTER (LETTER | DIGIT)* ;

NUMBER              : DIGIT+ ('.' DIGIT+)? ;

IMPORT				: 'import' ;

WHITESPACE : ' ' -> channel(HIDDEN);


