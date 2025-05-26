namespace AcceptLangInterpreter;

public abstract class AcceptNodeBase : IVisitable
{
    public abstract void Accept(IVisitor visitor);
}