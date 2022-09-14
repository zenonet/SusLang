using System;
using System.Linq;

namespace SusLang
{
    public class Crewmate
    {
        public const Crewmate Null = null;

        public static readonly Crewmate SussedColorRef = SusLang.SussedColorRef.Instance;
        

        public virtual string Name { get; }

        private Crewmate(string name)
        {
            Name = name;
        }

        protected Crewmate()
        {
        }


        //Parse color
        public static Crewmate Parse(string color, ExecutionContext context)
        {
            Crewmate parsedCrewmate = context.Crewmates.FirstOrDefault(
                x => x.Key.Name == color
            ).Key;
            
            return parsedCrewmate;
        }

        public override string ToString()
        {
            return "Crewmate: " + Name;
        }

        public static implicit operator Crewmate(string color)
        {
            return new Crewmate(color);
        }

        public override bool Equals(object obj)
        {
            if (obj is Crewmate crewmate)
            {
                return Name.Equals(crewmate.Name);
            }
            
            return obj == this;
        }

        protected bool Equals(Crewmate other)
        {
            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }
    }
}