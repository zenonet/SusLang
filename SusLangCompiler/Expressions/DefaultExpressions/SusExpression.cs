namespace SusLang.Expressions.DefaultExpressions
{
    public class SusExpression : Expression
    {
        private Crewmate target;
        
        protected override bool OnParse(ref string code)
        {
            target = ParseColor(RawExpression.Replace("sus", ""));
            
            //If _target is Crewmate.Null, return false
            return target is not Crewmate.Null;
        }

        public override bool Execute()
        {
            Compiler.SussedColor = target;
            return true;
        }
    }
}