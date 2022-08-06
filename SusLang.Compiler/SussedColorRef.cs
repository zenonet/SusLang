namespace SusLang
{
    public class SussedColorRef : Crewmate
    {
        public static SussedColorRef Instance = new ();

        public override string Name => Compiler.SussedColor.Name;
    }
}