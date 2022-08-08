using System.Collections;
using System.Collections.Generic;
using SusLang.Expressions;

namespace SusLang;

public class ExecutionContext : IEnumerable<Expression>
{
    public IEnumerable<Expression> Expressions;

    public Dictionary<Crewmate, byte> Crewmates;

    public ExecutionContext(
        IEnumerable<Expression> expressions,
        Dictionary<Crewmate, byte> crewmates
    )
    {
        Expressions = expressions;
        Crewmates = crewmates;
    }
    
    
    public ExecutionContext(
        IEnumerable<Expression> expressions,
        Compiler compiler
    )
    {
        Expressions = expressions;
        Crewmates = compiler.ExecutionContext.Crewmates;
    }

    
    public IEnumerator<Expression> GetEnumerator()
    {
        return Expressions.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}