using System.Text;

namespace SusLang.Expressions.DefaultExpressions
{
    public class OutputExpression : Expression
    {
        private byte _outputType;
        protected override bool OnParse(ref string code)
        {
            switch (RawExpression)
            {
                case "emergencyMeeting":
                    _outputType = 0;
                    break;
                default:
                    return false;
            }

            return true;
        }

        public override void Execute()
        {
            switch (_outputType)
            {
                case 0: //emergencyMeeting
                    Compiler.Logging.LogProgramOutput(
                        Encoding.ASCII.GetString(new[]
                            {
                                Compiler.Crewmates[Compiler.SussedColor]
                            }
                        )
                    );
                    break;
                case 1: //Report
                    Compiler.Logging.LogProgramOutput(Compiler.Crewmates[Compiler.SussedColor].ToString()
                    );
                    break;
            }

        }
    }
}