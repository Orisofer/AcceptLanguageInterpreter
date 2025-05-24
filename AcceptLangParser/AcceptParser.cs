namespace AcceptLangParser;

public class AcceptParser
{
    private readonly Queue<Token> m_Tokens;
    private Queue<AcceptNodeBase> m_Statements;
    
    public Queue<AcceptNodeBase> Statements => m_Statements;
    
    private const TokenType IF_ELSE_CONDITIONS = TokenType.LESS_THAN | TokenType.GREATER_THAN | TokenType.EQUAL;

    public AcceptParser(Queue<Token> tokens)
    {
        m_Tokens = tokens;
    }

    public Queue<AcceptNodeBase> BuildAbstractSyntaxTree()
    {
        m_Statements = new Queue<AcceptNodeBase>();

        AcceptNodeBase programStart = new AcceptNodeProgram();
        
        m_Statements.Enqueue(programStart);
        
        while (m_Tokens.Count > 0)
        {
            AcceptNodeBase node = ParseStatement();
            m_Statements.Enqueue(node);
        }

        if (m_Statements.Count == 0)
        {
            throw new Exception("General error in Accept Parser");
        }
        
        return m_Statements;
    }

    private AcceptNodeBase ParseStatement()
    {
        Token currentToken = m_Tokens.Peek();

        AcceptNodeBase result = null;

        switch (currentToken.Type)
        {
            case TokenType.SAY:
                result = ParseStatementSay();
                break;
            case TokenType.VAR_DECLARATION:
                result = ParseStatementVarDeclaration();
                break;
            case TokenType.IF:
                result = ParseIfElseStatement();
                break;
            case TokenType.END_OF_PROGRAM:
                result = ParseEndOfProgramStatement();
                break;
            default:
                throw new Exception("Unexpected token type: " + currentToken.Type);
        }

        return result;
    }

    private AcceptNodeBase ParseEndOfProgramStatement()
    {
        AcceptNodeBase endOfProgramStatementNode = new AcceptNodeEndOfProgram();
        
        try
        {
            if (ValidateEndOfProgram())
            {
                return endOfProgramStatementNode;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return endOfProgramStatementNode;
    }

    private bool ValidateEndOfProgram()
    {
        Token lastToken = m_Tokens.Dequeue();
        if (m_Tokens.Count == 0)
        {
            return true;
        }
        
        throw new Exception("There are tokens left after end of program, these would not be interpreted.");
    }

    private AcceptNodeBase ParseIfElseStatement()
    {
        AcceptNodeIfElse ifElseNode = ParseIfBlock();
        ParseElseBlock(ifElseNode);
        
        return ifElseNode;
    }

    private AcceptNodeIfElse ParseIfBlock()
    {
        Token ifToken = m_Tokens.Dequeue();

        if (m_Tokens.TryDequeue(out Token identifierToken) && identifierToken.Type == TokenType.IDENTIFIER)
        {
            if (m_Tokens.TryDequeue(out Token conditionToken) && (conditionToken.Type & IF_ELSE_CONDITIONS) != 0)
            {
                if (m_Tokens.TryDequeue(out Token numberToken) && numberToken.Type == TokenType.NUMBER)
                {
                    if (m_Tokens.TryDequeue(out Token colonToken) && colonToken.Type == TokenType.END_OF_IF_ELSE_LINE)
                    {
                        AcceptNodeIfElse ifElseStatementNode = new AcceptNodeIfElse(identifierToken.Value, conditionToken.Value, numberToken.Value);
                        
                        Queue<AcceptNodeBase> ifBlockStatements = new Queue<AcceptNodeBase>();

                        while (m_Tokens.Count > 0)
                        {
                            Token nextToken = m_Tokens.Peek();

                            if (nextToken.Type == TokenType.ELSE)
                            {
                                break;
                            }
                            
                            AcceptNodeBase currentStatementNode = ParseStatement();
                            ifBlockStatements.Enqueue(currentStatementNode);
                        }
                        
                        ifElseStatementNode.SetTrueBranchStatements(ifBlockStatements);
                        
                        return ifElseStatementNode;
                    }
                }
            }
        }
        
        throw new Exception("Unexpected parsing statement: " + TokenType.IF);
    }
    
    private void ParseElseBlock(AcceptNodeIfElse ifElseNode)
    {
        Token elseToken = m_Tokens.Dequeue();

        if (m_Tokens.TryDequeue(out Token colonToken) && colonToken.Type == TokenType.END_OF_IF_ELSE_LINE)
        {
            Queue<AcceptNodeBase> elseBlockStatements = new Queue<AcceptNodeBase>();

            while (m_Tokens.Count > 0)
            {
                Token nextToken = m_Tokens.Peek();
            
                // this is hard coded because right now we don't have any way to know when if else block ois finished
                if (nextToken.Type == TokenType.END_OF_PROGRAM)
                {
                    break;
                }
            
                AcceptNodeBase currentStatementNode = ParseStatement();
                elseBlockStatements.Enqueue(currentStatementNode);
            }
        
            ifElseNode.SetFalseBranchStatements(elseBlockStatements);
            
        }
        else
        {
            throw new Exception("Unexpected parsing statement: " + TokenType.END_OF_IF_ELSE_LINE);
        }
    }

    private AcceptNodeBase ParseStatementVarDeclaration()
    {
        Token varDclr = m_Tokens.Dequeue();

        if (m_Tokens.TryDequeue(out Token identifierToken) && identifierToken.Type == TokenType.IDENTIFIER)
        {
            if (m_Tokens.TryDequeue(out Token equalsToken) && equalsToken.Type == TokenType.ASSIGN)
            {
                if (m_Tokens.TryDequeue(out Token numberToken) && numberToken.Type == TokenType.NUMBER)
                {
                    if (m_Tokens.TryDequeue(out Token endLineToken) && endLineToken.Type == TokenType.ENDL)
                    {
                        AcceptNodeBase varDeclarationNode = new AcceptNodeVariableDeclaration(identifierToken.Value, numberToken.Value);
                        
                        return varDeclarationNode;
                    }
                    else
                    {
                        throw new Exception("Unexpected token type: Expected: " + TokenType.ENDL);
                    }
                }
                else
                {
                    throw new Exception("Unexpected token type: Expected: " + TokenType.NUMBER);
                }
            }
            else
            {
                throw new Exception("Unexpected token type: Expected: " + TokenType.EQUAL);
            }
        }
        else
        {
            throw new Exception("Unexpected token type: Expected: " + TokenType.IDENTIFIER);
        }
    }

    private AcceptNodeBase ParseStatementSay()
    {
        Token sayToken = m_Tokens.Dequeue();

        if (m_Tokens.TryDequeue(out Token strLiteralToken) && strLiteralToken.Type == TokenType.STRING)
        {
            if (m_Tokens.TryDequeue(out Token endLineToken) && endLineToken.Type == TokenType.ENDL)
            {
                AcceptNodeBase sayNode = new AcceptNodeSay(strLiteralToken.Value);

                return sayNode;
            }
            else
            {
                throw new Exception("Unexpected token type: Expected: " + TokenType.ENDL);
            }
        }
        else
        {
            throw new Exception("Unexpected token type: Expected: " + TokenType.STRING);
        }
    }
}