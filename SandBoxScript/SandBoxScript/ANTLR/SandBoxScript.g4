grammar SandBoxScript;

block				: command* ;

command				: importStmnt
					| ifStmnt
					| assignStmnt
					| expression										
					;

importStmnt			: IMPORT Target=expression																#importStatement
					;

ifStmnt				: IF '(' Condition=expression ')' stmntBlock elseifStmnt* elseStmnt?					#ifStatement			
					;

elseifStmnt			: ELSE IF '(' Condition=expression ')' stmntBlock
					;

elseStmnt			: ELSE stmntBlock																			
					;

stmntBlock			: '{' Block=block '}'
					| expression
					;

assignStmnt			: NAME								ASSIGN expression									#assignNameStatement
					| memberAccess						ASSIGN expression									#assignMemberStatement
					| memberAccessComp					ASSIGN expression									#assignComputedMemberStatement
					;

expression          : '(' expression ')'																	#parenthesisExp
					| expression DOT NAME																	#memberAccessExp
					| expression '[' expression ']'															#computedMemberAccessExp
                    | Function=expression '(' Arguments=expressionGroup ')'									#functionCallExp
					
					| <assoc=right>		Left=expression Operation=EXPONENT			Right=expression		#binaryOperationExp
                    |					Left=expression Operation=(ASTERISK|SLASH)	Right=expression		#binaryOperationExp
                    |					Left=expression Operation=(PLUS|MINUS)		Right=expression		#binaryOperationExp
                    |					Left=expression Operation=EQUAL				Right=expression		#binaryOperationExp

					| NAME																					#nameExp
                    | number																				#numberLiteral
					| string																				#stringLiteral
                    ;

memberAccess		: expression DOT NAME ;
memberAccessComp	: expression '[' expression ']' ;

string returns [string value] : STRING { 
var content = $STRING.text.Substring(1,  $STRING.text.Length - 2);

$value = System.Text.RegularExpressions.Regex.Unescape(content);
} ;

number returns [double value] : NUMBER { 
$value = double.Parse($NUMBER.text); 
} ;

expressionGroup		: (expression (',' expression)*)? ;

fragment LETTER			: [a-zA-Z] ;
fragment DIGIT			: [0-9] ;
fragment ESCAPED_QUOTE	: '\\"';
DOT						: '.' ;
IMPORT					: 'import' ;
IF						: 'if' ;
ELSE					: 'else' ;

STRING : '"' ~('"')* ('"' | {throw new RecognitionException("Unterminated string detected.", this, this.InputStream, (ParserRuleContext)_localctx);}) ;

EQUAL				: '==' ;

ASSIGN	            : '=' ;

ASTERISK            : '*' ;
SLASH               : '/' ;
PLUS                : '+' ;
MINUS               : '-' ;
EXPONENT            : '**';

INCREMENT			: '++';
DECREMENT			: '--';

NAME				: LETTER (LETTER | DIGIT)* ;

NUMBER              : DIGIT+ ('.' DIGIT+)? ;

WHITESPACE : [ \n\t\r]+ -> channel(HIDDEN);

// handle characters which failed to match any other token
ErrorCharacter : . ;