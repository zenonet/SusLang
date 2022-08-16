using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SusLang.Expressions;

namespace SusLang;

public class ExecutionContext : IEnumerable<Expression>
{
    public IEnumerable<Expression> Expressions;

    public Dictionary<Crewmate, byte> Crewmates;

    public Crewmate Selected;
    
    public int Index = 0;
    
    public bool IsRunning;

    public void Continue()
    {
        IsRunning = true;
        

        
        foreach (Expression expression in Expressions.ToArray()[Index..])
        {
            //Index is actually the index of the next expression to be executed
            Index++;

            expression.Execute();
            

            
            if(!IsRunning)
                break;
        }
        Index = 0;
        IsRunning = false;
    }
    
    public ExecutionContext(
        IEnumerable<Expression> expressions
    )
    {
        Expressions = expressions;
        Crewmates = new Dictionary<Crewmate, byte>(Compiler.StandardCrewmates);
    }
    
    
    public ExecutionContext(
        IEnumerable<Expression> expressions,
        Dictionary<Crewmate, byte> crewmates
    )
    {
        Expressions = expressions;
        Crewmates = crewmates;
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