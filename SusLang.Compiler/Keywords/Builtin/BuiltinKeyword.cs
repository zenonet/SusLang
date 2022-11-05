namespace SusLang;

public class BuiltinKeyword : IKeyword
{
    public readonly BuiltinKeywordDelegate KeywordDelegate;

    public BuiltinKeyword(BuiltinKeywordDelegate keywordDelegate)
    {
        KeywordDelegate = keywordDelegate;
    }

    public void Execute(ExecutionContext context, Parameters parameters)
    {
        KeywordDelegate.Invoke(context, parameters);
    }
}