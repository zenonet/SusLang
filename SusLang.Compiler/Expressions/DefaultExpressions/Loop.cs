using System.Collections.Generic;
using System.Diagnostics;
using SusLang.CodeAnalysis;

namespace SusLang.Expressions.DefaultExpressions
{
    public class Loop : Expression
    {
        private Queue<Expression> expressions;

        protected override bool IsCuttingCode() => true;

        protected override bool OnParse(ref string code)
        {
            expressions = new();
            
            //Remove the opening bracket
            code = code[1..];

            string inside = ParsingUtility.FindBetweenBrackets(ref code);
            while (inside.Length > 0)
            {
                Expression expression = Parse(ref inside, Context);
                if (expression is null)
                    return false;

                expressions.Enqueue(expression);
            }

            return true;
        }

        public override bool Execute()
        {
            Stopwatch sw = Stopwatch.StartNew();
            while (Crewmates[Selected] > 0)
            {
                if (Compiler.LoopTimeout != -1 && sw.Elapsed.TotalSeconds > Compiler.LoopTimeout)
                    Compiler.Logging.LogError(new(Context, "Loop timed out", InspectionSeverity.Error, Context.LineNumber));
                    
                foreach (Expression expression in expressions)
                {
                    if (!expression.Execute())
                        return false;
                }
            }

            return true;
        }

        public override void SetContextRecursively(ExecutionContext @new)
        {
            base.SetContextRecursively(@new);
            foreach (Expression expression in expressions)
            {
                expression.SetContextRecursively(@new);
            }
        }
    }
}