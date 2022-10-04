namespace SusLang;

/// <summary>
/// This is a VERY SUS piece of code.
/// Basically, when it's being compared to other crewmates, it pretends to be the currently selected crewmate.
/// </summary>
public class SussedColorRef : Crewmate
{
    public ExecutionContext Context;
        
    public static SussedColorRef Instance = new ();

    public override string Name => Context.Selected.Name;
}