using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SusLang.Expressions
{
    public class Expression
    {
        public ExpressionType Type;
        public string RawExpression;

        public void Execute()
        {
            switch (Type)
            {
                //Set the currently sussed Crewmate
                case ExpressionType.Sus:
                {
                    string rest = RawExpression.Replace("sus", "");
                    Crewmate? _color = ParseColor(rest);
                    if (_color is null)
                        return;
                    Crewmate color = (Crewmate) _color;
                    
                    Compiler.SussedColor = color;
                    break;
                }
                case ExpressionType.Vented:
                {
                    string rest = RawExpression.Replace("vented", "");
                    Crewmate? _color = ParseColor(rest);
                    if (_color is null)
                        return;
                    Crewmate color = (Crewmate) _color;
                    
                    Compiler.Crewmates[color] += 1;
                    break;
                }
                case ExpressionType.Killed:
                {
                    string rest = RawExpression.Replace("killed", "");
                    Crewmate? _color = ParseColor(rest);
                    if (_color is null)
                        return;
                    Crewmate color = (Crewmate) _color;

                    Compiler.Crewmates[color] += 10;
                    break;
                }
                case ExpressionType.WasWithMe:
                {
                    string rest = RawExpression.Replace("wasWithMe", "");
                    Crewmate? _color = ParseColor(rest);
                    if (_color is null)
                        return;
                    Crewmate color = (Crewmate) _color;

                    Compiler.Crewmates[color] -= 1;
                    break;
                }
                case ExpressionType.DidVisual:
                {
                    string rest = RawExpression.Replace("didVisual", "");
                    Crewmate? _color = ParseColor(rest);
                    if (_color is null)
                        return;
                    Crewmate color = (Crewmate) _color;

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
                    Crewmate? _color = ParseColor(Console.ReadLine());
                    if (_color is null)
                        return;

                    Compiler.SussedColor = (Crewmate) _color;
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


        private static Crewmate? ParseColor(string code)
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
                    return null;
                }

            return Compiler.SussedColor;
        }

        private static readonly Dictionary<string, ExpressionType> Patterns = new()
        {
            {@"^(\w+) vented", ExpressionType.Vented},
            {@"^(\w+) killed", ExpressionType.Killed},
            {@"^(\w+) wasWithMe", ExpressionType.WasWithMe},
            {@"^(\w+) didVisual", ExpressionType.DidVisual},
            {@"^sus (\w+)", ExpressionType.Sus},
            {@"^emergencyMeeting", ExpressionType.EmergencyMeeting},
            {@"^who\?", ExpressionType.Who},


            {@"^(?:\s|\n|\r|\t)+", ExpressionType.EmptyLine},
            {@"^(?:\/\/.*|(trashtalk).*)", ExpressionType.Comment},
        };


        public static Expression Parse(string code, out string rest)
        {
            Expression expression = null;
            string restBuffer = code;

            foreach (KeyValuePair<string, ExpressionType> pair in Patterns)
            {
                Match match = Regex.Match(code, pair.Key);

                //TODO: Check if Match.Success is the right thing to do in this situation
                if (!match.Success)
                    continue;
                expression = new Expression
                {
                    Type = pair.Value,
                    RawExpression = match.Value
                };
                restBuffer = code.Substring(match.Length);
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
    }
}