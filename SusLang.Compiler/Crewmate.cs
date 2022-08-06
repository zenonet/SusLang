using System.Linq;

namespace SusLang
{
    public class Crewmate
    {
        public const Crewmate Null = null;


        public readonly string Name;

        private Crewmate(string name)
        {
            Name = name;
        }


        //Parse color
        public static Crewmate Parse(string color)
        {
            Crewmate parsedCrewmate = Compiler.Crewmates.FirstOrDefault(
                x => x.Key.Name == color
            ).Key;
            
            return parsedCrewmate;
        }


        public static implicit operator Crewmate(string color)
        {
            return new Crewmate(color);
        }
    }
}