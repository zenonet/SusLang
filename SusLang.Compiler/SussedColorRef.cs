namespace SusLang
{
    public class SussedColorRef : Crewmate
    {
        public ExecutionContext Context;
        
        public static SussedColorRef Instance = new ();

        public override string Name => Context.Selected.Name;
    }
}