namespace AcceptLangInterpreter;

public class AcceptRunner
{
    private Queue<Token> m_Tokens;
    private Queue<AcceptNodeBase> m_Statements;
    private string m_CodePath;

    public AcceptRunner(string codePath)
    {
        m_CodePath = codePath;
    }

    public void RunProgram()
    {
        string[] lines = OpenSourceFile(m_CodePath);

        AcceptTokenizer tokenizer = new AcceptTokenizer(lines);

        m_Tokens = tokenizer.GenerateTokens();
        
        AcceptParser parser = new AcceptParser(m_Tokens);
        
        m_Statements = parser.BuildAbstractSyntaxTree();
        
        AcceptInterpreter interpreter = new AcceptInterpreter(m_Statements);
        
        interpreter.RunProgram();
    }

    private string[] OpenSourceFile(string codePath)
    {
        StreamReader streamReader = new StreamReader(codePath);
        string[] lines = streamReader.ReadToEnd().Split('\n');
        
        return lines;
    }
}