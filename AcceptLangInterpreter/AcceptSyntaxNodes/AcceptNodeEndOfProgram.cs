namespace AcceptLangInterpreter;

public class AcceptNodeEndOfProgram : AcceptNodeBase
{
    public override void Accept(IVisitor visitor)
    {
        if (visitor is IAcceptExecuteVisitor executeVisitor)
        {
            executeVisitor.Visit(this);
        }
    }
}