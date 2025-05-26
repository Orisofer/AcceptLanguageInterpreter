namespace AcceptLangInterpreter;

public class AcceptNodeProgram : AcceptNodeBase
{
    private List<AcceptNodeBase> Statements;

    public AcceptNodeProgram()
    {
        Statements = new List<AcceptNodeBase>();
    }
    
    public AcceptNodeProgram(List<AcceptNodeBase> statements)
    {
        Statements = statements;
    }
    
    public override void Accept(IVisitor visitor)
    {
        if (visitor is IAcceptExecuteVisitor executeVisitor)
        {
            executeVisitor.Visit(this);
        }
    }
}