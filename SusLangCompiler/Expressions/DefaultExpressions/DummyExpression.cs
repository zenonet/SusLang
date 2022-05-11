namespace SusLang.Expressions.DefaultExpressions
{
    public class DummyExpression : Expression
    {
        protected override bool OnParse(ref string code)
        {
            return true;
        }

        public override void Execute()
        {
            
        }
    }
}