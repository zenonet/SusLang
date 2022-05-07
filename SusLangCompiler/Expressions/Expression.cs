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
                    Crewmate? _color = ParseColor(Compiler.Logging.WaitForInput());
                    if (_color is null)
                        return;

                    Compiler.SussedColor = (Crewmate) _color;
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
                    Crewmate? _color0 = ParseColor(colors[0]);
                    Crewmate? _color1 = ParseColor(colors[1]);
                    if (_color0 is null || _color1 is null)
                        return;
                    
                    Crewmate color0 = (Crewmate) _color0;
                    Crewmate color1 = (Crewmate) _color1;

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
            {@"^\[(?:.|\s)*", ExpressionType.Loop},
            {@"^\w+ wasWith \w+", ExpressionType.WasWith},

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