grammar SandBoxScript;

expression          : '(' expression ')'										#parenthesisExp
					| expression DOT expression									#memberAccessExp
					| expression '[' expression ']'								#computedMemberAccessExp
                    | expression '(' expression (',' expression)* ')'			#functionCallExp
                    | expression Operation=(ASTERISK|SLASH) expression			#operationExp
                    | expression Operation=(PLUS|MINUS) expression				#operationExp
					| <assoc=right>  expression Operation='^' expression		#operationExp
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


