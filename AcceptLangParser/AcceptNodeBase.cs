namespace AcceptLangParser;

public abstract class AcceptNodeBase : IVisitable
{
    public abstract void Accept(IVisitor visitor);
}