namespace AcceptLangInterpreter;

class Program
{
    static void Main(string[] args)
    {
        // ----- THIS IS AN EXAMPLE CODE PROVIDED INSIDE THE PROJECT FILES ----
        
        string baseDir = AppContext.BaseDirectory;
        string projectRoot = Path.GetFullPath(Path.Combine(baseDir, "..", "..", ".."));
        string sourcePath = Path.Combine(projectRoot, "Data", "AcceptSourceCode", "source.acpt");
        
        //---------------------------------------------------------------------
        
        AcceptRunner acptRunner = new AcceptRunner(sourcePath);
        
        acptRunner.RunProgram();
    }
}