using System.Diagnostics.CodeAnalysis;

namespace SusLang;

public readonly struct Parameters
{
    public readonly Crewmate A;
    public readonly Crewmate B;

    public Parameters()
    {
        A = null;
        B = null;
    }

    public Parameters([DisallowNull] Crewmate a)
    {
        A = a;
        B = null;
    }

    public Parameters([DisallowNull] Crewmate a, [DisallowNull] Crewmate b)
    {
        A = a;
        B = b;
    }
    
    public int Count => A != null && B != null ? 2 : A != null ^ B != null ? 1 : 0;
}