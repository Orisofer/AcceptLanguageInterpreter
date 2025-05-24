This is a small project I've made to learn how to build a programing language.
The project is a small programing language named "Accept".
In this project I've build an Accept Tokenizer, Accept Parser and Accept Interpreter in C#

Source code example:

say _Hello_Im_Accept_Lang%
mish x => 56%
im x > 42:
say _Good!%
o:
say _Shitty_Number%
say _the_program_is_working%
accept

Literals and keywords:

"say" -> prints something to the console
"mish" -> variable declaration keyword
"im" -> start of an if block
"o:" -> start of an else block
"accept" -> end of program
"%" -> end of line token

Language grammars:

<Program> ::= <Statement>* 'accept'
<Statement> ::= <SayStatement> | <VarDeclaration> | <IfElse>
<SayStatement> ::= 'say' '_' <String> '%'
<VarDeclaration> ::= 'mish' <Identifier> '=>' <Nummber> '%'
<IfElse> ::= 'im' <Identifier> <Comparison> <Number> ':' <Statement>* <Dedent> 'o:' <Statement>*
<Identifier> ::= <Character>*
<Character> ::= [a-z]* | [a-z]* <Nummber>
<Nummber> ::= <Digit>*
<Digit> ::= [0-9]
