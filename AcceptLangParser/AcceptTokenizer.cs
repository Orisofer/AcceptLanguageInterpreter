namespace AcceptLangParser;

public class AcceptTokenizer
{
    private Queue<Token> m_Tokens;
    private Queue<string> m_Lines;

    public AcceptTokenizer(string[] lines)
    {
        m_Lines = new Queue<string>(lines);
    }

    public Queue<Token> GenerateTokens()
    {
        m_Tokens = new Queue<Token>();

        while (m_Lines.Count > 0)
        {
            TokenizeLine();
        }
        
        return m_Tokens;
    }

    private void TokenizeLine()
    {
        Queue<string> tokenStream = RequestLine();

        while (tokenStream.Count > 0)
        {
            TokenizeStatement(tokenStream);
        }
    }
    
    private void TokenizeStatement(Queue<string> tokenStream)
    {
        string word = tokenStream.Peek();
        
        // this is a print statement
        if (word == "say")
        {
            TokenizeSay(tokenStream);
        }
        else if (word.StartsWith("_"))
        {
            TokenizeString(tokenStream);
        }
        else if (word == "mish")
        {
            TokenizeVariableDeclaration(tokenStream);
        }
        else if (word == "im")
        {
            TokenizeIfElse(tokenStream);
        }
        else if (word == "accept")
        {
            TokenizeEndOfProgram(tokenStream);
        }
        else if (word == "%")
        {
            TokenizeEndOfLine(tokenStream);
        }
    }

    private void TokenizeEndOfLine(Queue<string> tokenStream)
    {
        if (tokenStream.TryDequeue(out string endOfLine))
        {
            m_Tokens.Enqueue(new Token(TokenType.ENDL,  endOfLine));
        }
    }

    private void TokenizeSay(Queue<string> tokenStream)
    {
        string word = tokenStream.Dequeue();
        
        m_Tokens.Enqueue(new Token(TokenType.SAY,  word));
    }
    
    private void TokenizeString(Queue<string> tokenStream)
    {
        string word = tokenStream.Dequeue();
        
        word = word.Substring(1);
        word = word.Replace("_", " ");
        
        m_Tokens.Enqueue(new Token(TokenType.STRING,  word));
    }

    private void TokenizeVariableDeclaration(Queue<string> tokenStream)
    {
        string word = tokenStream.Dequeue();
        
        m_Tokens.Enqueue(new Token(TokenType.VAR_DECLARATION, word));
        
        if (tokenStream.TryDequeue(out string identifier))
        {
            m_Tokens.Enqueue(new Token(TokenType.IDENTIFIER, identifier));
        }
        else
        {
            throw new Exception("Invalid variable declaration");
        }
        
        if (tokenStream.TryDequeue(out string assignment))
        {
            if (assignment == "=>")
            {
                m_Tokens.Enqueue(new Token(TokenType.ASSIGN, assignment));
            }
            else
            {
                throw new Exception("Invalid assignment token");
            }
        }
        else
        {
            throw new Exception("Invalid variable declaration");
        }
        
        TokenizeNumber(tokenStream);
        TokenizeEndOfLine(tokenStream);
    }

    private void TokenizeIfElse(Queue<string> tokenStream)
    {
        TokenizeIf(tokenStream);

        // end of line inside if else block, we need a block of statements now
        if (tokenStream.Count == 0)
        {
            // we're requesting the new line of the statements
            while (m_Lines.Count > 0)
            {
                Queue<string> ifElseBlockLineTokenStream = RequestLine();

                if (ifElseBlockLineTokenStream.Peek() == "o")
                {
                    TokenizeElse(ifElseBlockLineTokenStream);
                    break;
                }

                while (ifElseBlockLineTokenStream.Count > 0)
                {
                    TokenizeStatement(ifElseBlockLineTokenStream);
                }
            }
        }
    }

    private void TokenizeElse(Queue<string> tokenStream)
    {
        TokenizeStartOfElse(tokenStream);
        TokenizeColons(tokenStream);
    }

    private void TokenizeStartOfElse(Queue<string> tokenStream)
    {
        if (tokenStream.TryDequeue(out string startElse))
        {
            if (startElse == "o")
            {
                m_Tokens.Enqueue(new Token(TokenType.ELSE, startElse));
                return;
            }
        }
        throw new Exception("Invalid else opening block token");
    }

    private void TokenizeColons(Queue<string> tokenStream)
    {
        if (tokenStream.TryDequeue(out string endIfLine))
        {
            if (endIfLine == ":")
            {
                m_Tokens.Enqueue(new Token(TokenType.END_OF_IF_ELSE_LINE, endIfLine));
                return;
            }
        }
        throw new Exception("Invalid colon after condition");
    }

    private void TokenizeCondition(Queue<string> tokenStream)
    {
        if (tokenStream.TryDequeue(out string condition))
        {
            if (condition == "==")
            {
                m_Tokens.Enqueue(new Token(TokenType.EQUAL, condition));
            }
            else if (condition == ">")
            {
                m_Tokens.Enqueue(new Token(TokenType.GREATER_THAN, condition));
            }
            else if (condition == "<")
            {
                m_Tokens.Enqueue(new Token(TokenType.LESS_THAN, condition));
            }
            else
            {
                throw new Exception("Invalid condition");
            }
        }
    }

    private void TokenizeIf(Queue<string> tokenStream)
    {
        string ifOpener = tokenStream.Dequeue();
        m_Tokens.Enqueue(new Token(TokenType.IF, ifOpener));
        
        TokenizeIdentifier(tokenStream);
        TokenizeCondition(tokenStream);
        TokenizeNumber(tokenStream);
        TokenizeColons(tokenStream);
    }

    private void TokenizeNumber(Queue<string> tokenStream)
    {
        if (tokenStream.TryDequeue(out string number))
        {
            if (int.TryParse(number, out int numInt))
            {
                m_Tokens.Enqueue(new Token(TokenType.NUMBER, number));
            }
            else if (float.TryParse(number, out float numFloat))
            {
                m_Tokens.Enqueue(new Token(TokenType.NUMBER, number));
            }
            else
            {
                throw new Exception("Invalid number");
            }
        }
        else
        {
            throw new Exception("Invalid number");
        }
    }

    private void TokenizeIdentifier(Queue<string> tokenStream)
    {
        if (tokenStream.TryDequeue(out string identifier))
        {
            m_Tokens.Enqueue(new Token(TokenType.IDENTIFIER, identifier));
        }
        else
        {
            throw new Exception("Invalid identifier inside if else block");
        }
    }

    private void TokenizeEndOfProgram(Queue<string> tokenStream)
    {
        if (tokenStream.TryDequeue(out string endOfProgram))
        {
            m_Tokens.Enqueue(new Token(TokenType.END_OF_PROGRAM, endOfProgram));
        }
        else
        {
            throw new Exception("Invalid token inside end of program");
        }
    }

    private Queue<string> RequestLine()
    {
        string line = m_Lines.Dequeue();
        line = line.Replace("%", " %").Replace(":", " :");
        return new Queue<string>(line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }
}