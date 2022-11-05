namespace SusLang;

public class CustomKeyword : IKeyword
{
    public readonly ExecutionContext ExecutionContext;

    public CustomKeyword(ExecutionContext executionContext)
    {
        ExecutionContext = executionContext;
    }

    public void Execute(ExecutionContext context, Parameters parameters)
    {
        
    }
}