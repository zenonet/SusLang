namespace SusLang;

/// <summary>
/// Represents a SusLang keyword definition that is defined in SusLang code.
/// </summary>
public class CustomKeywordDefinition : IKeywordDefinition
{
    public readonly ExecutionContext ExecutionContext;

    public CustomKeywordDefinition(ExecutionContext executionContext, int parameterCount = 0)
    {
        ExecutionContext = executionContext;
        ParameterCount = parameterCount;
    }

    public int ParameterCount { get; }

    public IKeyword CreateKeyword()
    {
        return new CustomKeyword(ExecutionContext.CloneAsNew());
    }
}