grammar SandBoxScript;

program				: block EOF ;

block				: (
					importStmnt
					| ifStmnt
					| fnStmnt 
					| assignStmnt
					| expression
					)*
					;

stmntBlock			: '{' Block=block '}'
					| expression
					;

fnBlock				: (
					importStmnt
					| ifStmnt
					| fnStmnt 
					| returnStmnt
					| assignStmnt
					| expression
					)*
					;

fnStmntBlock		: '{' Block=fnBlock '}'
					| expression
					;

importStmnt			: IMPORT Target=expression																					#importStatement
					;

fnStmnt				: FN NAME '(' parameterGroup ')' fnStmntBlock																#functionStatement
					;											

returnStmnt			: RETURN expression?																						#returnStatement
					;

parameterGroup		: (parameter (',' parameter)*)? ;							

parameter			: NAME ;

ifStmnt				: if (elseif)* else?																						#ifStatement			
					;

if					: IF '(' Condition=expression ')' stmntBlock 
					;

elseif				: ELSE IF '(' Condition=expression ')' stmntBlock
					;

else				: ELSE stmntBlock																			
					;

assignStmnt			: NAME								ASSIGN expression														#assignNameStatement
					| memberAccess						ASSIGN expression														#assignMemberStatement
					| memberAccessComp					ASSIGN expression														#assignComputedMemberStatement
					;

expression          : '(' expression ')'																						#parenthesisExp
					| expression DOT NAME																						#memberAccessExp
					| expression '[' expression ']'																				#computedMemberAccessExp
                    | Function=expression '(' Arguments=expressionGroup ')'														#functionCallExp
					
					| <assoc=right>		Left=expression Operation=EXPONENT			Right=expression							#binaryOperationExp
                    |					Left=expression Operation=(ASTERISK|SLASH)	Right=expression							#binaryOperationExp
                    |					Left=expression Operation=(PLUS|MINUS)		Right=expression							#binaryOperationExp

                    |					Left=expression Operation=(LESS|LESSEQ|GREATER|GREATEREQ)	Right=expression			#binaryOperationExp
                    |					Left=expression Operation=(EQUAL|NOTEQUAL)					Right=expression			#binaryOperationExp

					| NAME																										#nameExp
                    | number																									#numberLiteral
					| string																									#stringLiteral
					| vector2																									#vector2Literal
					| vector3																									#vector3Literal
					| vector4																									#vector4Literal
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

vector2				:	'<' X=expression ',' Y=expression '>' ;
vector3				:	'<' X=expression ',' Y=expression ',' Z=expression '>' ;
vector4				:	'<' X=expression ',' Y=expression ',' Z=expression ',' W=expression'>' ;

expressionGroup		: (expression (',' expression)*)? ;

fragment LETTER			: [a-zA-Z] ;
fragment DIGIT			: [0-9] ;
fragment ESCAPED_QUOTE	: '\\"';

DOT						: '.' ;

IMPORT					: 'import' ;
IF						: 'if' ;
ELSE					: 'else' ;
FN						: 'fn' ;
RETURN					: 'return' ;
BREAK					: 'break' ;
CONTINUE				: 'continue' ;

KEYWORD					: (IMPORT | IF | ELSE | FN | RETURN | BREAK | CONTINUE) ;

STRING : '"' ~('"')* ('"' | {throw new RecognitionException("Unterminated string detected.", this, this.InputStream, (ParserRuleContext)_localctx);}) ;

LESS				: '<'	;
LESSEQ				: '<='	;
GREATER				: '>'	;
GREATEREQ			: '>='	;
EQUAL				: '=='	;
NOTEQUAL			: '!='	;

ASSIGN	            : '='	;

ASTERISK            : '*'	;
SLASH               : '/'	;
PLUS                : '+'	;
MINUS               : '-'	;
EXPONENT            : '**'	;

INCREMENT			: '++'	;
DECREMENT			: '--'	;

NAME				: LETTER (LETTER | DIGIT)*;

NUMBER              : DIGIT+ ('.' DIGIT+)?;

WHITESPACE : [ \n\t\r]+ -> channel(HIDDEN);

// handle characters which failed to match any other token
ErrorCharacter : . ;