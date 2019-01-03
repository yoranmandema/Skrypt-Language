grammar SandBoxScript;

program				: block EOF ;

block				: (
					importStmnt
					| moduleStmnt
					| ifStmnt
					| forStmnt
					| whileStmnt
					| fnStmnt 
					| returnStmnt
					| breakStmnt
					| continueStmnt
					| assignStmnt
					| expression
					)*
					;

stmntBlock			: '{' Block=block '}'
					| returnStmnt
					| continueStmnt
					| assignStmnt
					| expression
					;

importStmnt			: IMPORT name (DOT NAME)*?	{

var nameCtx = ($ctx as ImportStatementContext).name();
var scope = GetDefinitionBlock($ctx);

if (nameCtx.variable == null) {
	Engine.ErrorHandler.AddError(nameCtx.NAME().Symbol, "Undefined variable: " + nameCtx.GetText());
} else {

	var root = nameCtx.variable.Value;
	var target = root;

	var members = ($ctx as ImportStatementContext).NAME();

	foreach (var m in members) {
		try {
			target = target.GetProperty(m.GetText()).Value;
		} catch (System.Exception e) {
			Engine.ErrorHandler.AddError(nameCtx.NAME().Symbol, e.Message);
		}
	}

	foreach (var m in target.Members) {
		var v = m.Value;

		scope.Variables[m.Key] = new SandBoxScript.Variable(m.Key,v.Value);
	}

}

}																																#importStatement
					;

moduleStmnt			: MODULE name  '{' moduleProperty+ '}' {

var Ctx = ($ctx as ModuleStatementContext);
var nameCtx = Ctx.name();
var block = GetDefinitionBlock($ctx.Parent);

var module = new SandBoxScript.Variable(nameCtx.GetText(), new ScriptModule(nameCtx.GetText(), this.Engine));

block.Variables[nameCtx.GetText()] = module;

foreach (var c in Ctx.moduleProperty()) {
	this.Engine.Visitor.Visit(c);

	IToken nameToken = null;
	bool isValid = false;

	if (c.GetChild(0) is AssignNameStatementContext assignCtx) {
		nameToken = assignCtx.name().NAME().Symbol;
	}

	var value = Ctx.Variables[nameToken.Text].Value;

	if (value == null) {
		Engine.ErrorHandler.AddError(nameToken, "Module field can't be set to an undefined value.");
	}

    module.Value.CreateProperty(nameToken.Text, value);
}

//foreach (var kv in Ctx.Variables) {
//    var v = kv.Value;

//	if (v.Value == null) {
//		Engine.ErrorHandler.AddError(nameCtx.NAME().Symbol, "Module field can't be set to an undefined value.");
//	}	

//    module.Value.CreateProperty(kv.Key, v.Value);
//}
}																																#moduleStatement
					;

moduleProperty		: (assignStmnt | fnStmnt | moduleStmnt) ;

fnStmnt				locals [
					BaseValue ReturnValue = null,
					SandBoxScript.JumpState JumpState = SandBoxScript.JumpState.None
					]
					: FN name '(' parameterGroup ')' {
var fnCtx = ($ctx as FunctionStatementContext);
var nameCtx = fnCtx.name();

var newVar = new SandBoxScript.Variable(nameCtx.GetText());

var scope = GetDefinitionBlock($ctx.Parent);

scope.Variables[nameCtx.GetText()] = newVar;
nameCtx.variable = newVar;		

var parameters = fnCtx.parameterGroup().parameter();
var processedParameters = new SandBoxScript.Parameter[parameters.Length];

for (var i = 0; i < parameters.Length; i++) {
	var p = parameters[i];
	var name = p.NAME().GetText();

	processedParameters[i] = new SandBoxScript.Parameter(name, p.expression()); 

	var parameterVar = new SandBoxScript.Variable(name);

	fnCtx.Variables[name] = parameterVar;
}

} stmntBlock {
	var function = new SandBoxScript.ScriptFunction(fnCtx) { 
		Parameters = processedParameters
	}; 
	var functionVar = new SandBoxScript.FunctionInstance(this.Engine, function); 

	newVar.Value = functionVar;													
}																																#functionStatement
					;											

returnStmnt			locals [
					FunctionStatementContext Statement
					]
					: RETURN expression? {
$Statement = GetFirstOfType<FunctionStatementContext>($ctx);

if ($Statement == null) {
	Engine.ErrorHandler.AddError((_localctx as ReturnStatementContext).RETURN().Symbol, "Return statement must be inside a function.");
}
}																																#returnStatement
					;

parameterGroup		: (parameter (',' parameter)*)? ;								

parameter			: NAME ('=' expression)?;

forStmnt			: FOR '(' Instantiator=assignStmnt ',' Condition=expression ',' Modifier=expression ')' stmntBlock			#forStatement
					;

whileStmnt			: WHILE '(' Condition=expression ')' stmntBlock																#whileStatement
					;

continueStmnt		locals [
					RuleContext Statement,
					SandBoxScript.JumpState JumpState = SandBoxScript.JumpState.None
					]
					: CONTINUE {
$Statement = GetFirstOfType<WhileStatementContext>($ctx);

if ($Statement == null) {
	Engine.ErrorHandler.AddError((_localctx as ContinueStatementContext).CONTINUE().Symbol, "Continue statement must be inside a loop.");
}
}																																#continueStatement
					;

breakStmnt			locals [
					RuleContext Statement,
					SandBoxScript.JumpState JumpState = SandBoxScript.JumpState.None
					]
					: BREAK {
$Statement = GetFirstOfType<WhileStatementContext>($ctx);

if ($Statement == null) {
	Engine.ErrorHandler.AddError((_localctx as BreakStatementContext).BREAK().Symbol, "Break statement must be inside a loop.");
}
}																																#breakStatement
					;

ifStmnt				: if (elseif)* else?																						#ifStatement			
					;

if					: IF '(' Condition=expression ')' stmntBlock 
					;

elseif				: ELSE IF '(' Condition=expression ')' stmntBlock
					;

else				: ELSE stmntBlock																			
					;

assignStmnt			: name				ASSIGN expression	{
var assignNameCtx = ($ctx as AssignNameStatementContext);
var nameCtx = assignNameCtx.name();
var block = GetDefinitionBlock(nameCtx.GetText(), $ctx);

if (nameCtx.variable == null) {
	var newVar = new SandBoxScript.Variable(nameCtx.GetText());

	block.Variables[nameCtx.GetText()] = newVar;
	nameCtx.variable = newVar;
}

var isInFunction = block.Context.Parent is StmntBlockContext SmntBlock && SmntBlock.Parent is FunctionStatementContext;

if (!isInFunction) {
	try {
		nameCtx.variable.Value = this.Engine.Visitor.Visit(assignNameCtx.expression());
	} catch (System.Exception e) {
		
	}
}
}																																#assignNameStatement
					| memberAccess		ASSIGN expression																		#assignMemberStatement
					| memberAccessComp	ASSIGN expression																		#assignComputedMemberStatement					
					;

expression          : '(' expression ')'																						#parenthesisExp
					| expression DOT NAME																						#memberAccessExp
					| expression '[' expression ']'																				#computedMemberAccessExp
                    | Function=expression '(' Arguments=expressionGroup ')'														#functionCallExp
					
					| Target=expression Operation=(INCREMENT|DECREMENT) 														#postfixOperationExp		
					| Operation=(MINUS|NOT|BITNOT|INCREMENT|DECREMENT) Target=expression										#prefixOperationExp
			


					| <assoc=right>		Left=expression Operation=EXPONENT			Right=expression							#binaryOperationExp
                    |					Left=expression Operation=(ASTERISK|SLASH|REMAINDER)	Right=expression				#binaryOperationExp
                    |					Left=expression Operation=(PLUS|MINUS)		Right=expression							#binaryOperationExp

                    |					Left=expression Operation=(LESS|LESSEQ|GREATER|GREATEREQ)	Right=expression			#binaryOperationExp
                    |					Left=expression Operation=(EQUAL|NOTEQUAL)					Right=expression			#binaryOperationExp

					|					Left=expression Operation=AND	Right=expression										#binaryOperationExp
                    |					Left=expression Operation=OR	Right=expression										#binaryOperationExp

                    | number																									#numberLiteral
					| string																									#stringLiteral
					| boolean																									#booleanLiteral
					| vector2																									#vector2Literal
					| vector3																									#vector3Literal
					| vector4																									#vector4Literal
					| name {
var nameCtx = ($ctx as NameExpContext).name();

if (nameCtx.variable == null) {
	Engine.ErrorHandler.AddError(nameCtx.NAME().Symbol, "Undefined variable: " + nameCtx.GetText());
}																								
}																																#nameExp
                    ;

name returns [SandBoxScript.Variable variable] : NAME 	{
var scope = GetDefinitionBlock($NAME.text, $ctx);

$variable = GetReference($NAME.text, scope);
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

boolean returns [bool value] : BOOLEAN { 
$value = $BOOLEAN.text == "true" ? true : false; 
} ;

vector2				:	'<' X=expression ',' Y=expression '>' ;
vector3				:	'<' X=expression ',' Y=expression ',' Z=expression '>' ;
vector4				:	'<' X=expression ',' Y=expression ',' Z=expression ',' W=expression'>' ;

expressionGroup		: (expression (',' expression)*)? ;

fragment LETTER			: [a-zA-Z] ;
fragment DIGIT			: [0-9] ;
fragment ESCAPED_QUOTE	: '\\"';
fragment TRUE			: 'true';
fragment FALSE			: 'false';

DOT						: '.' ;

IMPORT					: 'import' ;
MODULE					: 'module' ;
IF						: 'if' ;
ELSE					: 'else' ;
FN						: 'fn' ;
WHILE					: 'while' ;
FOR						: 'for' ;
RETURN					: 'return' ;
BREAK					: 'break' ;
CONTINUE				: 'continue' ;

KEYWORD					: (IMPORT | MODULE | IF | ELSE | FN | WHILE | RETURN | BREAK | CONTINUE) ;

LESS					: '<'	;
LESSEQ					: '<='	;
GREATER					: '>'	;
GREATEREQ				: '>='	;
EQUAL					: '=='	;
NOTEQUAL				: '!='	;

AND						: 'and' ;
OR						: 'or' ;

ASSIGN					: '='	;

ASTERISK				: '*'	;
SLASH					: '/'	;
PLUS					: '+'	;
MINUS					: '-'	;
REMAINDER				: '%'	;
EXPONENT				: '**'	;

BITAND					: '&' ;
BITOR					: '^' ;
BITXOR					: '|' ; 

INCREMENT				: '++'	;
DECREMENT				: '--'	;
NOT						: '!' ;
BITNOT					: '~' ;

BOOLEAN					: TRUE | FALSE ;

NAME					: LETTER (LETTER | DIGIT)*;

NUMBER					: DIGIT+ ('.' DIGIT+)?;

STRING					: '"' ~('"')* ('"' | {

System.Console.WriteLine(Token);

Engine.ErrorHandler.FatalError(Token, "Unterminated string.");

}) ;

WHITESPACE				: [ \n\t\r]+ -> channel(HIDDEN);

COMMENT					: '/*' .*? '*/' -> skip ;

LINE_COMMENT			: '//' ~[\r\n]* -> skip ;

// handle characters which failed to match any other token
ErrorCharacter : . ;