namespace SusLang.Expressions.DefaultExpressions
{
    public class SetterExpression : Expression
    {
        private Crewmate color0;
        private Crewmate color1;
        
        protected override bool OnParse(ref string code)
        {
            string[] colors = RawExpression.Split("wasWith");
            color0 = ParseColor(colors[0], Context);
            color1 = ParseColor(colors[1], Context);
            
            return color0 is not null && color1 is not null;
        }

        public override bool Execute()
        {
            Crewmates[color0] = Crewmates[color1];
            return true;
        }
    }
}