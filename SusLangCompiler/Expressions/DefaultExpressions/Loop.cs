using System.Collections.Generic;

namespace SusLang.Expressions.DefaultExpressions
{
    public class Loop : Expression
    {
        private readonly Queue<Expression> _expressions = new();
        protected override bool OnParse(ref string code)
        {
            string inside = ParsingUtility.FindBetweenBrackets(ref code);
            while (inside.Length > 0)
            {
                Expression expression = Expression.Parse(inside, out inside);
                if (expression is null)
                    return false;
                
                _expressions.Enqueue(expression);
            }

            return true;

        }

        public override bool Execute()
        {
            while (Compiler.Crewmates[Compiler.SussedColor] > 0)
            {
                foreach (Expression expression in _expressions)
                {
                    if (!expression.Execute())
                        return false;
                }
            }

            return true;
        }
    }
}