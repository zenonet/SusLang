namespace SusLang.Expressions.DefaultExpressions
{
    public class SetterExpression : Expression
    {
        private Crewmate color0;
        private Crewmate color1;
        
        protected override bool OnParse(ref string code)
        {
            string[] colors = RawExpression.Split("wasWith");
            color0 = ParseColor(colors[0]);
            color1 = ParseColor(colors[1]);
            
            return color0 is not Crewmate.Null && color1 is not Crewmate.Null;
        }

        public override bool Execute()
        {
            Compiler.Crewmates[color0] = Compiler.Crewmates[color1];
            return true;
        }
    }
}