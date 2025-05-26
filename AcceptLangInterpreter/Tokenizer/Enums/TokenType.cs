namespace AcceptLangInterpreter;

[System.Flags]
public enum TokenType
{
    SAY,
    STRING,
    NUMBER,
    VAR_DECLARATION,
    IDENTIFIER,
    ASSIGN,
    ENDL,
    IF,
    END_OF_IF_ELSE_LINE,
    ELSE,
    GREATER_THAN = 1,
    EQUAL = 2,
    LESS_THAN = 4,
    END_OF_PROGRAM
}