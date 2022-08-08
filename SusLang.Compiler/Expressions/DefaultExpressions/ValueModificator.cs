namespace SusLang.Expressions.DefaultExpressions
{
    public class ValueModificator : Expression
    {
        private short addent;
        private Crewmate color;
        protected override bool OnParse(ref string code)
        {
            string[] parts = RawExpression.Split(' ');
            
            color = ParseColor(parts[0]);
            if (color is null)
                return false;
            
            switch (parts[1])
            {
                case "vented":
                {
                    addent = 1;
                    break;
                }
                case "killed":
                {
                    addent = 10;
                    break;
                }
                case "wasWithMe":
                {
                    addent = -1;
                    break;
                }
                case "didVisual":
                {
                    addent = -10;
                    break;
                }
            }

            return true;
        }

        public override bool Execute()
        {
            Crewmates[color] = (byte) (Crewmates[color] + addent);
            return true;
        }
    }
}