using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SusLang.Expressions.DefaultExpressions;

namespace SusLang.Expressions
{
    public class Expression
    {
        public Compiler Compiler { get; private set; }
        
        public string RawExpression;

        protected Crewmate[] PreparsedColors;
        
        
        public Dictionary<Crewmate, byte> Crewmates =>
            Compiler.ExecutionContext.Crewmates;

        public virtual bool Execute() => true;

        protected static Crewmate ParseColor(string code, bool logErrors = true)
        {
            //Prepare
            string colorString = code.ToLower().Trim();

            if (colorString is "he" or "him" or "her" or "she")
                return Crewmate.SussedColorRef;

            //Parse the color:

            Crewmate color = Crewmate.Parse(colorString, Compiler);

            if (color != null)
                return color;

            if (logErrors)
            {
                Compiler.Logging.LogError(
                    $"Color not found: {code}\n" +
                    "     Consider defining it using '#define color <name>'");
            }

            return null;
        }

        private static readonly Dictionary<string, Type> Patterns = new()
        {
            {@"^sus (\w+)", typeof(SusExpression)},
            {@"^emergencyMeeting", typeof(OutputExpression)},
            {@"^report", typeof(OutputExpression)},
            {@"^who\?", typeof(WhoExpression)},
            {@"^\[(?:.|\s)*", typeof(Loop)},
            {@"^\w+ wasWith \w+", typeof(SetterExpression)},
            {@"^#define ", typeof(DefineExpression)},
            {@"^breakpoint", typeof(Breakpoint)},

            {@"^(\w+) vented", typeof(ValueModificator)},
            {@"^(\w+) killed", typeof(ValueModificator)},
            {@"^(\w+) wasWithMe", typeof(ValueModificator)},
            {@"^(\w+) didVisual", typeof(ValueModificator)},

            {@"^(?:\s|\n|\r|\t)+", typeof(DummyExpression)},
            {@"^(?:\/\/.*|(trashtalk).*)", typeof(DummyExpression)},

            {@"^(\w+) \w+ ?(\w*)", typeof(CustomKeywordExpression)}
        };

        public static Expression Parse(ref string code, Compiler compiler)
        {
            Expression expression = null;
            string restBuffer = code;

            foreach (KeyValuePair<string, Type> pair in Patterns)
            {
                Match match = Regex.Match(code, pair.Key);

                if (!match.Success)
                    continue;

                Crewmate[] colors = new Crewmate[match.Groups.Count - 1];
                if (match.Groups.Count > 1)
                {
                    List<Crewmate> colorsList = new();
                    for (int i = 1; i < match.Groups.Count; i++)
                    {
                        Group group = match.Groups[i];
                        string color = group.Value.Trim();
                        if (color.Length < 1)
                            continue;

                        Crewmate crewmate = ParseColor(color, false);
                        if (crewmate == null)
                            goto nextPattern;

                        colorsList.Add(crewmate);
                    }
                    colors = colorsList.ToArray();
                }

                expression = Activator.CreateInstance(pair.Value) as Expression;
                
                if (expression is null)
                {
                    Compiler.Logging.LogError($"There was a problem parsing '{Regex.Match(code, $@"[^\s\\]+").Value}'");
                    return null;
                }

                expression.Compiler = compiler;
                
                expression.PreparsedColors = colors;

                expression.RawExpression = match.Value;
                restBuffer = code.Substring(match.Length);

                //Call the subclasses OnParse callback
                bool success = expression.OnParse(ref code);

                if (!success)
                    return null;

                break;

                nextPattern: ;
            }

            if (expression == null)
            {
                Compiler.Logging.LogError($"Couldn't parse '{Regex.Match(restBuffer, $@"[^\s\\]+").Value}'");
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