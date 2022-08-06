using System;
using System.Text;
using System.Text.RegularExpressions;

namespace SusLang.Expressions.DefaultExpressions
{
    public class OutputExpression : Expression
    {
        private byte outputType;
        private Crewmate target;

        protected override bool IsCuttingCode() => true;

        protected override bool OnParse(ref string code)
        {
            switch (RawExpression)
            {
                case "emergencyMeeting":
                    outputType = 0;
                    break;
                case "report":
                    outputType = 1;
                    break;
                default:
                    Compiler.Logging.LogRaw(
                        "Internal error. Please report this and your code at https://github.com/zenonet/SusLang/issues so that I can fix it!");
                    return false;
            }

            //Optionally specified color:
            string optColor =
                code.Replace(RawExpression, "")
                    .Split(new[] {"\n", "//"}, StringSplitOptions.TrimEntries)[0]
                    .Trim();

            Crewmate color;

            if (optColor.Length > 0)
            {
                color = ParseColor(optColor);
                
                if (code == null)
                    return false;
            }
            else
            {
                color = null;
            }


            //Cut RawExpression:
            code = code.Substring(RawExpression.Length);

            target = color;

            //Cut the specified color out too
            Regex regex = new Regex(@"\s*" + optColor + @"\s*");
            code = regex.Replace(code, "", 1);


            return true;
        }

        public override bool Execute()
        {
            target ??= Compiler.SussedColor;
            switch (outputType)
            {
                case 0: //emergencyMeeting
                    Compiler.Logging.LogProgramOutput(
                        Encoding.ASCII.GetString(new[]
                            {
                                Compiler.Crewmates[target]
                            }
                        )
                    );
                    break;
                case 1: //Report
                    Compiler.Logging.LogProgramOutput(Compiler.Crewmates[target].ToString()
                    );
                    break;
            }

            return true;
        }
    }
}