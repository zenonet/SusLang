using System.Collections.Generic;

namespace SusLang;

public class CrewmateList : Dictionary<Crewmate, byte>
{
    public Crewmate Selected;
    public byte ValueOfSelected => this[Selected];
    public CrewmateList(IDictionary<Crewmate, byte> dictionary) : base(dictionary)
    {
    }

    public new byte this[Crewmate index]
    {
        get => index.RefToSelected ? base[Selected] : base[index];
        set
        {
            if(index.RefToSelected)
                base[Selected] = value;
            else
                base[index] = value;
        }
    }
}