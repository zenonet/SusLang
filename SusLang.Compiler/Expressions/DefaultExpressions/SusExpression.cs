namespace SusLang.Expressions.DefaultExpressions
{
    public class SusExpression : Expression
    {
        private Crewmate target;
        
        protected override bool OnParse(ref string code)
        {
            target = ParseColor(RawExpression[3..], Context);
            
            //If _target is null, return false
            return target is not null;
        }

        public override bool Execute()
        {
            Context.Selected = target;
            return true;
        }
    }
}