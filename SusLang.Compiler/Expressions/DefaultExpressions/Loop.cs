using System.Collections.Generic;

namespace SusLang.Expressions.DefaultExpressions
{
    public class Loop : Expression
    {
        private readonly Queue<Expression> expressions = new();


        protected override bool IsCuttingCode() => true;
        protected override bool OnParse(ref string code)
        {
            //Remove the opening bracket
            code = code[1..];
            
            string inside = ParsingUtility.FindBetweenBrackets(ref code);
            while (inside.Length > 0)
            {
                Expression expression = Expression.Parse(ref inside, Context);
                if (expression is null)
                    return false;
                
                expressions.Enqueue(expression);
            }

            return true;

        }

        public override bool Execute()
        {
            while (Crewmates[Selected] > 0)
            {
                foreach (Expression expression in expressions)
                {
                    if (!expression.Execute())
                        return false;
                }
            }

            return true;
        }
    }
}