namespace AcceptLangParser;

public interface IAcceptExecuteVisitor : IVisitor
{
    public void Visit(AcceptNodeBase programNode);
    
    public void Visit(AcceptNodeProgram programNode);

    public void Visit(AcceptNodeVariableDeclaration varDeclarationNode);

    public void Visit(AcceptNodeSay varDeclarationNode);

    public void Visit(AcceptNodeIfElse varDeclarationNode);

    public void Visit(AcceptNodeEndOfProgram varDeclarationNode);
}