namespace AcceptLangParser;

public class AcceptNodeVariableDeclaration : AcceptNodeBase
{
    public string Identifier;
    public string Number;

    public AcceptNodeVariableDeclaration(string identifier, string number)
    {
        Identifier = identifier;
        Number = number;
    }
    
    public override void Accept(IVisitor visitor)
    {
        if (visitor is IAcceptExecuteVisitor executeVisitor)
        {
            executeVisitor.Visit(this);
        }
    }
}