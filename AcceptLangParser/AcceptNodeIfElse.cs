namespace AcceptLangParser;

public class AcceptNodeIfElse : AcceptNodeBase
{
    public Queue<AcceptNodeBase> StatementsTrueBranch;
    public Queue<AcceptNodeBase> StatementsFalseBranch;
    
    public string Identifier;
    public string Condition;
    public string Value;

    public AcceptNodeIfElse(string identifier, string condition, string value)
    {
        Identifier = identifier;
        Condition = condition;
        Value = value;
    }

    public void SetTrueBranchStatements(Queue<AcceptNodeBase> statementsTrueBranch)
    {
        StatementsTrueBranch = statementsTrueBranch;
    }

    public void SetFalseBranchStatements(Queue<AcceptNodeBase> statementsFalseBranch)
    {
        StatementsFalseBranch = statementsFalseBranch;
    }

    public override void Accept(IVisitor visitor)
    {
        if (visitor is IAcceptExecuteVisitor executeVisitor)
        {
            executeVisitor.Visit(this);
        }
    }
}