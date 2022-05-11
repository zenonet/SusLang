namespace SusLang.Expressions.DefaultExpressions
{
    public class ValueModificator : Expression
    {
        private short _addent;
        private Crewmate color;
        protected override bool OnParse(ref string code)
        {
            string[] parts = RawExpression.Split(' ');
            
            color = ParseColor(parts[0]);
            if (color is Crewmate.Null)
                return false;
            
            switch (parts[1])
            {
                case "vented":
                {
                    _addent = 1;
                    break;
                }
                case "killed":
                {
                    _addent = 10;
                    break;
                }
                case "wasWithMe":
                {
                    _addent = -1;
                    break;
                }
                case "didVisual":
                {
                    _addent = -10;
                    break;
                }
            }

            return true;
        }

        public override void Execute()
        {
            Compiler.Crewmates[color] = (byte) (Compiler.Crewmates[color] + _addent);
        }
    }
}