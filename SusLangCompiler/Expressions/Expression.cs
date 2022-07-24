using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SusLang.Expressions.DefaultExpressions;

namespace SusLang.Expressions
{
    public class Expression
    {
        public string RawExpression;

        public virtual bool Execute() => true;


        protected static Crewmate ParseColor(string code, bool logErrors = true)
        {
            if (code.ToLower().Replace(" ", "") != "he")
                try
                {
                    Crewmate color = Enum.Parse<Crewmate>(code, true);
                    return color;
                }
                catch (Exception)
                {
                    if(logErrors)
                        Compiler.Logging.LogError($"Can't parse color {code}");
                    return Crewmate.Null;
                }
            if (code.ToLower().Replace(" ", "") == "he") return Compiler.SussedColor;
            try
            {
                Crewmate color = Enum.Parse<Crewmate>(code, true);
                return color;
            }
            catch (Exception)
            {
                if(logErrors)
                    Compiler.Logging.LogError($"Can't parse color {code}");
                return Crewmate.Null;
            }

            return Compiler.SussedColor;
        }

        private static readonly Dictionary<string, Type> Patterns = new()
        {
            {@"^(\w+) vented", typeof(ValueModificator)},
            {@"^(\w+) killed", typeof(ValueModificator)},
            {@"^(\w+) wasWithMe", typeof(ValueModificator)},
            {@"^(\w+) didVisual", typeof(ValueModificator)},
            {@"^sus (\w+)", typeof(SusExpression)},
            {@"^emergencyMeeting", typeof(OutputExpression)},
            {@"^report", typeof(OutputExpression)},
            {@"^who\?", typeof(WhoExpression)},
            {@"^\[(?:.|\s)*", typeof(Loop)},
            {@"^\w+ wasWith \w+", typeof(SetterExpression)},
            {@"^breakpoint", typeof(Breakpoint)},

            {@"^(?:\s|\n|\r|\t)+", typeof(DummyExpression)},
            {@"^(?:\/\/.*|(trashtalk).*)", typeof(DummyExpression)},
        };


        public static Expression Parse(ref string code)
        {
            Expression expression = null;
            string restBuffer = code;

            foreach (KeyValuePair<string, Type> pair in Patterns)
            {
                Match match = Regex.Match(code, pair.Key);

                if (!match.Success)
                    continue;
                expression = Activator.CreateInstance(pair.Value) as Expression;

                if (expression is null)
                {
                    Compiler.Logging.LogError($"There was a problem parsing '{Regex.Match(code, $@"[^\s\\]+").Value}'");
                    return null;
                }
                
                
                
                expression.RawExpression = match.Value;
                restBuffer = code.Substring(match.Length);

                //Call the subclasses OnParse callback
                expression.OnParse(ref code);
                

                break;
            }

            if (expression == null)
            {
                Compiler.Logging.LogError($"Couldn't parse '{Regex.Match(code, $@"[^\s\\]+").Value}'");
                return null;
            }

            if (!expression.IsCuttingCode())
                code = restBuffer;
            return expression;
        }

        protected virtual bool OnParse(ref string code)
        {
            return true;
        }

        protected virtual bool IsCuttingCode() => false;
    }
}
