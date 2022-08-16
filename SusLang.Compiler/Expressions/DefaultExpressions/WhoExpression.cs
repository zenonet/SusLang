namespace SusLang.Expressions.DefaultExpressions
{
    public class WhoExpression : Expression
    {
        private Crewmate color;

        public override bool Execute()
        {
            color = ParseColor(Compiler.Logging.WaitForInput(), Context);
            if (color is null)
                return false;

            return true;
        }
    }
}