namespace SusLang.Expressions.DefaultExpressions
{
    public class WhoExpression : Expression
    {
        private Crewmate color;

        public override bool Execute()
        {
            color = ParseColor(Compiler.Logging.WaitForInput(), Context);
            return color is not null;
        }
    }
}