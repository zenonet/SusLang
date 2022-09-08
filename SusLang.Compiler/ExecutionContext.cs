using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SusLang.CodeAnalysis;
using SusLang.Expressions;

namespace SusLang;

public class ExecutionContext : IEnumerable<Expression>
{
    public IEnumerable<Expression> Expressions;

    public Dictionary<Crewmate, byte> Crewmates;

    public Crewmate Selected;
    
    public int Index = 0;

    public int LineNumber;
    
    public bool IsRunning;
    
    public List<Diagnosis> Diagnoses = new();

    public Crewmate[] Parameters = null;

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

    /// <summary>
    /// Creates a new ExecutionContext that contains the Expressions of this one but isn't changed in any other way.
    /// </summary>
    /// <returns></returns>
    public ExecutionContext CloneAsNew()
    {
        return new ExecutionContext(Expressions);
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