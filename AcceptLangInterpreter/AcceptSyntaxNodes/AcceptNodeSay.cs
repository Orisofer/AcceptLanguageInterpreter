namespace AcceptLangInterpreter;

public class AcceptNodeSay : AcceptNodeBase
{
    public string Value;

    public AcceptNodeSay(string value)
    {
        this.Value = value;
    }
    
    public override void Accept(IVisitor visitor)
    {
        if (visitor is IAcceptExecuteVisitor executeVisitor)
        {
            executeVisitor.Visit(this);
        }
    }
}