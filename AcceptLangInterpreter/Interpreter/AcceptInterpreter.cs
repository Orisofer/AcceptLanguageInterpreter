using System.Threading.Channels;

namespace AcceptLangInterpreter;

public class AcceptInterpreter : IAcceptExecuteVisitor
{
    private readonly Queue<AcceptNodeBase> m_Statements;

    private Dictionary<string, VariableValue> m_Variables;

    public AcceptInterpreter(Queue<AcceptNodeBase> statements)
    {
        m_Statements = statements;
        
        m_Variables = new Dictionary<string, VariableValue>();
    }

    public void RunProgram()
    {
        while (m_Statements.Count > 0)
        {
            AcceptNodeBase node = m_Statements.Dequeue();
            IVisitable visitableNode = node as IVisitable;
            visitableNode.Accept(this);
        }
    }

    public void Visit(AcceptNodeProgram programNode)
    {
        Console.WriteLine("Program Started");
    }

    public void Visit(AcceptNodeVariableDeclaration varDeclarationNode)
    {
        string varName = varDeclarationNode.Identifier;

        if (int.TryParse(varDeclarationNode.Number, out int intNum))
        {
            m_Variables.Add(varName, new VariableValue(typeof(int), intNum));
        }
        else if (float.TryParse(varDeclarationNode.Number, out float floatNum))
        {
            m_Variables.Add(varName, new VariableValue(typeof(float), floatNum));
        }
    }

    public void Visit(AcceptNodeSay varDeclarationNode)
    {
        string whatToSay = varDeclarationNode.Value;
        Console.WriteLine(whatToSay);
    }

    public void Visit(AcceptNodeIfElse varDeclarationNode)
    {
        string varName = varDeclarationNode.Identifier;
        
        if (TryGetVariableValue(varName, out object variableValue))
        {
            Queue<AcceptNodeBase> ifElseBlockToExecute = new Queue<AcceptNodeBase>();
            
            if (variableValue is int)
            {
                ifElseBlockToExecute = EvaluateIfElse<int>(variableValue, varDeclarationNode);
            }
            else if(variableValue is float)
            {
                ifElseBlockToExecute = EvaluateIfElse<float>(variableValue, varDeclarationNode);
            }
            
            while (ifElseBlockToExecute.Count > 0)
            {
                AcceptNodeBase node = ifElseBlockToExecute.Dequeue();
                IVisitable visitableNode = node as IVisitable;
                visitableNode.Accept(this);
            }
        }
        else
        {
            throw new Exception("Error running if-else block");
        }
    }

    public void Visit(AcceptNodeEndOfProgram varDeclarationNode)
    {
        Console.WriteLine("Program Finished");
    }
    
    public void Visit(AcceptNodeBase programNode)
    {
        // no-op
    }

    private bool TryGetVariableValue(string variableName, out object value)
    {
        if (m_Variables.TryGetValue(variableName, out VariableValue varValue))
        {
            if (varValue.Is<int>())
            {
                value = varValue.GetValue<int>();
                return true;
            }
            else if (varValue.Is<float>())
            {
                value = varValue.GetValue<float>();
                return true;
            }
        }
        throw new Exception("Trying to fetch an unknown variable");
    }
    
    private Queue<AcceptNodeBase> EvaluateIfElse<T>(object variableValue, AcceptNodeIfElse node) where T : IComparable<T>
    {
        bool isTrue = true;
        T right = (T)Convert.ChangeType(variableValue, typeof(T));
        T left = (T)Convert.ChangeType(node.Value, typeof(T));

        switch (node.Condition)
        {
            case ">":
                isTrue = right.CompareTo(left) > 0;
                break;
            case "<":
                isTrue = right.CompareTo(left) < 0;
                break;
            case "==":
                isTrue = right.CompareTo(left) == 0;
                break;
        }
        
        Queue<AcceptNodeBase> result = new Queue<AcceptNodeBase>();

        if (isTrue)
        {
            result = node.StatementsTrueBranch;
        }
        else
        {
            result = node.StatementsFalseBranch;
        }

        return result;
    }
}