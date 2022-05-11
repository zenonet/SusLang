namespace SusLang.Expressions.DefaultExpressions
{
    public class SusExpression : Expression
    {
        private Crewmate _target;
        
        protected override bool OnParse(ref string code)
        {
            _target = ParseColor(RawExpression.Replace("sus", ""));
            
            //If _target is Crewmate.Null, return false
            return _target is not Crewmate.Null;
        }

        public override void Execute()
        {
            Compiler.SussedColor = _target;
        }
    }
}