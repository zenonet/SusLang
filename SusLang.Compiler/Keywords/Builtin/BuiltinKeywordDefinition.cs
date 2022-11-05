namespace SusLang;

/// <summary>
/// Represents a SusLang keyword definition that is built into the language using C# code.
/// </summary>
public class BuiltinKeywordDefinition : IKeywordDefinition
{
    public readonly BuiltinKeywordDelegate Delegate;

    public BuiltinKeywordDefinition(BuiltinKeywordDelegate @delegate, int parameterCount = 0)
    {
        Delegate = @delegate;
        ParameterCount = parameterCount;
    }

    public int ParameterCount { get; }

    public IKeyword CreateKeyword()
    {
        return new BuiltinKeyword(Delegate);
    }
}

public delegate void BuiltinKeywordDelegate(ExecutionContext context, Parameters parameters);