using System;
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
                case "report":
                    _outputType = 1;
                    break;
                default:
                    Compiler.Logging.LogRaw(
                        "Internal error. Please report this and your code at https://github.com/zenonet/SusLang/issues so that I can fix it!");
                    return false;
            }

            return true;
        }

        public override bool Execute()
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
            return true;
        }
    }
}