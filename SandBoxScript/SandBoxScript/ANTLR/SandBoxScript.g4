grammar SandBoxScript;

expression          : '(' expression ')'                        #parenthesisExp
					| expression DOT expression					#memberAccessExp
					| expression '[' expression ']'				#computedMemberAccessExp
                    | expression '(' expression (',' expression)* ')'    #functionCallExp
                    | expression (ASTERISK|SLASH) expression    #mulDivExp
                    | expression (PLUS|MINUS) expression        #addSubExp
					| <assoc=right>  expression '^' expression	#powerExp
					| NAME						                #nameExp
                    | NUMBER                                    #numericAtomExp
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


