# Accept Lang

This is a small project I've made to learn how to build a programming language.

The project is a small programming language named **"Accept"**.  
In this project, I've built:

- An Accept Tokenizer
- An Accept Parser
- An Accept Interpreter in C#

---

## 🔤 Source Code Example

```accept
say _Hello_Im_Accept_Lang%
mish x => 56%
im x > 42:
say _Good!%
o:
say _Shitty_Number%
say _the_program_is_working%
accept

🧠 Literals and Keywords
say → prints something to the console

mish → variable declaration keyword

im → start of an if block

o: → start of an else block

accept → end of program

% → end of line token

📚 Language Grammar (BNF Style)
bnf
Copy
Edit
<Program> ::= <Statement>* 'accept'
<Statement> ::= <SayStatement> | <VarDeclaration> | <IfElse>
<SayStatement> ::= 'say' '_' <String> '%'
<VarDeclaration> ::= 'mish' <Identifier> '=>' <Number> '%'
<IfElse> ::= 'im' <Identifier> <Comparison> <Number> ':' <Statement>* <Dedent> 'o:' <Statement>*
<Identifier> ::= <Character>*
<Character> ::= [a-z]* | [a-z]* <Number>
<Number> ::= <Digit>*
<Digit> ::= [0-9]
