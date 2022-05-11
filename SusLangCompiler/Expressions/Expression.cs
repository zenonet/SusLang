using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SusLang.Expressions.DefaultExpressions;

namespace SusLang.Expressions
{
    public class Expression
    {
        public ExpressionType Type;
        public string RawExpression;

        public virtual void Execute()
        {
            switch (Type)
            {
                //Set the currently sussed Crewmate
                case ExpressionType.Sus:
                {
                    string rest = RawExpression.Replace("sus", "");
                    Crewmate color = ParseColor(rest);
                    if (color is Crewmate.Null)
                        return;
                    
                    Compiler.SussedColor = color;
                    break;
                }
                case ExpressionType.Vented:
                {
                    string rest = RawExpression.Replace("vented", "");
                    Crewmate color = ParseColor(rest);
                    if (color is Crewmate.Null)
                        return;
                    
                    Compiler.Crewmates[color] += 1;
                    break;
                }
                case ExpressionType.Killed:
                {
                    string rest = RawExpression.Replace("killed", "");
                    Crewmate color = ParseColor(rest);
                    if (color is Crewmate.Null)
                        return;

                    Compiler.Crewmates[color] += 10;
                    break;
                }
                case ExpressionType.WasWithMe:
                {
                    string rest = RawExpression.Replace("wasWithMe", "");
                    Crewmate color = ParseColor(rest);
                    if (color is Crewmate.Null)
                        return;

                    Compiler.Crewmates[color] -= 1;
                    break;
                }
                case ExpressionType.DidVisual:
                {
                    string rest = RawExpression.Replace("didVisual", "");
                    Crewmate color = ParseColor(rest);
                    if (color is Crewmate.Null)
                        return;

                    Compiler.Crewmates[color] -= 10;
                    break;
                }
                case ExpressionType.EmergencyMeeting:
                {
                    Compiler.Logging.LogProgramOutput(
                        Encoding.ASCII.GetString(new[]
                            {
                                Compiler.Crewmates[Compiler.SussedColor]
                            }
                        )
                    );
                    break;
                }
                case ExpressionType.Who:
                {
                    Crewmate color = ParseColor(Compiler.Logging.WaitForInput());
                    if (color is Crewmate.Null)
                        return;

                    Compiler.SussedColor = color;
                    break;
                }
                case ExpressionType.Loop:
                {
                    string inside = ParsingUtility.FindBetweenBrackets(ref RawExpression);
                    while (Compiler.Crewmates[Compiler.SussedColor] > 0)
                    {
                        //If the loop wasn't successfully executed, return
                        if(!Compiler.ExecuteInternal(inside))
                            return;
                    }
                    Compiler.ExecuteInternal(RawExpression);
                    break;
                }
                case ExpressionType.WasWith:
                {
                    string[] colors = RawExpression.Split("wasWith");
                    Crewmate color0 = ParseColor(colors[0]);
                    Crewmate color1 = ParseColor(colors[1]);
                    if (color0 is Crewmate.Null || color1 is Crewmate.Null)
                        return;
                    
                    Compiler.Crewmates[color0] = Compiler.Crewmates[color1];
                    
                    break;
                }


                case ExpressionType.Comment:
                {
                    break;
                }
                case ExpressionType.EmptyLine:
                {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        protected static Crewmate ParseColor(string code)
        {
            if (code.ToLower().Replace(" ", "") != "he")
                try
                {
                    Crewmate color = Enum.Parse<Crewmate>(code, true);
                    return color;
                }
                catch (Exception)
                {
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
            {@"^who\?", typeof(WhoExpression)},
            {@"^\[(?:.|\s)*", typeof(Loop)},
            {@"^\w+ wasWith \w+", typeof(SetterExpression)},

            {@"^(?:\s|\n|\r|\t)+", typeof(DummyExpression)},
            {@"^(?:\/\/.*|(trashtalk).*)", typeof(DummyExpression)},
        };


        public static Expression Parse(string code, out string rest)
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
                    rest = code;
                    return null;
                }
                
                
                
                expression.RawExpression = match.Value;
                restBuffer = code.Substring(match.Length);

                //Call the subclasses OnParse callback
                expression.OnParse(ref code);
            }

            if (expression == null)
            {
                Compiler.Logging.LogError($"Couldn't parse '{Regex.Match(code, $@"[^\s\\]+").Value}'");
                rest = code;
                return null;
            }

            rest = restBuffer;


            return expression;
        }

        protected virtual bool OnParse(ref string code)
        {
            return false;
        }
    }
}