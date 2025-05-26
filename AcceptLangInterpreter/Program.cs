namespace AcceptLangInterpreter;

class Program
{
    static void Main(string[] args)
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "AcceptSourceCode", "source.acpt");
        AcceptRunner acptRunner = new AcceptRunner(filePath);
    }
}