namespace SusLang;

public interface IKeywordDefinition
{
    public int ParameterCount { get; }
    public IKeyword CreateKeyword();
}