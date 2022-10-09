using System.Collections.Generic;
using System.Linq;
using SoftCircuits.Collections;
using SusLang.Expressions.DefaultExpressions;

namespace SusLang;

    public class Crewmate
    {
        public const Crewmate Null = null;

        public virtual string Name { get; }

        public static readonly Crewmate RefToSelectedInstance = new();
        
        public bool RefToSelected => ReferenceEquals(this, RefToSelectedInstance);

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

            
            if (parsedCrewmate == null)
            {
                //Check if it might be a pointer referencing
                OrderedDictionary<string,ExecutionContext> keywords = CustomKeywordExpression.CustomKeywords;
                if (keywords.ContainsKey(color))
                {
                    // Check if there is a keyword with the same name as the color
                    if (!keywords.ContainsKey(color))
                        return null;
                    
                    // Get the index of the keyword
                    int index = keywords.IndexOf(color);
                    return new (index.ToString());
                }
            }
            
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