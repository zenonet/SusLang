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


            if (!IsRunning)
                break;
        }

        Index = 0;
        IsRunning = false;
    }

    /// <summary>
    /// Executes expressions in the current ExecutionContext.
    /// </summary>
    /// <param name="context">The context to use the expressions from</param>
    public void ExecuteInThisContext(ExecutionContext context)
    {
        Expression[] array = context.Expressions.ToArray();
        Expression[] expressions = new Expression[array.Length];

        array.CopyTo(expressions, 0);

        foreach (Expression expression in expressions)
        {
            expression.Execute();
        }
    }

    /// <summary>
    /// Executes expressions in the current ExecutionContext.
    /// </summary>
    /// <param name="expressions">An array of expressions to execute</param>
    public void ExecuteInThisContext(Expression[] expressions)
    {
        Expression[] copiedExpressions = new Expression[expressions.Length];
        expressions.CopyTo(copiedExpressions, 0);

        //Change the context to this one
        foreach (Expression copiedExpression in copiedExpressions)
        {
            copiedExpression.SetContextRecursively(this);
        }

        foreach (Expression expression in copiedExpressions)
        {
            expression.Execute();

            if (!IsRunning)
                break;
        }
    }

    /// <summary>
    /// Creates a new ExecutionContext that contains the Expressions and Parameters
    /// of this one but isn't changed in any other way.
    /// </summary>
    /// <returns></returns>
    public ExecutionContext CloneAsNew()
    {
        ExecutionContext @new = new ExecutionContext(Expressions)
        {
            Parameters = (Crewmate[]) Parameters.Clone(),
        };

        foreach (Expression expression in @new.Expressions)
        {
            expression.SetContextRecursively(@new);
        }

        return @new;
    }

    public ExecutionContext(
        IEnumerable<Expression> expressions
    )
    {
        Expressions = expressions;
        Crewmates = new Dictionary<Crewmate, byte>(Compiler.StandardCrewmates);
        Selected = Crewmates.Keys.First();
    }


    public ExecutionContext(
        IEnumerable<Expression> expressions,
        Dictionary<Crewmate, byte> crewmates
    )
    {
        Expressions = expressions;
        Crewmates = crewmates;
        Selected = Crewmates.Keys.First();
    }

    /// <summary>
    /// Adds another contexts expressions to the end of this one.
    /// </summary>
    /// <param name="context">The context to add</param>
    public void AddStart(ExecutionContext context)
    {
        List<Expression> expressions = Expressions.ToList();
        expressions.AddRange(context.Expressions);
        Expressions = expressions;
    }

    /// <summary>
    /// Adds another contexts expressions to the end of this one.
    /// </summary>
    /// <param name="context">The context to add</param>
    public void AddEnd(ExecutionContext context)
    {
        List<Expression> expressions = context.Expressions.ToList();
        expressions.AddRange(Expressions);
        Expressions = expressions;
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