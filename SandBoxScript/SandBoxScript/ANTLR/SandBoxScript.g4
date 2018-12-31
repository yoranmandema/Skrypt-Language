grammar SandBoxScript;

program				: block EOF ;

block				locals [
					Dictionary<string, SandBoxScript.Variable> Variables = new Dictionary<string, SandBoxScript.Variable>()
					]
					: (
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

importStmnt			: IMPORT Target=expression																					#importStatement
					;

fnStmnt				: FN NAME '(' parameterGroup ')' stmntBlock																	#functionStatement
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

assignStmnt			: NAME	{
if (!$block::Variables.ContainsKey($NAME.text)) {
	$block::Variables[$NAME.text] = new SandBoxScript.Variable($NAME.text);
}
}			
																		ASSIGN expression										#assignNameStatement
					| memberAccess										ASSIGN expression										#assignMemberStatement
					| memberAccessComp									ASSIGN expression										#assignComputedMemberStatement					
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

					| name																										#nameExp
                    | number																									#numberLiteral
					| string																									#stringLiteral
					| vector2																									#vector2Literal
					| vector3																									#vector3Literal
					| vector4																									#vector4Literal
                    ;

name returns [SandBoxScript.Variable variable] : NAME 	{
RuleContext currentContext = $ctx;
bool exists = false;

while (currentContext.Parent != null) {

	if (currentContext is BlockContext blockCtx) {
		if (blockCtx.Variables.ContainsKey($NAME.text)) {
			exists = true;

			$variable = blockCtx.Variables[$NAME.text];
			break;
		}
	}

	currentContext = currentContext.Parent;
}
											  
if (!exists) {
	throw new RecognitionException("Undefined variable: " + $NAME.text, this, this._input, $ctx);
}
} ;

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

STRING : '"' ~('"')* ('"' | {throw new Antlr4.Runtime.Misc.ParseCanceledException("Unterminated string detected");}) ;

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