namespace AcceptLangInterpreter;

public interface IVisitable
{
    public void Accept(IVisitor visitor);
}