using System.Linq;

namespace SusLang;

    public class Crewmate
    {
        public const Crewmate Null = null;

        public virtual string Name { get; }

        public static readonly Crewmate RefToSelectedInstance = new();
        
        // ReSharper disable once PossibleUnintendedReferenceComparison
        public bool RefToSelected => this == RefToSelectedInstance;

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

            /*
            if (parsedCrewmate == null)
            {
                //Check if it might be a pointer referencing
                Dictionary<string,ExecutionContext> keywords = CustomKeywordExpression.CustomKeywords;
                if (keywords.ContainsKey(color))
                {
                    //Return the index of the keyword in CustomKeywordExpression.CustomKeywords
                    ExecutionContext keyword 
                }
            }
            */
            return parsedCrewmate;
        }

        public override string ToString()
        {
            return "Crewmate: " + Name;
        }

        public static implicit operator Crewmate(string color)
        {
            return new (color);
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