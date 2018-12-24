grammar SandBoxScript;

expression          : '(' expression ')'										#parenthesisExp
					| expression DOT NAME										#memberAccessExp
					| expression '[' expression ']'								#computedMemberAccessExp
                    | expression '(' expression* (',' expression)* ')'			#functionCallExp
                    | expression Operation=(ASTERISK|SLASH) expression			#binaryOperationExp
                    | expression Operation=(PLUS|MINUS) expression				#binaryoperationExp
					| <assoc=right>  expression Operation='^' expression		#binaryoperationExp
					| NAME														#nameExp
                    | NUMBER													#numericAtomExp
                    ;

fragment LETTER     : [a-zA-Z] ;
fragment DIGIT      : [0-9] ;

ASTERISK            : '*' ;
SLASH               : '/' ;
PLUS                : '+' ;
MINUS               : '-' ;

INCREMENT			: '++';
DECREMENT			: '--';

DOT		            : '.' ;

NAME				: LETTER (LETTER | DIGIT)* ;

NUMBER              : DIGIT+ ('.' DIGIT+)? ;

WHITESPACE : ' ' -> channel(HIDDEN);


