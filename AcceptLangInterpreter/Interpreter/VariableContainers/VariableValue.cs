namespace AcceptLangInterpreter;

public struct VariableValue
{
    public Type Type;
    public object Value;

    public VariableValue(Type type, object value)
    {
        Type = type;
        Value = value;
    }

    public T GetValue<T>()
    {
        return (T)Value;
    }

    public bool Is<T>()
    {
        return Type == typeof(T);
    }
}