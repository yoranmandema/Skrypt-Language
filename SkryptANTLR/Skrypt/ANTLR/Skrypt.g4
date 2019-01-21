grammar Skrypt;

program				: block EOF ;

block				: (
					importStmnt
					| importAllFromStmnt
					| importFromStmnt
					| moduleStmnt
					| structStmnt
					| traitStmnt
					| traitImplStmnt
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

		scope.Variables[m.Key] = new Skrypt.Variable(m.Key,v.Value);
	}

}

}																																#importStatement
					;

importAllFromStmnt	: IMPORT ASTERISK FROM string {
var Ctx = ($ctx as ImportAllFromStatementContext);

var relativePath = Ctx.@string().value;
var input = Engine.FileHandler.Read(relativePath);

Engine.Run(input).CreateGlobals();

}																																#importAllFromStatement
					;

importFromStmnt		: IMPORT NAME (',' NAME)* FROM string {
var Ctx = ($ctx as ImportFromStatementContext);
var scope = GetDefinitionBlock($ctx);

var relativePath = Ctx.@string().value;
var input = Engine.FileHandler.Read(relativePath);

Engine.Run(input);

foreach (var n in Ctx.NAME()) {
	var name = n.GetText();

	scope.Variables[name] = new Skrypt.Variable(name, Engine.GetValue(name));
}

}																																#importFromStatement
					;

moduleStmnt			: MODULE name {

var isInValidContext = ContextIsIn($ctx, new [] {typeof(ModuleStatementContext), typeof(ProgramContext)});

if (!isInValidContext) {
	Engine.ErrorHandler.AddError($ctx.Start, "Module has to be in global scope or module block.");
}

var Ctx = ($ctx as ModuleStatementContext);
var nameCtx = Ctx.name();
var block = GetDefinitionBlock($ctx.Parent);

if (nameCtx.variable != null && nameCtx.variable.IsConstant) Engine.ErrorHandler.AddError(nameCtx.Start, "Constant cannot be redefined.");

var module = new Skrypt.Variable(nameCtx.GetText(), new ScriptModule(nameCtx.GetText(), this.Engine));

block.Variables[nameCtx.GetText()] = module;

} '{' property* '}' {

foreach (var c in Ctx.property()) {
	this.Engine.Visitor.Visit(c);

	CreateProperty(module.Value, Ctx, c, false);
}

}																																#moduleStatement
					;


structStmnt			: STRUCT name {

var isInValidContext = ContextIsIn($ctx, new [] {typeof(ModuleStatementContext), typeof(ProgramContext), typeof(StructStatementContext)});

if (!isInValidContext) {
	Engine.ErrorHandler.AddError($ctx.Start, "Struct has to be in global scope, module block or struct block.");
}

var Ctx = ($ctx as StructStatementContext);
var nameCtx = Ctx.name();
var block = GetDefinitionBlock($ctx.Parent);
var typeName = nameCtx.GetText();

if (nameCtx.variable != null && nameCtx.variable.IsConstant) Engine.ErrorHandler.AddError(nameCtx.Start, "Constant cannot be redefined.");

var type = new Skrypt.Variable(typeName, new ScriptType(typeName, this.Engine));
var template = new Template {Name = typeName};

block.Variables[nameCtx.GetText()] = type;
Ctx.Variables[nameCtx.GetText()] = type;

}  '{' structProperty* '}' {

foreach (var c in Ctx.structProperty()) {
	var isPrivate = c.PRIVATE() != null;	
	var isStatic = c.STATIC() != null;

	this.Engine.Visitor.Visit(c.Property);

	if (isStatic) {
		CreateProperty(type.Value, Ctx, c.Property, isPrivate);
	} else {
		var nameToken = GetPropertyNameToken(c.Property);
		var value = Ctx.Variables[nameToken.Text].Value;

        if (value == null) {
            Engine.ErrorHandler.AddError(c.Property.Start, "Field can't be set to an undefined value.");
        }

		if (nameToken.Text == "init" && value is FunctionInstance function) {
			(type.Value as ScriptType).Constructor = function.Function as ScriptFunction;
			continue;
		}

		template.Members[nameToken.Text] = new Member(value, isPrivate, Ctx);
	}
}

(type.Value as ScriptType).Template = template;

}																																#structStatement
					;

traitStmnt			: TRAIT name {

var isInValidContext = ContextIsIn($ctx, new [] {typeof(ModuleStatementContext), typeof(ProgramContext)});

if (!isInValidContext) {
	Engine.ErrorHandler.AddError($ctx.Start, "Trait has to be in global scope or module block.");
}

var Ctx = ($ctx as TraitStatementContext);
var nameCtx = Ctx.name();
var block = GetDefinitionBlock($ctx.Parent);
var traitName = nameCtx.GetText();

if (nameCtx.variable != null && nameCtx.variable.IsConstant) Engine.ErrorHandler.AddError(nameCtx.Start, "Constant cannot be redefined.");

var trait = new ScriptTrait(traitName, this.Engine);
var traitVariable = new Skrypt.Variable(traitName, trait);

block.Variables[nameCtx.GetText()] = traitVariable;

} propertiesBlock {

foreach (var child in Ctx.propertiesBlock().property()) {
	this.Engine.Visitor.Visit(child);

	var nameToken = GetPropertyNameToken(child);
	var value = Ctx.Variables[nameToken.Text].Value;

    if (value == null) {
        Engine.ErrorHandler.AddError(nameToken, "Field can't be set to an undefined value.");
    }

	trait.TraitMembers[nameToken.Text] = new Member(value, false, Ctx);
}

}																																#traitStatement
					;

traitImplStmnt		: IMPL name FOR name propertiesBlock? {
var isInValidContext = ContextIsIn($ctx, new [] {typeof(ProgramContext)});

if (!isInValidContext) {
	Engine.ErrorHandler.AddError($ctx.Start, "Implementation has to be in global scope.");
}

var Ctx = ($ctx as TraitImplStatementContext);
var traitNameCtx = Ctx.name(0);
var typeNameCtx = Ctx.name(1);

var trait = traitNameCtx.variable.Value as BaseTrait;
var type = typeNameCtx.variable.Value as BaseType;

if (!typeof(BaseTrait).IsAssignableFrom(traitNameCtx.variable.Value.GetType())) {
	Engine.ErrorHandler.AddError(traitNameCtx.NAME().Symbol, "Trait expected.");
}

if (!typeof(BaseType).IsAssignableFrom(typeNameCtx.variable.Value.GetType())) {
	Engine.ErrorHandler.AddError(typeNameCtx.NAME().Symbol, "Type expected.");
}

type.Traits.Add(trait);

foreach (var kv in trait.TraitMembers) {
	type.Template.Members[kv.Key] = kv.Value;
}

var modifiesProperties = Ctx.propertiesBlock() != null;

if (modifiesProperties) {
	foreach (var child in Ctx.propertiesBlock().property()) {
		this.Engine.Visitor.Visit(child);

		var nameToken = GetPropertyNameToken(child);	
		var value = Ctx.Variables[nameToken.Text].Value;

		if (!trait.TraitMembers.ContainsKey(nameToken.Text)) {
			Engine.ErrorHandler.AddError(nameToken, $"Trait does not contain property {nameToken.Text}.");
			continue;
		}

		if (value == null) {
			Engine.ErrorHandler.AddError(nameToken, "Field can't be set to an undefined value.");
		}

		type.Template.Members[nameToken.Text].Value = value;
	}
}
}																																#traitImplStatement
					;

propertiesBlock		: '{' property+ '}' ;
traitProperty		: property ;
structProperty		: PRIVATE? STATIC? Property=property ;
moduleProperty		: property | moduleStmnt ;
property			: assignStmnt | fnStmnt ;

fnStmnt				locals [
					BaseObject ReturnValue = null,
					Skrypt.JumpState JumpState = Skrypt.JumpState.None
					]
					: CONST? FN name '(' parameterGroup ')' {
var fnCtx = ($ctx as FunctionStatementContext);
var nameCtx = fnCtx.name();
var isConstant = fnCtx.CONST() != null;

if (nameCtx.variable != null && nameCtx.variable.IsConstant) Engine.ErrorHandler.AddError(nameCtx.Start, "Constant cannot be redefined.");

var newVar = new Skrypt.Variable(nameCtx.GetText());

var scope = GetDefinitionBlock($ctx.Parent);

scope.Variables[nameCtx.GetText()] = newVar;
nameCtx.variable = newVar;		

fnCtx.Variables["self"] = new Variable("self", null){IsConstant = true};

var parameters = fnCtx.parameterGroup().parameter();
var processedParameters = new Skrypt.Parameter[parameters.Length];

for (var i = 0; i < parameters.Length; i++) {
	var p = parameters[i];
	var name = p.NAME().GetText();

	processedParameters[i] = new Skrypt.Parameter(name, p.expression()); 

	var parameterVar = new Skrypt.Variable(name);

	fnCtx.Variables[name] = parameterVar;
}

} stmntBlock {
	var function = new Skrypt.ScriptFunction(fnCtx) { 
		Parameters = processedParameters
	}; 
	var functionVar = new Skrypt.FunctionInstance(this.Engine, function); 

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
					Skrypt.JumpState JumpState = Skrypt.JumpState.None
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
					Skrypt.JumpState JumpState = Skrypt.JumpState.None
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

assignStmnt			: CONST? name ASSIGN expression	{
var assignNameCtx = ($ctx as AssignNameStatementContext);
var nameCtx = assignNameCtx.name();
var block = GetDefinitionBlock(nameCtx.GetText(), $ctx);
var isConstant = assignNameCtx.CONST() != null;

if (nameCtx.variable != null && nameCtx.variable.IsConstant) Engine.ErrorHandler.AddError(nameCtx.Start, "Constant cannot be redefined.");

if (nameCtx.variable == null) {
	var newVar = new Skrypt.Variable(nameCtx.GetText()) {
		IsConstant = isConstant
	};

	block.Variables[nameCtx.GetText()] = newVar;
	nameCtx.variable = newVar;
} 	

var isInFunction = block.Context.Parent is StmntBlockContext SmntBlock && SmntBlock.Parent is FunctionStatementContext;
}																																#assignNameStatement
					| memberAccess		ASSIGN expression																		#assignMemberStatement
					| memberAccessComp	ASSIGN expression																		#assignComputedMemberStatement					
					;

expression          : '(' expression ')'																						#parenthesisExp
					| expression DOT NAME	 																					#memberAccessExp
					| expression '[' expression ']'																				#computedMemberAccessExp
                    | Function=expression '(' Arguments=expressionGroup ')'														#functionCallExp
					
					| Target=expression Operation=(INCREMENT|DECREMENT) 														#postfixOperationExp		
					| Operation=(MINUS|NOT|BITNOT|INCREMENT|DECREMENT) Target=expression										#prefixOperationExp
			


					| <assoc=right>		Left=expression Operation=EXPONENT			Right=expression							#binaryOperationExp
                    |					Left=expression Operation=(ASTERISK|SLASH|REMAINDER)	Right=expression				#binaryOperationExp
                    |					Left=expression Operation=(PLUS|MINUS)		Right=expression							#binaryOperationExp

                    |					Left=expression Operation=(LESS|LESSEQ|GREATER|GREATEREQ)	Right=expression			#binaryOperationExp
                    |					Left=expression Operation=(EQUAL|NOTEQUAL|IS)				Right=expression			#binaryOperationExp

					|					Left=expression Operation=AND	Right=expression										#binaryOperationExp
                    |					Left=expression Operation=OR	Right=expression										#binaryOperationExp

                    | number																									#numberLiteral
					| string																									#stringLiteral
					| boolean																									#booleanLiteral
					| null																										#nullLiteral
					| vector2																									#vector2Literal
					| vector3																									#vector3Literal
					| vector4																									#vector4Literal
					| array																										#arrayLiteral
					| name {
var nameCtx = ($ctx as NameExpContext).name();

if (nameCtx.variable == null) {
	Engine.ErrorHandler.AddError(nameCtx.NAME().Symbol, "Undefined variable: " + nameCtx.GetText());
}																								
}																																#nameExp
                    ;

name returns [Skrypt.Variable variable] : NAME 	{
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

null returns [object value] : NULL { 
$value = null; 
} ;

vector2				:	'<' X=expression ',' Y=expression '>' ;
vector3				:	'<' X=expression ',' Y=expression ',' Z=expression '>' ;
vector4				:	'<' X=expression ',' Y=expression ',' Z=expression ',' W=expression'>' ;

array				: '[' expressionGroup ']' ; 

expressionGroup		: (expression (',' expression)*)? ;

fragment LETTER			: [a-zA-Z] ;
fragment DIGIT			: [0-9] ;
fragment ESCAPED_QUOTE	: '\\"';
fragment TRUE			: 'true';
fragment FALSE			: 'false';

DOT						: '.' ;

IMPORT					: 'import' ;
FROM					: 'from' ;
MODULE					: 'module' ;
STRUCT					: 'struct' ;
TRAIT					: 'trait' ;
IMPL					: 'impl' ;
IF						: 'if' ;
ELSE					: 'else' ;
FN						: 'fn' ;
WHILE					: 'while' ;
FOR						: 'for' ;
RETURN					: 'return' ;
BREAK					: 'break' ;
CONTINUE				: 'continue' ;
STATIC					: 'static' ;
PRIVATE					: 'private' ;
CONST					: 'const' ;

KEYWORD					: (IMPORT | MODULE | IF | ELSE | FN | WHILE | FOR | RETURN | BREAK | CONTINUE | STATIC | PRIVATE | CONST) ;

LESS					: '<'	;
LESSEQ					: '<='	;
GREATER					: '>'	;
GREATEREQ				: '>='	;
EQUAL					: '=='	;
NOTEQUAL				: '!='	;
IS						: 'is' ;

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

NULL					: 'null' ;

BOOLEAN					: TRUE | FALSE ;

NAME					: ('_' | LETTER) ('_' | LETTER | DIGIT)*;

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