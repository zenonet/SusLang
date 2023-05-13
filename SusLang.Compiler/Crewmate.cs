using System.Linq;

namespace SusLang;

public sealed class Crewmate
{
    public string Name { get; }

    public static readonly Crewmate RefToSelectedInstance = new();

    public bool IsRefToSelected => ReferenceEquals(this, RefToSelectedInstance);

    private Crewmate(string name)
    {
        Name = name;
    }

    private Crewmate()
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
        return new(color);
    }

    public override bool Equals(object obj)
    {
        if (obj is Crewmate crewmate)
        {
            return Name.Equals(crewmate.Name);
        }

        return obj == this;
    }

    private bool Equals(Crewmate other)
    {
        return Name == other.Name;
    }

    public override int GetHashCode()
    {
        return Name != null ? Name.GetHashCode() : 0;
    }
}